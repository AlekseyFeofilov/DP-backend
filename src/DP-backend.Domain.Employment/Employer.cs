using DP_backend.Common;

namespace DP_backend.Domain.Employment;

/// <summary>
/// Компания-работодатель
/// </summary>
public class Employer : BaseEntity
{
    public required string CompanyName { get; set; }
    public string? PlacesQuantity { get; set; }
    public bool isPartner { get; set; }
    public string Tutor {  get; set; }
    public string Vacancy { get; set; }
    public string? CommunicationPlace { get; set; }
    public string? Contact { get; set; }
    public string? Comment { get; set; }
}