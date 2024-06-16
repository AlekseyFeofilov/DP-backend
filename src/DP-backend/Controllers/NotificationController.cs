using DP_backend.Domain.Employment;
using DP_backend.Helpers;
using DP_backend.Models.DTOs;
using DP_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DP_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "StaffAndStudent")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<IActionResult> CreateNotification(NotificationCreationDTO creationDTO)
        {
            await _notificationService.Create(creationDTO);
            return Ok();
        }

        [HttpGet]
        [Route("my")]
        [ProducesResponseType(typeof(List<NotificationDTO>), 200)]
        public async Task<ActionResult<List<NotificationDTO>>> GetNotifications()
        {
            var notifications = await _notificationService.GetUserNotifications(User.GetUserId());
            return Ok(notifications);
        }

        /// <summary>
        /// Получить количество непрочитанных уведомлений
        /// </summary>
        [HttpGet]
        [Route("my/unread")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<ActionResult<int>> GetCountUnreadNotifications()
        {
            var count = await _notificationService.GetCountUnreadNotifications(User.GetUserId());
            return Ok(count);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteNotification(Guid id)
        {
            await _notificationService.Delete(id, User.GetUserId());
            return Ok();
        }
    }
}
