using DP_backend.Domain.Employment;
using DP_backend.Domain.FileStorage;
using System.ComponentModel.DataAnnotations;

namespace DP_backend.Models.DTOs
{
    public class InternshipDiaryRequestDTO
    {
        public Guid Id { get; set; }

        [Required]
        public StudentShortDTO Student { get; set; }

        public List<FileDTO> Files { get; set; }

        public InternshipDiaryRequestStatus Status { get; set; }

        /// <summary>
        /// Допустимые значения: 5,6,7,8
        /// </summary>
        public int Semester { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime ModifyDateTime { get; set; }

        public float? Mark { get; set; }
        
        /// <summary>
        /// Руководитель практики от профильной организации; если <c>null</c> в генерации документа используется уполномоченный представитель из Трудоустройства
        /// </summary>
        public string? ManagerFromEmployment { get; set; }

        /// <summary>
        /// Индивидуальный задание на практике; если <c>null</c> в генерации документа используется имя компании 
        /// </summary>
        public string? StudentIndividualTask { get; set; }
        

        public InternshipDiaryRequestDTO() { }

        public InternshipDiaryRequestDTO(InternshipDiaryRequest internshipDiaryRequest, IEnumerable<FileEntityLink> files) 
        {
            Id = internshipDiaryRequest.Id;
            Student = new StudentShortDTO(internshipDiaryRequest.Student);
            Files = files.Select(f => new FileDTO { FileId = f.FileId, FileName = f.File.Name, ContentType = f.File.ContentType, Size = f.File.Size, CreatedAt = f.File.CreatedAt }).ToList();
            Status = internshipDiaryRequest.Status;
            Semester = internshipDiaryRequest.Semester;
            CreateDateTime = internshipDiaryRequest.CreateDateTime;
            ModifyDateTime = internshipDiaryRequest.ModifyDateTime;
            ManagerFromEmployment = internshipDiaryRequest.ManagerFromEmployment;
            StudentIndividualTask = internshipDiaryRequest.StudentIndividualTask;
            Mark = internshipDiaryRequest.Mark;
        }
    }
}
