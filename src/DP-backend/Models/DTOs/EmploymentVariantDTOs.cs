using DP_backend.Domain.Employment;

namespace DP_backend.Models.DTOs;

/// <param name="Status"></param>
/// <param name="Occupation">Должность \ занятость \ стек</param>
/// <param name="Priority">Приоритет, чем меньше тем приоритетнее (семантика - "Первый приоритет")</param>
public record EmploymentVariantDTO(Guid Id, EmploymentVariantStatus Status, int Priority, string Occupation, Guid StudentId, InternshipRequestDTO? InternshipRequestDTO);

/// <param name="Status"></param>
/// <param name="Occupation">Должность \ занятость \ стек</param>
/// <param name="Priority">Приоритет, чем меньше тем приоритетнее (семантика - "Первый приоритет")</param>
public record EmploymentVariantUpdateDTO(EmploymentVariantStatus Status, string Occupation, int Priority, string Comment);

/// <param name="EmployerVariant"></param>
/// <param name="Occupation">Должность \ занятость \ стек </param>
/// <param name="Priority">Приоритет, чем меньше тем приоритетнее (семантика - "Первый приоритет")</param>
public record EmploymentVariantCreateDTO(Guid EmployerId, string Occupation, EmploymentVariantStatus Status, string Comment, int Priority = 0);