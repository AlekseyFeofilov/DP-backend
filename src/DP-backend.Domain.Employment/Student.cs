using System;
using DP_backend.Common;

namespace DP_backend.Domain.Employment;

public class Student : BaseEntity
{
    public required Guid UserId { get => Id; init => Id = value; }
    public StudentStatus Status { get; set; } = StudentStatus.None;
    public Guid? GroupId { get; set; }
    public Group? Group { get; set; }
    public List<EmploymentVariant> EmploymentVariants { get; set; }

    public Employment? Employment { get; set; }
}

public enum StudentStatus
{
    None = 0,
    CompaniesChosen = 1,
    PassedTheInterview = 2,
    GetAnOffer = 3,
    OfferChosen = 4,
    Employed = 5
}