﻿using DP_backend.Common;
using DP_backend.Common.Exceptions;
using DP_backend.Database;
using DP_backend.Domain.Employment;
using DP_backend.Domain.Identity;
using DP_backend.Helpers;
using DP_backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DP_backend.Services
{
    public interface INotificationService
    {
        Task Create(NotificationCreationDTO creationDTO);
        Task CreateByFilter(NotificationCreationByFilterDTO notificationFilter, Guid userId);
        Task CreateNotificationForStaff(NotificationCreationDTO creationDTO);
        Task<List<NotificationDTO>> GetUserNotifications(Guid userId);
        Task<int> GetCountUnreadNotifications(Guid userId);
        Task Delete(Guid notificationId, Guid userId);
    }

    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task Create(NotificationCreationDTO creationDTO)
        {
            var user = await _context.Users.GetUndeleted().FirstOrDefaultAsync(u => u.Id == creationDTO.AddresseeId);
            if (user == null)
            {
                throw new BadDataException($"Пользователь {creationDTO.AddresseeId} не найден");
            }
            var notification = new Notification
            {
                Title = creationDTO.Title,
                Message = creationDTO.Message,
                AddresseeId = creationDTO.AddresseeId,
                Link = creationDTO.Link,
                Type = creationDTO.Type ?? NotificationType.Other,
            };
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task CreateByFilter(NotificationCreationByFilterDTO notificationFilter, Guid userId)
        {
            var author = await _context.Users.GetUndeleted().FirstOrDefaultAsync(u => u.Id == userId);
            if (author == null)
            {
                throw new BadDataException($"Пользователь-отправитель {userId} не найден");
            }

            var studentQuery = _context.Students.GetUndeleted();
            if (notificationFilter.Statuses != null)
            {
                studentQuery = studentQuery.Where(s => notificationFilter.Statuses.Contains(s.Status));
            }
            if (notificationFilter.Сourses != null)
            {
                studentQuery = studentQuery.Include(s => s.Group).Where(s => notificationFilter.Сourses.Contains(s.Group.Grade));
            }

            var students = await studentQuery.ToListAsync();
            var newNotifications = students.Select(s => new Notification
            {
                Title = $"Новое уведомление от {author.UserName}",
                Message = notificationFilter.Text,
                AddresseeId = s.Id,
                Link = "http://dp-student.alexfil888.fvds.ru/",
                Type = NotificationType.Other,
            });
            await _context.Notifications.AddRangeAsync(newNotifications);
            await _context.SaveChangesAsync();
        }

        public async Task CreateNotificationForStaff(NotificationCreationDTO creationDTO)
        {
            var allStaff = await _context.Users.GetUndeleted()
                .Include(r => r.Roles)
                .Where(u => u.Roles.Any(r => r.Role.Name == ApplicationRoles.Staff.ToString()))
                .ToListAsync();
            var newNotifications = allStaff.Select(s => new Notification
            {
                Title = creationDTO.Title,
                Message = creationDTO.Message,
                AddresseeId = s.Id,
                Link = creationDTO.Link,
                Type = creationDTO.Type ?? NotificationType.Other,
            });
            await _context.Notifications.AddRangeAsync(newNotifications);
            await _context.SaveChangesAsync();
        }

        public async Task<List<NotificationDTO>> GetUserNotifications(Guid userId)
        {
            var user = await _context.Users.GetUndeleted().FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new BadDataException($"Пользователь {userId} не найден");
            }

            var notifications = await _context.Notifications.GetUndeleted()
                .Where(n => n.AddresseeId == userId)
                .OrderByDescending(t => t.CreateDateTime)
                .ToListAsync();
            var forReturn = notifications.Select(n => new NotificationDTO(n)).ToList();

            await ReadNotification(notifications);

            return forReturn;
        }

        public async Task<int> GetCountUnreadNotifications(Guid userId)
        {
            var count = await _context.Notifications.GetUndeleted()
                .Where(n => n.AddresseeId == userId && n.IsRead == false).CountAsync();

            return count;
        }

        public async Task Delete(Guid notificationId, Guid userId)
        {
            var notification = await _context.Notifications.GetUndeleted().FirstOrDefaultAsync(u => u.Id == notificationId);
            if (notification == null)
            {
                throw new NotFoundException($"Уведомление {notificationId} не найдено");
            }

            if (userId != notification.AddresseeId)
            {
                throw new NoPermissionException("Нельзя удалить чужое уведомление");
            }
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }

        private async Task ReadNotification(List<Notification> notifications)
        {
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }
            await _context.SaveChangesAsync();
        }
    }
}
