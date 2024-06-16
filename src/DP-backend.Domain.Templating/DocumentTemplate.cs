using DP_backend.Common;

namespace DP_backend.Domain.Templating;

public class DocumentTemplate : BaseEntity
{
    /// <summary>
    /// Тип шаблона, дескриминатор для определения для чего предназначен  
    /// </summary>
    public required string TemplateType { get; init; }

    public required Guid TemplateFileId { get; init; }

    /// <summary>
    /// Предопределённые значения
    /// </summary>
    public TemplateContext? BaseTemplateContext { get; set; }

    public string[] FieldIds { get; set; } = Array.Empty<string>();
}