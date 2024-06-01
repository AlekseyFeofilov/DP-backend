using DP_backend.Domain.Employment;
using DP_backend.Models.DTOs;

namespace DP_backend.Models
{
    public class EmploymentWithDatesShortDTO
    {
        public Guid Id { get; set; }
        public EmployerDTO Employer { get; set; }
        public string Vacancy { get; set; }
        public string? Comment { get; set; }
        public DateTime StartDate { get; set; }
        public EmploymentStatus Status { get; set; }
        public DateTime EndDate { get; set; }

        public EmploymentWithDatesShortDTO() { }

        public EmploymentWithDatesShortDTO(Employment model, DateTime startDate, DateTime endDate)
        {
            Id = model.Id;
            Employer = new EmployerDTO(model.Employer);
            Vacancy = model.Vacancy;
            Comment = model.Comment;
            Status = model.Status;
            StartDate = startDate>model.CreateDateTime? startDate : model.CreateDateTime;
            EndDate = (endDate<model.EndDate|| model.EndDate==null)? endDate : (DateTime)model.EndDate;
        }

    }
}
