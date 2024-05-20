using DP_backend.Domain.Employment;

namespace DP_backend.Models.DTOs
{
    public class InternshipDiaryRequestDTO
    {
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }

        public List<Guid> FileIds { get; set; }

        public int Semester { get; set; }

        public InternshipDiaryRequestDTO() { }

        public InternshipDiaryRequestDTO(InternshipDiaryRequest internshipDiaryRequest, List<Guid> fileIds) 
        {
            Id = internshipDiaryRequest.Id;
            StudentId = internshipDiaryRequest.StudentId;
            FileIds = fileIds;
            Semester = internshipDiaryRequest.Semester;
        }
    }
}
