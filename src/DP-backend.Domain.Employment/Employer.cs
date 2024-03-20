using DP_backend.Common;

namespace DP_backend.Domain.Employment;

/// <summary>
/// Компания-работодатель
/// </summary>
public class Employer : BaseEntity
{
    public required string CompanyName { get; set; }
}