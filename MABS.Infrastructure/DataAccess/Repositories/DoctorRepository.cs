using MABS.Domain.Models.DoctorModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MABS.Infrastructure.Data;
using MABS.Application.DataAccess.Repositories;

namespace MABS.Infrastructure.DataAccess.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ILogger<DoctorRepository> _logger;
        private readonly DataContext _context;

        public DoctorRepository(ILogger<DoctorRepository> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void Create(Doctor doctor)
        {
            _logger.LogInformation("Saving doctor to databse.");
            _context.Doctors.Add(doctor);
        }

        public void CreateEvent(DoctorEvent doctorEvent)
        {
            _logger.LogInformation($"Saving to database event {doctorEvent.TypeId} for doctor with id = {doctorEvent.Doctor.Id}.");
            _context.DoctorEvents.Add(doctorEvent);
        }

        public async Task<List<Doctor>> GetAll()
        {
            return await _context.Doctors
                .Include(d => d.Specialties)
                .Include(d => d.Title)
                .Include(d => d.Events)
                .Include(d => d.Status)
                .Where(d => d.StatusId == DoctorStatus.Status.Active)
                .ToListAsync();
        }

        public async Task<Doctor> GetByUUID(Guid uuid)
        {
            return await _context.Doctors
                .Include(d => d.Specialties)
                .Include(d => d.Title)
                .Include(d => d.Events)
                .Include(d => d.Status)
                .FirstOrDefaultAsync(d => d.StatusId == DoctorStatus.Status.Active && d.UUID == uuid);
        }

        public async Task<List<Doctor>> GetBySpecalties(List<int> ids)
        {
            return await _context.Doctors
                .Include(d => d.Title)
                .Include(d => d.Events)
                .Include(d => d.Status)
                .Include(d => d.Specialties)
                .Where(
                    d =>
                        d.StatusId == DoctorStatus.Status.Active &&
                        d.Specialties.Any(s => ids.Contains(s.Id))
                )
                .ToListAsync();
        }

        public async Task<List<Specialty>> GetAllSpecialties()
        {
            return await _context.Specialties.ToListAsync();
        }

        public async Task<List<Title>> GetAllTitles()
        {
            return await _context.Titles.ToListAsync();
        }

        public async Task<List<Specialty>> GetSpecialtiesByIds(List<int> ids)
        {
            return await _context.Specialties
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();
        }

        public async Task<Title> GetTitleById(int id)
        {
            return await _context.Titles.FirstOrDefaultAsync(t => t.Id == id);
        }

    }
}