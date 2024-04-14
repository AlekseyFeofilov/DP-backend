using DP_backend.Domain.Employment;
using System.ComponentModel.DataAnnotations;

namespace DP_backend.Models.DTOs
{
    public class EmploymentDTO
    {
        public Guid Id { get; set; }

        [Required]
        public Guid StudentId { get; set; }

        [Required]
        public EmployerDTO Employer {  get; set; }

        [Required]
        public string Vacancy { get; set; }

        public string? Comment { get; set; }

        public EmploymentStatus EmploymentStatus { get; set; }

        public EmploymentDTO() { }

        public EmploymentDTO(Employment model)
        {
            Id = model.Id;
            StudentId = model.StudentId;
            Employer = new EmployerDTO(model.Employer);
            Vacancy = model.Vacancy;
            Comment = model.Comment;
            EmploymentStatus = model.Status;
        }
    }
}