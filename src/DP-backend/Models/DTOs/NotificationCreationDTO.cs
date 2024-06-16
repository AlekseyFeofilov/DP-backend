using DP_backend.Domain.Employment;

namespace DP_backend.Models.DTOs
{
    public class NotificationCreationDTO
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public Guid AddresseeId { get; set; }

        public string Link { get; set; }

        public NotificationType? Type { get; set; } = NotificationType.Other;
    }
}
