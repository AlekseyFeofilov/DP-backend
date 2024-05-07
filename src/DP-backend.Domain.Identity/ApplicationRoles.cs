using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DP_backend.Domain.Identity;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApplicationRoles
{
    [Display(Name = "Администратор")]
    Administrator,
    [Display(Name = "Cтудент")]
    Student,
    [Display(Name = "Cотрудник")]
    Staff,
    [Display(Name = "Никто")]
    NoOne,

}

public class ApplicationRoleNames
{
    public const string Administrator = "Administrator";
    public const string Student = "Student";
    public const string Staff = "Staff";
    public const string NoOne = "NoOne";
    public static readonly Dictionary<ApplicationRoles, String> SystemRoleNamesDictionary = new Dictionary<ApplicationRoles, String>
    {
        [ApplicationRoles.Administrator] = ApplicationRoleNames.Administrator,
        [ApplicationRoles.Student] = ApplicationRoleNames.Student,
        [ApplicationRoles.Staff] = ApplicationRoleNames.Staff,
        [ApplicationRoles.NoOne] = ApplicationRoleNames.NoOne
    };
}
