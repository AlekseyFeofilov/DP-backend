using DP_backend.Domain.Employment;

namespace DP_backend.Models.DTOs
{
    public class StudentWithGradesDTO : StudentShortDTO
    {
        public float? InternshipDiaryMark {  get; set; }
        public float? CourseWorkMark { get; set; }

        public StudentWithGradesDTO() { }

        public StudentWithGradesDTO(Student model, float? internshipDiaryMark, float? courseWorkMark) : base(model)
        {
            InternshipDiaryMark = internshipDiaryMark;
            CourseWorkMark = courseWorkMark;
        }
    }
}
