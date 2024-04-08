using DP_backend.Helpers;
using DP_backend.Models;
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
    [Authorize]
    [ProducesResponseType<ErrorDto>(404)]
    public async Task<EmploymentVariantDTO> Create(EmploymentVariantCreateDTO dto, CancellationToken cancellationToken)
    {
        var employmentVariant = await employmentVariantService.Create(new(User, dto), cancellationToken);
        return employmentVariant.Adapt<EmploymentVariantDTO>();
    }

    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType<ErrorDto>(404)]
    [ProducesResponseType<ErrorDto>(403)]
    public async Task<EmploymentVariantDTO> Update(Guid id, EmploymentVariantUpdateDTO data, CancellationToken cancellationToken)
    {
        var employmentVariant = await employmentVariantService.Update(new(User, id, data), cancellationToken);
        return employmentVariant.Adapt<EmploymentVariantDTO>();
    }

    [HttpPut("{id}/r")]
    [Authorize]
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
}