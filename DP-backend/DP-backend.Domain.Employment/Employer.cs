namespace DP_backend.Domain.Employment;

/// <summary>
/// Компания-работодатель
/// </summary>
public class Employer
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string CompanyName { get; set; }
}