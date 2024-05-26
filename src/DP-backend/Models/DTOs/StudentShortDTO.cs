using DP_backend.Domain.Employment;

namespace DP_backend.Models.DTOs
{
    public class StudentShortDTO
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public StudentStatus Status { get; set; } = StudentStatus.None;
        public GroupDTO? Group { get; set; }

        public StudentShortDTO() { }
        public StudentShortDTO(Student model)
        {
            UserId = model.Id;
            Name = model.Name;
            Status = model.Status;
            Group = model.Group == null ? null : new GroupDTO(model.Group);
        }
    }
}
