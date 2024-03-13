using DP_backend.Models.Enumerations;

namespace DP_backend.Models
{
    public class Student : User
    {
        public StudentStatus Status { get; set; }
    }
}
