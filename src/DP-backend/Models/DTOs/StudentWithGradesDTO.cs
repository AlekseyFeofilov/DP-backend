using DP_backend.Domain.Employment;

namespace DP_backend.Models.DTOs
{
    public class StudentWithGradesDTO : StudentShortDTO
    {
        public int? InternshipDiaryGrade {  get; set; }
        public int? CourseWorkGrade { get; set; }

        public StudentWithGradesDTO() { }

        public StudentWithGradesDTO(Student model, int? internshipDiaryGrade, int? courseWorkGrade) : base(model)
        {
            InternshipDiaryGrade = internshipDiaryGrade;
            CourseWorkGrade = courseWorkGrade;
        }
    }
}
