using DP_backend.Domain.Employment;
using DP_backend.Helpers;
using DP_backend.Models.DTOs;
using DP_backend.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DP_backend.Services
{
    public interface IEmployerService
    {
        public Task AddEmployer(EmployerPostDTO model);
        public Task UpdateEmployer(Guid employerId, EmployerPostDTO model);
        public Task DeleteEmployer(Guid employerId);
        public Task<EmployerDTO> GetEmployerById(Guid employerId);
        public Task<List<EmployerDTO>> GetAllEmployers();
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
                CommunicationPlace = model.CommunicationPlace,
                PlacesQuantity = model.PlacesQuantity
            };
            await _context.AddAsync(employer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployer(Guid employerId)
        {
            var employer = await _context.Employers.GetUndeleted().FirstOrDefaultAsync(x => x.Id == employerId);
            if (employer == null)
            {
                throw new NotFoundException($"There is no employer with this {employerId} id!");
            }
            _context.Employers.Remove(employer);
            await _context.SaveChangesAsync();
        }

        public async Task<List<EmployerDTO>> GetAllEmployers()
        {
            var employers = await _context.Employers.GetUndeleted().Select(x=>new EmployerDTO(x)).ToListAsync();
            return employers;
        }

        public async Task<EmployerDTO> GetEmployerById(Guid employerId)
        {
            var employer = await _context.Employers.GetUndeleted().FirstOrDefaultAsync(x => x.Id == employerId);
            if (employer == null)
            {
                throw new NotFoundException($"There is no employer with this {employerId} id!");
            }
            return new EmployerDTO(employer);
        }

        public async Task UpdateEmployer(Guid employerId, EmployerPostDTO model)
        {
            var employer = await _context.Employers.GetUndeleted().FirstOrDefaultAsync(x=>x.Id== employerId);
            if (employer == null)
            {
                throw new NotFoundException($"There is no employer with this {employerId} id!");
            }
            employer.CompanyName = model.CompanyName;
            employer.Comment = model.Comment;
            employer.CommunicationPlace = model.CommunicationPlace;
            employer.Contact= model.Contact;
            employer.PlacesQuantity = model.PlacesQuantity;
            await _context.SaveChangesAsync();
        }
    }
}
