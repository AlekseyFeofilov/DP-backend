using DP_backend.Domain.Employment;

namespace DP_backend.Models.DTOs;

public record EmployerVariantDTO(Guid? EmployerId, string? CustomCompanyName);

/// <param name="Status"></param>
/// <param name="Occupation">Должность \ занятость \ стек</param>
/// <param name="Priority">Приоритет, чем меньше тем приоритетнее (семантика - "Первый приоритет")</param>
public record EmploymentVariantDTO(EmploymentVariantStatus Status, int Priority, EmployerVariantDTO Employer, string Occupation, Guid StudentId);

/// <param name="Status"></param>
/// <param name="Occupation">Должность \ занятость \ стек</param>
/// <param name="Priority">Приоритет, чем меньше тем приоритетнее (семантика - "Первый приоритет")</param>
public record EmploymentVariantUpdateDTO(EmploymentVariantStatus Status, string Occupation, int Priority);

/// <param name="EmployerVariant"></param>
/// <param name="Occupation">Должность \ занятость \ стек </param>
/// <param name="Priority">Приоритет, чем меньше тем приоритетнее (семантика - "Первый приоритет")</param>
public record EmploymentVariantCreateDTO(EmployerVariantDTO EmployerVariant, string Occupation, EmploymentVariantStatus Status, int Priority = 0);