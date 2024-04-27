using DP_backend.Domain.Employment;
using Mapster;

namespace DP_backend.Models.DTOs
{
    public class StudentDTO
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public StudentStatus Status { get; set; } = StudentStatus.None;
        public GroupDTO? Group { get; set; }
        public List<EmploymentVariantDTO> EmploymentVariants { get; set; }
        public List<EmploymentDTO> Employments { get; set; }
        public StudentDTO() { }
        public StudentDTO(Student model) 
        { 
            UserId = model.UserId;
            Name = model.Name;
            Status = model.Status;
            Group = model.Group==null? null : new GroupDTO(model.Group);
            Employments = model.Employments.Select(x=> new EmploymentDTO(x)).ToList();
            EmploymentVariants = model.EmploymentVariants.Select(x=>x.Adapt<EmploymentVariantDTO>()).ToList();
        }
    }
}
