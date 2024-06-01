namespace DP_backend.Models.DTOs
{
    public class EmploymentReportDTO
    {
        public StudentShortDTO Student {  get; set; }
        public List<EmploymentWithDatesShortDTO> Employments { get; set; }
    }
}
