using DP_backend.Domain.Employment;

namespace DP_backend.Models.DTOs
{
    public class EmploymentRequestDTO
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public InternshipRequestDTO InternshipRequest { get; set; }
        public EmploymentRequestStatus Status { get; set; }

        public EmploymentRequestDTO() { }
        public EmploymentRequestDTO(EmploymentRequest model) 
        { 
            Id = model.Id;
            InternshipRequest = new InternshipRequestDTO(model.InternshipRequest);
            Status = model.Status;
            StudentId = model.StudentId;
        }
    }
}
