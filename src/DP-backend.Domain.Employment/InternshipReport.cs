using DP_backend.Common;
using System.Text.Json.Serialization;

namespace DP_backend.Domain.Employment;

// todo : Дополнить когда составятся требования по Дневникам практики  
/// <summary>
/// Дневник практики
/// </summary>
public class InternshipReport : BaseEntity
{
    public EmployerVariant Employer { get; set; }
    public required DateTime InternshipStartedAt { get; set; }
    public required DateTime InternshipFinishedAt { get; set; }
}
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum InternshipReportStatus
{
    Submitted = 1,
    Verified = 2,
    Signed = 3,
}