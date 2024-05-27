using DP_backend.Domain.Employment;

namespace DP_backend.Models.DTOs
{
    public class EmploymentRequestDTO
    {
        public Guid Id { get; set; }
        public StudentShortDTO Student { get; set; }
        public InternshipRequestDTO InternshipRequest { get; set; }
        public DateTime CreateDateTime { get; set; }
        public EmploymentRequestStatus Status { get; set; }

        public EmploymentRequestDTO() { }
        public EmploymentRequestDTO(EmploymentRequest model) 
        { 
            Id = model.Id;
            InternshipRequest = new InternshipRequestDTO(model.InternshipRequest);
            Status = model.Status;
            Student = new StudentShortDTO(model.Student);
            CreateDateTime = model.CreateDateTime;
        }
    }
}
