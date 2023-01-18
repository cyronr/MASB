using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.PatientModels;
using MABS.Domain.Models.ProfileModels;

namespace MABS.Application.CRUD.Creators.ProfileCreators
{
    public class ProfileCreator : IProfileCreator
    {
        private readonly IDbOperation _db;
        private readonly IProfileRepository _profileRepository;

        public ProfileCreator(IDbOperation dbOperation, IProfileRepository profileRepository)
        {
            _db = dbOperation;
            _profileRepository = profileRepository;
        }

        public async Task CreateAsync(Profile entity, CallerProfile callerProfile)
        {
            if (!_db.IsActiveTransaction())
                throw new TransactionMissingException("Operation needs to be in transaction.");

            _profileRepository.Create(entity);
            await _db.Save();
        }

        public async Task CreateEventAsync(Profile entity, ProfileEventType.Type eventType, CallerProfile callerProfile, string addInfo)
        {
            if (!_db.IsActiveTransaction())
                throw new TransactionMissingException("Operation needs to be in transaction.");

            _profileRepository.CreateEvent(new ProfileEvent
            {
                TypeId = eventType,
                Profile = entity,
                AddInfo = addInfo,
                CallerProfile = callerProfile.GetProfileEntity()
            });
            await _db.Save();
        }
    }
}
