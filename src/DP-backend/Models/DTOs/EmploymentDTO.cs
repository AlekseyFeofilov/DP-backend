using DP_backend.Domain.Employment;

namespace DP_backend.Models.DTOs
{
    public class EmploymentDTO
    {
        Guid Id { get; set; }
        public EmploymentStatus Status { get; set; } 

        public EmploymentDTO() { }
        public EmploymentDTO(Employment model) 
        { 
            Id = model.Id;
            Status = model.Status;
        }
    }
}
