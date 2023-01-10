using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Models.PatientModels;
using MABS.Domain.Models.ProfileModels;
using MABS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MABS.Infrastructure.DataAccess.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ILogger<PatientRepository> _logger;
        private readonly DataContext _context;
        public PatientRepository(ILogger<PatientRepository> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void Create(Patient patient)
        {
            _logger.LogInformation("Saving patient to databse.");
            _context.Patients.Add(patient);
        }

        public void CreateEvent(PatientEvent patientEvent)
        {
            _logger.LogInformation($"Saving to database event {patientEvent.TypeId} for profile with id = {patientEvent.Patient.Id}.");
            _context.PatientEvents.Add(patientEvent);
        }

        public async Task<Patient> GetByProfile(Profile profile)
        {
            return await _context.Patients
                .Include(p => p.Status)
                .Include(p => p.Events)
                .FirstOrDefaultAsync(p => p.StatusId == PatientStatus.Status.Active && p.Profile.Equals(profile));
        }

        public async Task<Patient> GetByUUID(Guid uuid)
        {
            return await _context.Patients
                .Include(p => p.Status)
                .Include(p => p.Events)
                .FirstOrDefaultAsync(p => p.StatusId == PatientStatus.Status.Active && p.UUID == uuid);
        }
    }
}
