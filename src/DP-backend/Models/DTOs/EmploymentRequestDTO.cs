using DP_backend.Domain.Employment;

namespace DP_backend.Models.DTOs
{
    public class EmploymentRequestDTO
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid InternshipRequestId { get; set; }
        public EmploymentRequestStatus Status { get; set; }

        public EmploymentRequestDTO() { }
        public EmploymentRequestDTO(EmploymentRequest model) 
        { 
            Id = model.Id;
            InternshipRequestId = model.InternshipRequestId;
            Status = model.Status;
            StudentId = model.StudentId;
        }
    }
}
