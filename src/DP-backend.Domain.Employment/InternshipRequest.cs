using DP_backend.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DP_backend.Domain.Employment
{
    public class InternshipRequest : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public required Employer Employer { get; set; }
        public required string Vacancy { get; set; }
        public string? Comment { get; set; }
        public InternshipStatus Status { get; set; } = InternshipStatus.NonVerified;

    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum InternshipStatus
    {
        NonVerified = 1,
        Accepted = 2,
        Unactual=3,
        Declined=4
    }
}
