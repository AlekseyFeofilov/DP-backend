using DP_backend.Domain.Employment;

namespace DP_backend.Models.DTOs
{
    public class EmployerDTO : EmployerPostDTO
    {
        public Guid? Id { get; set; }

        public EmployerDTO(Employer employer) : base(employer)
        {
            Id = employer.Id;            
        }
    }
}
