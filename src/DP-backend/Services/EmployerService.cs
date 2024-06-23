using DP_backend.Common.Exceptions;
using DP_backend.Database;
using DP_backend.Domain.Employment;
using DP_backend.Helpers;
using DP_backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DP_backend.Services
{
    public interface IEmployerService
    {
        public Task AddEmployer(EmployerPostDTO model);
        public Task UpdateEmployer(Guid employerId, EmployerPostDTO model);
        public Task DeleteEmployer(Guid employerId);
        public Task<EmployerDTO> GetEmployerById(Guid employerId);
        public Task<List<EmployerDTO>> GetAllEmployers(bool? asPartner);
    }
    public class EmployerService : IEmployerService
    {
        private readonly ApplicationDbContext _context;
        public EmployerService(ApplicationDbContext context) 
        { 
            _context = context;
        }
        public async Task AddEmployer(EmployerPostDTO model)
        {
            var employer = new Employer
            {
                Id = Guid.NewGuid(),
                CompanyName = model.CompanyName,
                Contact = model.Contact,
                Comment = model.Comment,
                isPartner = model.isPartner,
                CommunicationPlace = model.CommunicationPlace,
                PlacesQuantity = model.PlacesQuantity,
                AuthorizedDelegate = model.AuthorizedDelegate,
                Vacancy = model.Vacancy
        };
            await _context.AddAsync(employer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployer(Guid employerId)
        {
            var employer = await _context.Employers.FirstOrDefaultAsync(x => x.Id == employerId);
            if (employer == null)
            {
                throw new NotFoundException($"There is no employer with this {employerId} id!");
            }
            _context.Employers.Remove(employer);
            await _context.SaveChangesAsync();
        }

        public async Task<List<EmployerDTO>> GetAllEmployers(bool? asPartner)
        {
            IQueryable<Employer> query = _context.Employers;
            if (asPartner==true)
            {
                query = query.Where(x => x.isPartner);
            }
            else if (asPartner==false)
            {
                query = query.Where(x => !x.isPartner);
            }
            return await query.Select(x=>new EmployerDTO(x)).ToListAsync();
            
        }

        public async Task<EmployerDTO> GetEmployerById(Guid employerId)
        {
            var employer = await _context.Employers.FirstOrDefaultAsync(x => x.Id == employerId);
            if (employer == null)
            {
                throw new NotFoundException($"There is no employer with this {employerId} id!");
            }
            return new EmployerDTO(employer);
        }

        public async Task UpdateEmployer(Guid employerId, EmployerPostDTO model)
        {
            var employer = await _context.Employers.FirstOrDefaultAsync(x=>x.Id== employerId);
            if (employer == null)
            {
                throw new NotFoundException($"There is no employer with this {employerId} id!");
            }
            employer.CompanyName = model.CompanyName;
            employer.Comment = model.Comment;
            employer.isPartner = model.isPartner;
            employer.CommunicationPlace = model.CommunicationPlace;
            employer.Contact= model.Contact;
            employer.PlacesQuantity = model.PlacesQuantity;
            employer.AuthorizedDelegate = model.AuthorizedDelegate;
            employer.Vacancy = model.Vacancy;
            await _context.SaveChangesAsync();
        }
    }
}
