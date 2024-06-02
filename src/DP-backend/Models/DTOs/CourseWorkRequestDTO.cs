using DP_backend.Domain.Employment;

namespace DP_backend.Models.DTOs
{
    public class CourseWorkRequestDTO
    {
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }

        public List<Guid> FileIds { get; set; }

        public CourseWorkRequestStatus Status { get; set; }

        public int Semester { get; set; }

        public CourseWorkRequestDTO() { }

        public CourseWorkRequestDTO(CourseWorkRequest courseWorkRequest, List<Guid> fileIds)
        {
            Id = courseWorkRequest.Id;
            StudentId = courseWorkRequest.StudentId;
            FileIds = fileIds;
            Status = courseWorkRequest.Status;
            Semester = courseWorkRequest.Semester;
        }
    }
}
