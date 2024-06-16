using DP_backend.Domain.Templating;
using DP_backend.Domain.Templating.Employment;

namespace DP_backend.Templating;

public class InternshipDiaryTasksFieldsResolver : ITemplateFieldsResolver<InternshipDiaryTemplateResolutionContext>
{
    private static readonly string[] ResolvingFields =
    [
        InternshipDiaryTemplate.Keys.TasksDoneReportTable,
        InternshipDiaryTemplate.Keys.TaskBeginDate,
        InternshipDiaryTemplate.Keys.TaskEndDate,
        InternshipDiaryTemplate.Keys.TaskName,
        InternshipDiaryTemplate.Keys.TaskTimeSpentHours,
    ];

    public InternshipDiaryTasksFieldsResolver()
    {
    }

    public IEnumerable<string> CanResolveFields() => ResolvingFields;

    public Task ResolveFields(string[] fieldToResolve, InternshipDiaryTemplateResolutionContext context, CancellationToken ct)
    {
        var tasks = context.InternshipDiaryTasks.Select(x => new TemplateContext.Entry(
                TemplateContext.EntryType.KeyValueCollection,
                KeyValueCollection: new Dictionary<string, TemplateContext.Entry>
                {
                    //@formatter:off
                    [InternshipDiaryTemplate.Keys.TaskBeginDate]      = InternshipDiaryTemplate.Formatting.Date(x.BeginDate),       
                    [InternshipDiaryTemplate.Keys.TaskEndDate]        = InternshipDiaryTemplate.Formatting.Date(x.EndDate),     
                    [InternshipDiaryTemplate.Keys.TaskName]           = x.Name,  
                    [InternshipDiaryTemplate.Keys.TaskTimeSpentHours] = InternshipDiaryTemplate.Formatting.Hours(x.TimeSpentHours),
                    //@formatter:on
                }))
            .ToArray();

        context.SetField(InternshipDiaryTemplate.Keys.TasksDoneReportTable, new TemplateContext.Entry(TemplateContext.EntryType.Collection, Collection: tasks));

        return Task.CompletedTask;
    }
}