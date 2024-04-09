using DP_backend.Domain.Employment;
using Mapster;

namespace DP_backend.Models.DTOs
{
    public class StudentDTO
    {
        public Guid UserId { get; set; }
        public StudentStatus Status { get; set; } = StudentStatus.None;
        public GroupDTO? Group { get; set; }
        public List<EmploymentVariantDTO> EmploymentVariants { get; set; }
        public EmploymentDTO Employment { get; set; }
        public StudentDTO() { }
        public StudentDTO(Student model) 
        { 
            UserId = model.UserId;
            Status = model.Status;
            Group = new GroupDTO(model.Group);
            Employment = new EmploymentDTO(model.Employment);
            EmploymentVariants = model.EmploymentVariants.Select(x=>x.Adapt<EmploymentVariantDTO>()).ToList();
        }
    }
}
