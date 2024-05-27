using DP_backend.Domain.Employment;
using System.ComponentModel.DataAnnotations;

namespace DP_backend.Models.DTOs
{
    public class InternshipRequestDTO
    {
        public Guid Id { get; set; }

        [Required]
        public StudentShortDTO Student { get; set; }

        [Required]
        public EmployerDTO Employer { get; set; }

        [Required]
        public string Vacancy { get; set; }

        public DateTime CreateDateTime { get; set; }
        public string? Comment { get; set; }

        public InternshipStatus InternshipRequestStatus { get; set; }

        public InternshipRequestDTO() { }

        public InternshipRequestDTO(InternshipRequest model)
        {
            Id = model.Id;
            Student = new StudentShortDTO(model.Student);
            Employer = new EmployerDTO(model.Employer);
            Vacancy = model.Vacancy;
            Comment = model.Comment;
            InternshipRequestStatus = model.Status;
            CreateDateTime = model.CreateDateTime;
        }
    }
}
