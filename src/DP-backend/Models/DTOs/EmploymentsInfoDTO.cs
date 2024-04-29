namespace DP_backend.Models.DTOs
{
    public class EmploymentsInfoDTO
    {
        public EmploymentDTO? Employment { get; set; }
        public EmploymentRequestDTO? EmploymentRequest { get; set; }
        public InternshipRequestDTO? InternshipRequest { get; set; }
    }
}
