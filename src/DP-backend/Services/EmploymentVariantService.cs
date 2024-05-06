using System.Security.Claims;
using DP_backend.Common.Exceptions;
using DP_backend.Database;
using DP_backend.Domain.Employment;
using DP_backend.Helpers;
using DP_backend.Models.DTOs;
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

public class EmploymentVariantService(ApplicationDbContext context, IEmploymentService employmentService) : IEmploymentVariantService
{
    public async Task<EmploymentVariant> Get(Guid employmentVariantId, CancellationToken ct)
    {
        var employmentVariant = await context.EmploymentVariants.GetUndeleted().FirstOrDefaultAsync(x => x.Id == employmentVariantId, ct);
        if (employmentVariant is null) throw new NotFoundException($"Вариант трудоустройства {{{employmentVariantId}}} не найден");
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
                           ?? throw new NotFoundException($"Компания-работодатель {{{employerId}}} не найден");
            employerVariant = new EmployerVariant(employer);
        }

        employerVariant ??= new EmployerVariant(dto.EmployerVariant.CustomCompanyName!);
        var internshipRequest = await employmentService.CreateInternshipRequest(request.CallingUser.GetUserId(), new InternshipRequestСreationDTO
        {
            EmployerId = (Guid)dto.EmployerVariant.EmployerId,
            Vacancy = dto.Occupation,
            Comment = ""
        });
        var employmentVariant = new EmploymentVariant()
        {
            Occupation = dto.Occupation,
            Priority = dto.Priority,
            Status = dto.Status,
            InternshipRequestId=internshipRequest.Id,
            StudentId = request.CallingUser.GetUserId()
        };
        context.EmploymentVariants.Add(employmentVariant);
        try
        {
            await context.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {

        }
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