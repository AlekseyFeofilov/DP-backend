using DP_backend.Common;
using System.Text.Json.Serialization;

namespace DP_backend.Domain.Employment;

public class Employment : BaseEntity
{
    public required Guid StudentId { get; init; }

    public Guid? InternshipRequestId { get; init; }
    public InternshipRequest? InternshipRequest { get; init; }

    public Guid? EmploymentRequestId { get; init; }
    public EmploymentRequest? EmploymentRequest { get; init; }

    public Student Student { get; init; }
    public required Employer Employer { get; set; }
    public required string Vacancy { get; set; }
    public string? Comment { get; set; }
    public EmploymentStatus Status { get; set; } = EmploymentStatus.InActive;
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EmploymentStatus
{
    Active = 1,
    InActive = 2
}