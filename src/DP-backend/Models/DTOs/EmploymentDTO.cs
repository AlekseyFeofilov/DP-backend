using DP_backend.Domain.Employment;

namespace DP_backend.Models.DTOs
{
    public class EmploymentDTO
    {
        Guid Id { get; set; }
        public EmploymentStatus Status { get; set; } 
        [Required]
        public required EmployerDTO Employer {  get; set; }
        [Required]
        public required string Vacancy { get; set; }
        public string? Comment { get; set; }
        public EmploymentStatus EmploymentStatus { get; set; }
        public EmploymentDTO(Employment model) 
        { 
            Id = model.Id;
        }
            Status = model.Status;
    }
}