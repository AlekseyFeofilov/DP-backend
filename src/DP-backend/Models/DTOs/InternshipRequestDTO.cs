﻿using DP_backend.Domain.Employment;
using System.ComponentModel.DataAnnotations;

namespace DP_backend.Models.DTOs
{
    public class InternshipRequestDTO
    {
        public Guid Id { get; set; }

        [Required]
        public Guid StudentId { get; set; }

        [Required]
        public EmployerDTO Employer { get; set; }

        [Required]
        public string Vacancy { get; set; }

        public string? Comment { get; set; }

        public InternshipStatus InternshipRequestStatus { get; set; }

        public InternshipRequestDTO() { }

        public InternshipRequestDTO(InternshipRequest model)
        {
            Id = model.Id;
            StudentId = model.StudentId;
            Employer = new EmployerDTO(model.Employer);
            Vacancy = model.Vacancy;
            Comment = model.Comment;
            InternshipRequestStatus = model.Status;
        }
    }
}
