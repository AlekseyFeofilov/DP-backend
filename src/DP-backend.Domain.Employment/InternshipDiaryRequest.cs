﻿using DP_backend.Common;
using System.Text.Json.Serialization;

namespace DP_backend.Domain.Employment;
 
/// <summary>
/// Заявка для дневника практики
/// </summary>
public class InternshipDiaryRequest : BaseEntity
{
    public Guid StudentId { get; set; }
    public Student Student { get; set; }
    public InternshipDiaryRequestStatus Status { get; set; }
    public int Semester {  get; set; }
    public float? Mark { get; set; }

    /// <summary>
    /// Руководитель практики от профильной организации; если <c>null</c> используется уполномоченный представитель из Трудоустройства
    /// </summary>
    public string? ManagerFromEmployment { get; set; }

    /// <summary>
    /// Индивидуальный задание на практике; если <c>null</c> используется имя компании 
    /// </summary>
    public string? StudentIndividualTask { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum InternshipDiaryRequestStatus
{
    No = 1,
    OnVerification = 2,
    OnRevision = 3,
    Approved = 4,
    SubmittedForSigning = 5,
    Rated = 6
}