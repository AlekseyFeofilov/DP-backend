using DP_backend.Domain.Employment;
using System.ComponentModel.DataAnnotations;

namespace DP_backend.Models.DTOs
{
    public class EmploymentDTO
    {

        public Guid Id { get; set; }
        public EmploymentStatus Status { get; set; } 
        Guid StudentId { get; set; }
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
            Status = model.Status;
            Employer = new EmployerDTO(model.Employer);
            Comment= model.Comment;
            Vacancy = model.Vacancy;
            StudentId = model.StudentId;
        }
    }
}