using DP_backend.Domain.Employment;
using DP_backend.Helpers;
using DP_backend.Models.DTOs;
using DP_backend.Services;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DP_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmploymentVariantController(IEmploymentVariantService employmentVariantService, ApplicationDbContext context) : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType<ErrorDto>(404)]
    public async Task<EmploymentVariantDTO> Get(Guid id, CancellationToken cancellationToken)
    {
        var employmentVariant = await employmentVariantService.Get(id, cancellationToken);
        return employmentVariant.Adapt<EmploymentVariantDTO>();
    }

    [HttpPost]
    [Authorize] // todo check permission for example employment_variant.my.manage 
    [ProducesResponseType<ErrorDto>(404)]
    [ProducesResponseType<ErrorDto>(403)]
    public async Task<EmploymentVariantDTO> Create(EmploymentVariantCreateDTO dto, CancellationToken cancellationToken)
    {
        var employmentVariant = await employmentVariantService.Create(new(User, dto), cancellationToken);
        return employmentVariant.Adapt<EmploymentVariantDTO>();
    }

    [HttpPut("{id}")]
    [Authorize] // todo check permission for example employment_variant.my.manage
    [ProducesResponseType<ErrorDto>(404)]
    [ProducesResponseType<ErrorDto>(403)]
    public async Task<EmploymentVariantDTO> Update(Guid id, EmploymentVariantUpdateDTO data, CancellationToken cancellationToken)
    {
        var employmentVariant = await employmentVariantService.Update(new(User, id, data), cancellationToken);
        return employmentVariant.Adapt<EmploymentVariantDTO>();
    }

    [HttpDelete("{id}")]
    [Authorize] // todo check permission for example employment_variant.my.manage
    [ProducesResponseType<ErrorDto>(404)]
    [ProducesResponseType<ErrorDto>(403)]
    public async Task<EmploymentVariantDTO> Remove(Guid id, CancellationToken cancellationToken)
    {
        var employmentVariant = await employmentVariantService.Remove(new(User, id), cancellationToken);
        return employmentVariant.Adapt<EmploymentVariantDTO>();
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<List<EmploymentVariantDTO>> GetCallingUserEmploymentVariants(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var employmentVariants = await context.EmploymentVariants.Where(x => x.Student.Id == userId).ToListAsync(cancellationToken);
        return employmentVariants.Select(x => x.Adapt<EmploymentVariantDTO>()).ToList();
    }

    [HttpGet]
    [Authorize] // todo check permission for example employment_variant.read
    public async Task<List<EmploymentVariantDTO>> SearchEmploymentVariants(
        [FromQuery] Guid? studentId,
        [FromQuery] Guid? employerId,
        CancellationToken cancellationToken)
    {
        var employmentVariants = await context.EmploymentVariants
            .If(studentId.HasValue,
                variants => variants.Where(x => x.Student.Id == studentId))
            .If(employerId.HasValue,
                variants => variants.Where(x => x.InternshipRequest.Employer != null && x.InternshipRequest.Employer.Id == employerId))
            .ToListAsync(cancellationToken);
        return employmentVariants.Select(x => x.Adapt<EmploymentVariantDTO>()).ToList();
    }

    [HttpGet("status/list")] // todo cache
    public IEnumerable<DictionaryEntry> GetStatuses() => DictionaryService.DescribeEnum<EmploymentVariantStatus>();
}