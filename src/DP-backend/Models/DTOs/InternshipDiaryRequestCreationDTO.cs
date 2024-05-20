namespace DP_backend.Models.DTOs
{
    public class InternshipDiaryRequestCreationDTO
    {
        public Guid StudentId { get; set; }

        public int Semester { get; set; }
    }
}
