using DP_backend.Common;
using DP_backend.Common.EntityType;

namespace DP_backend.Domain.Employment;

/// <summary>
/// Статично известные идентификаторы типов сущностей <see cref="EntityType"/> 
/// </summary>
public static class EntityTypeIds
{
    public const string EmploymentRequest = "EmploymentRequest";
    public const string InternshipRequest = "InternshipRequest";
    public const string InternshipDiaryRequest = "InternshipDiaryRequest";
    public const string CourseWorkRequest = "CourseWorkRequest";
    public const string EmploymentVariant = "EmploymentVariant";
}