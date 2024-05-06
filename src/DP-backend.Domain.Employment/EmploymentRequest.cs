using DP_backend.Common;

namespace DP_backend.Domain.Employment;

public class EmploymentRequest : BaseEntity
{
    public Guid InternshipRequestId { get; set; }
    public Guid StudentId { get; set; }
    public InternshipRequest InternshipRequest { get; set; }
    public EmploymentRequestStatus Status { get; set; }
}

public enum EmploymentRequestStatus
{
    NonVerified = 1,
    Accepted = 2,
    Declined = 3
}