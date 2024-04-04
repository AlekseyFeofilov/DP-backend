using DP_backend.Common;
using System.Text.Json.Serialization;

namespace DP_backend.Domain.Employment;

public class Employment : BaseEntity
{
    public required Guid StudentId { get; init; }
    public required Employer Employer { get; set; }
    public required string Vacancy { get; set; }
    public string? Comment { get; set; }
    public EmploymentStatus Status { get; set; } = EmploymentStatus.NonVerified;
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EmploymentStatus
{
    NonVerified = 1,
    Verified = 2
}