using System.Globalization;
using static DP_backend.Domain.Templating.TemplateContext;

namespace DP_backend.Domain.Templating.Employment;

public enum InternshipSemesters
{
    Firth = 5
}

public static class InternshipDiaryTemplate
{
    public static string TypeBySemester(InternshipSemesters semesterNumber) => "InternshipDiarySemester" + (int)semesterNumber;

    public static DocumentTemplate CreateFor5Semester(Guid templateFileId, IEnumerable<string> templateFieldIds, Semester5PredefinedContext semester5PredefinedContext)
        => new()
        {
            TemplateType = TypeBySemester(InternshipSemesters.Firth),
            FieldIds = templateFieldIds.ToArray(),
            TemplateFileId = templateFileId,
            BaseTemplateContext = semester5PredefinedContext.CreateContext()
        };

    public class Semester5PredefinedContext
    {
        public required string FullyQualifiedManagerFromUniversity { get; set; }
        public required string UniversityDepartmentManager { get; set; }
        public required DateTime PracticePeriodFrom { get; set; }
        public required DateTime PracticePeriodTo { get; set; }
        public required string PracticeOrderNumber { get; set; }
        public required DateTime PracticeOrderDate { get; set; }

        // todo : наверное руководителя практики от университета нужно указывать в заявке, а не глобально
        public required string ManagerFromUniversity { get; set; }

        public TemplateContext CreateContext()
            => new()
            {
                //@formatter:off
                [Keys.FullyQualifiedManagerFromUniversity] = FullyQualifiedManagerFromUniversity,
                [Keys.UniversityDepartmentManager]         = UniversityDepartmentManager,
                [Keys.PracticePeriodFrom]                  = Formatting.Date(PracticePeriodFrom),
                [Keys.PracticePeriodTo]                    = Formatting.Date(PracticePeriodTo),
                [Keys.PracticeOrderNumber]                 = PracticeOrderNumber,
                [Keys.PracticeOrderDate]                   = Formatting.Date(PracticeOrderDate),    
                [Keys.ManagerFromUniversity]               = ManagerFromUniversity,
                //@formatter:on
            };
    }

    public static class Keys
    {
        public const string StudentFullname = "StudentFullname";
        public const string EmploymentName = "EmploymentName";
        public const string PracticePeriodFrom = "PracticePeriodFrom";
        public const string PracticePeriodTo = "PracticePeriodTo";
        public const string FullyQualifiedManagerFromUniversity = "FullyQualifiedManagerFromUniversity";
        public const string PracticeOrderNumber = "PracticeOrderNumber";
        public const string PracticeOrderDate = "PracticeOrderDate";
        public const string ManagerFromEmployment = "ManagerFromEmployment";
        public const string UniversityDepartmentManager = "UniversityDepartmentManager";
        public const string EmploymentDelegate = "EmploymentDelegate";
        public const string ManagerFromUniversity = "ManagerFromUniversity";
        public const string StudentName = "Student";

        public const string TasksDoneReportTable = "TasksDoneReportTable";
        public const string TaskBeginDate = "TaskBeginDate";
        public const string TaskEndDate = "TaskEndDate";
        public const string TaskName = "TaskName";
        public const string TaskTimeSpentHours = "TaskTimeSpentHours";

        public const string AssessmentFromEmploymentText = "AssesmentFromEmploymentText";
        public const string AssessmentMarkFromEmployment = "AssessmentFromEmployment";
        public const string AssessmentFromEmploymentDate = "AssessmentFromEmploymentDate";
    }

    public static class Formatting
    {
        private static readonly CultureInfo RussianCulture = CultureInfo.GetCultureInfo("ru-RU");
        private static readonly DateTimeFormatInfo RussianDateTimeFormatting = DateTimeFormatInfo.GetInstance(RussianCulture);
        public static string Date(in DateTime dateTime) => $"«{dateTime.Day}» {RussianDateTimeFormatting.GetMonthName(dateTime.Month)}  {dateTime.Year} г.";

        public static string ShortName(string fullname)
        {
            var nameParts = fullname.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var lastName = nameParts[0];
            var initials = nameParts.Skip(1).Select(x => $"{char.ToUpper(x.First())}.");

            return string.Join(' ', Enumerable.Empty<string>().Append(lastName).Concat(initials));
        }

        public static string Hours(float hours) => $"{hours:F1} ч.";
    }
}