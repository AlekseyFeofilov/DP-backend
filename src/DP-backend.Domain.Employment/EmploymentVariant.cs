using DP_backend.Common;

namespace DP_backend.Domain.Employment;

public class EmploymentVariant : BaseEntity
{
    public EmploymentVariantStatus Status { get; set; }

    /// <summary>
    /// Приоритет, чем меньше тем приоритетнее (семантика - "Первый приоритет") 
    /// </summary>
    public int Priority { get; set; } = 0;

    public required EmployerVariant Employer { get; set; }

    /// <summary>
    /// Должность \ занятость \ стек 
    /// </summary>
    public string Occupation { get; set; }
    public Guid StudentId { get; set; }
    public Student Student { get; set; }
}

public enum EmploymentVariantStatus
{
    // вероятно мы могли бы учитывать планируемые варианты, по поводу которых ещё не было проведено никаких переговоров 
    // Challenge = 1,
    Interviewed = 2,
    Offered = 3,
}