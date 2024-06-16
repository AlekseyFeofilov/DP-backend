using DP_backend.Domain.Templating;
using DP_backend.Domain.Templating.Employment;
using Microsoft.Extensions.DependencyInjection;

namespace DP_backend.Templating.DI;

public static class DependenciesRegistration
{
    public static IServiceCollection AddTemplating(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IDocumentTemplatesService, DocumentTemplatesService>();

        serviceCollection
            .AddKeyedScoped<ITemplateFieldsResolver<InternshipDiaryTemplateResolutionContext>, EmploymentFieldsResolver>(nameof(InternshipDiaryTemplate))
            .AddKeyedScoped<ITemplateFieldsResolver<InternshipDiaryTemplateResolutionContext>, StudentFieldsResolver>(nameof(InternshipDiaryTemplate))
            .AddKeyedScoped<ITemplateFieldsResolver<InternshipDiaryTemplateResolutionContext>, InternshipDiaryTasksFieldsResolver>(nameof(InternshipDiaryTemplate))
            .AddKeyedScoped<ITemplateFieldsResolver<InternshipDiaryTemplateResolutionContext>, InternshipDiaryAssessmentFieldsResolver>(nameof(InternshipDiaryTemplate));
        
        serviceCollection.AddScoped<GenerateDocxInternshipDiaryUseCase>();

        return serviceCollection;
    }
}