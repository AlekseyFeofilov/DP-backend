using DP_backend.Domain.Employment;

namespace DP_backend.Models.DTOs
{
    public class NotificationDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public Guid AddresseeId { get; set; }

        public string Link { get; set; }

        public NotificationType Type { get; set; }

        public DateTime CreateDateTime { get; set; }

        public NotificationDTO() { }

        public NotificationDTO(Notification notification)
        {
            Id = notification.Id;
            Title = notification.Title;
            Message = notification.Message;
            AddresseeId = notification.AddresseeId;
            Link = notification.Link;
            Type = notification.Type;
            CreateDateTime = notification.CreateDateTime;
        }
    }
}
