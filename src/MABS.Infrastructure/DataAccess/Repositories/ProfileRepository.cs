using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Models.ProfileModels;
using MABS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MABS.Infrastructure.DataAccess.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ILogger<ProfileRepository> _logger;
        private readonly DataContext _context;
        public ProfileRepository(ILogger<ProfileRepository> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void Create(Profile profile)
        {
            _logger.LogInformation("Saving profile to databse.");
            _context.Profiles.Add(profile);
        }

        public void CreateEvent(ProfileEvent profileEvent)
        {
            _logger.LogInformation($"Saving to database event {profileEvent.TypeId} for profile with id = {profileEvent.Profile.Id}.");
            _context.ProfileEvents.Add(profileEvent);
        }

        public async Task<Profile?> GetByEmailAsync(string email)
        {
            return await _context.Profiles
                .FirstOrDefaultAsync(p => p.StatusId != ProfileStatus.Status.Deleted && p.Email == email);
        }

        public async Task<Profile?> GetByUUIDAsync(Guid uuid)
        {
            return await _context.Profiles
                .Include(p => p.Status)
                .Include(p => p.Type)
                .Include(p => p.Events)
                .FirstOrDefaultAsync(p => p.StatusId != ProfileStatus.Status.Deleted && p.UUID == uuid);
        }

        public Profile? GetByUUID(Guid uuid)
        {
            return _context.Profiles
                .Include(p => p.Status)
                .Include(p => p.Type)
                .Include(p => p.Events)
                .FirstOrDefault(p => p.StatusId != ProfileStatus.Status.Deleted && p.UUID == uuid);
        }


        public Task<List<ProfileType>> GetAllTypesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Guid?> GetFacilityIdByProfileIdAsync(Guid uuid)
        {
            var profile = await _context.Profiles
                .Include(p => p.Status)
                .Include(p => p.Type)
                .Include(p => p.Facility)
                .FirstOrDefaultAsync(p => p.StatusId != ProfileStatus.Status.Deleted && p.TypeId == ProfileType.Type.Facility && p.UUID == uuid);

            if (profile is not null)
                return profile.Facility.UUID;
            else
                return null;
        }

        public async Task<Guid?> GetPatientIdByProfileIdAsync(Guid uuid)
        {
            var profile = await _context.Profiles
                .Include(p => p.Status)
                .Include(p => p.Type)
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(p => p.StatusId != ProfileStatus.Status.Deleted && p.TypeId == ProfileType.Type.Patient && p.UUID == uuid);

            if (profile is not null)
                return profile.Patient.UUID;
            else
                return null;
        }
    }
}
