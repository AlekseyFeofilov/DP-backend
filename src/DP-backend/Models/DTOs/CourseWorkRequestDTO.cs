using DP_backend.Domain.Employment;
using DP_backend.Domain.FileStorage;
using System.ComponentModel.DataAnnotations;

namespace DP_backend.Models.DTOs
{
    public class CourseWorkRequestDTO
    {
        public Guid Id { get; set; }

        [Required]
        public StudentShortDTO Student { get; set; }

        public List<FileDTO> Files { get; set; }

        public CourseWorkRequestStatus Status { get; set; }

        /// <summary>
        /// Допустимые значения: 6,8
        /// </summary>
        public int Semester { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime ModifyDateTime { get; set; }

        public float? Mark { get; set; }

        public CourseWorkRequestDTO() { }

        public CourseWorkRequestDTO(CourseWorkRequest courseWorkRequest, IEnumerable<FileEntityLink> files)
        {
            Id = courseWorkRequest.Id;
            Student = new StudentShortDTO(courseWorkRequest.Student);
            Files = files.Select(f => new FileDTO { FileId = f.FileId, FileName = f.File.Name, ContentType = f.File.ContentType, Size = f.File.Size, CreatedAt = f.File.CreatedAt }).ToList();
            Status = courseWorkRequest.Status;
            Semester = courseWorkRequest.Semester;
            CreateDateTime = courseWorkRequest.CreateDateTime;
            ModifyDateTime = courseWorkRequest.ModifyDateTime;
            Mark = courseWorkRequest.Mark;
        }
    }
}
