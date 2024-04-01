using DP_backend.Common;

namespace DP_backend.Domain.Employment;

public class Employment : BaseEntity
{
    public required Guid StudentId { get; init; }
    public required EmployerVariant Employer { get; set; }
    public EmploymentStatus Status { get; set; } = EmploymentStatus.NonVerified;
}

public enum EmploymentStatus
{
    NonVerified = 1,
    Verified = 2
}