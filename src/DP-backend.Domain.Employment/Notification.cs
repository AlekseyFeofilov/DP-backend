using DP_backend.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DP_backend.Domain.Employment
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public Guid AddresseeId { get; set; }

        public string Link { get; set; }

        public NotificationType Type { get; set; }

        public bool IsRead { get; set; } = false;
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum NotificationType
    {
        Other = 0,
    }
}
