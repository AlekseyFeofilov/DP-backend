using DP_backend.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DP_backend.Domain.Employment
{
    /// <summary>
    /// Заявка для для курсовых и ВКР
    /// </summary>
    public class CourseWorkRequest : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public CourseWorkRequestStatus Status { get; set; }
        public int Semester { get; set; }
        public int? Grade { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CourseWorkRequestStatus
    {
        No = 1,
        Passed = 2,
        Rated = 3
    }
}
