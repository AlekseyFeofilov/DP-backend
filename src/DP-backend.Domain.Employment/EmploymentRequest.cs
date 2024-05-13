using DP_backend.Common;
using System.Text.Json.Serialization;

namespace DP_backend.Domain.Employment;

public class EmploymentRequest : BaseEntity
{
    public Guid InternshipRequestId { get; set; }
    public Guid StudentId { get; set; }
    public InternshipRequest InternshipRequest { get; set; }
    public EmploymentRequestStatus Status { get; set; }
}
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EmploymentRequestStatus
{
    NonVerified = 1,
    Accepted = 2,
    Declined = 3,
    UnActual = 4
}