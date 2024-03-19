namespace DP_backend.Domain.Employment;

public record EmployerVariant
{
    public string? CustomCompanyName { get; }
    public Employer? Employer { get; }

    public EmployerVariant(string customCompanyName) => CustomCompanyName = customCompanyName;

    public EmployerVariant(Employer employer) => Employer = employer;

    // for EF binding
    protected EmployerVariant() { }

    public string GetCompanyName() => Employer?.CompanyName ?? CustomCompanyName!;
}