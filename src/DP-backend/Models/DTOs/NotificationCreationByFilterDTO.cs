using DP_backend.Common.Enumerations;
using DP_backend.Domain.Employment;

namespace DP_backend.Models.DTOs
{
    public class NotificationCreationByFilterDTO
    {
        public List<StudentStatus>? Statuses { get; set; }

        public List<Grade>? Сourses { get; set; }

        public string Text { get; set; }
    }
}
