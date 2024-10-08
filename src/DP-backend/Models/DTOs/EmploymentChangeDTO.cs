﻿using DP_backend.Domain.Employment;
using System.ComponentModel.DataAnnotations;

namespace DP_backend.Models.DTOs
{
    public class EmploymentChangeDTO
    {
        [Required]
        public required string Vacancy { get; set; }

        public string? Comment { get; set; }

        public EmploymentStatus EmploymentStatus { get; set; }
    }
}
