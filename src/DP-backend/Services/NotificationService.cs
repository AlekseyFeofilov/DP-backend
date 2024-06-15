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

        public async Task<List<NotificationDTO>> GetUserNotifications(Guid userId)
        {
            var user = await _context.Users.GetUndeleted().FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new BadDataException($"Пользователь {userId} не найден");
            }

            var notifications = await _context.Notifications.GetUndeleted()
                .Where(n => n.AddresseeId == userId).ToListAsync();

            await ReadNotification(notifications);

            return notifications.Select(n => new NotificationDTO(n)).ToList();
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
