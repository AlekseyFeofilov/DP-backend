using System.ComponentModel;
using DP_backend.Common;

namespace DP_backend.Domain.Employment;

public class EmploymentVariant : BaseEntity
{
    public EmploymentVariantStatus Status { get; set; } = EmploymentVariantStatus.NoInfo;

    /// <summary>
    /// Приоритет, чем меньше тем приоритетнее (семантика - "Первый приоритет") 
    /// </summary>
    public int Priority { get; set; } = 0;

    public required EmployerVariant Employer { get; set; }

    /// <summary>
    /// Должность \ занятость \ стек 
    /// </summary>
    public string Occupation { get; set; }

    public Student Student { get; set; }
}

public enum EmploymentVariantStatus
{
    [Description("Нет - дефолтный статус")]
    NoInfo = 1,
    
    [Description("Прошел собеседование")] 
    Interviewed = 2,
    
    [Description("Получил оффер (Думаю)")] 
    OfferPending = 3,

    [Description("Получил оффер (Принял)")]
    OfferAccepted = 4,

    [Description("Получил оффер (Отказался)")]
    OfferRefused = 5
}