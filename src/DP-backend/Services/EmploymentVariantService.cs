using System.Security.Claims;
using DP_backend.Domain.Employment;
using DP_backend.Helpers;
using DP_backend.Models.DTOs;
using DP_backend.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DP_backend.Services;

public record CreateEmploymentVariantRequest(ClaimsPrincipal CallingUser, EmploymentVariantCreateDTO Data);

public record UpdateEmploymentVariantRequest(ClaimsPrincipal CallingUser, Guid EmploymentVariantId, EmploymentVariantUpdateDTO Data);

public record RemoveEmploymentVariantRequest(ClaimsPrincipal CallingUser, Guid EmploymentVariantId);

public interface IEmploymentVariantService
{
    Task<EmploymentVariant> Get(Guid employmentVariantId, CancellationToken ct);
    Task<EmploymentVariant> Create(CreateEmploymentVariantRequest request, CancellationToken ct);
    Task<EmploymentVariant> Update(UpdateEmploymentVariantRequest request, CancellationToken ct);
    Task<EmploymentVariant> Remove(RemoveEmploymentVariantRequest request, CancellationToken ct);
}

public class EmploymentVariantService(ApplicationDbContext context) : IEmploymentVariantService
{
    public async Task<EmploymentVariant> Get(Guid employmentVariantId, CancellationToken ct)
    {
        var employmentVariant = await context.EmploymentVariants.GetUndeleted().FirstOrDefaultAsync(x => x.Id == employmentVariantId, ct);
        if (employmentVariant is null) throw new KeyNotFoundException($"Вариант трудоустройства {{{employmentVariantId}}} не найден");
        return employmentVariant;
    }

    public async Task<EmploymentVariant> Create(CreateEmploymentVariantRequest request, CancellationToken ct)
    {
        var dto = request.Data;
        if (!dto.EmployerVariant.EmployerId.HasValue
            && dto.EmployerVariant.CustomCompanyName == null) throw new BadDataException("Не корректное значение варианта работадателя");

        EmployerVariant? employerVariant = null;
        if (dto.EmployerVariant.EmployerId.HasValue)
        {
            var employerId = dto.EmployerVariant.EmployerId.Value;
            var employer = await context.Employers.GetUndeleted().FirstOrDefaultAsync(x => x.Id == employerId, ct)
                           ?? throw new KeyNotFoundException($"Компания-работодатель {{{employerId}}} не найден");
            employerVariant = new EmployerVariant(employer);
        }

        employerVariant ??= new EmployerVariant(dto.EmployerVariant.CustomCompanyName!);

        var employmentVariant = new EmploymentVariant()
        {
            Employer = employerVariant,
            Occupation = dto.Occupation,
            Priority = dto.Priority,
            Status = dto.Status,
            Student = new Student { UserId = request.CallingUser.GetUserId() }
        };
        context.EmploymentVariants.Add(employmentVariant);

        await context.SaveChangesAsync(ct);
        return employmentVariant;
    }


    public async Task<EmploymentVariant> Update(UpdateEmploymentVariantRequest request, CancellationToken ct)
    {
        var employmentVariant = await Get(request.EmploymentVariantId, ct);
        if (request.CallingUser.GetUserId() != employmentVariant.Student.Id) throw new NoPermissionException();

        employmentVariant.Status = request.Data.Status;
        employmentVariant.Occupation = request.Data.Occupation;
        employmentVariant.Priority = request.Data.Priority;

        await context.SaveChangesAsync(ct);
        return employmentVariant;
    }

    public async Task<EmploymentVariant> Remove(RemoveEmploymentVariantRequest request, CancellationToken ct)
    {
        var employmentVariant = await Get(request.EmploymentVariantId, ct);
        if (request.CallingUser.GetUserId() != employmentVariant.Student.Id) throw new NoPermissionException();
        context.Remove(employmentVariant);
        await context.SaveChangesAsync(ct);
        return employmentVariant;
    }
}