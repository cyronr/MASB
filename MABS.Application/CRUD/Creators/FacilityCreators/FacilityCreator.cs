using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.FacilityModels;

namespace MABS.Application.CRUD.Creators.FacilityCreators
{
    public class FacilityCreator : IFacilityCreator
    {
        private readonly IDbOperation _db;
        private readonly IFacilityRepository _facilityRepository;

        public FacilityCreator(IDbOperation dbOperation, IFacilityRepository facilityRepository)
        {
            _db = dbOperation;
            _facilityRepository = facilityRepository;
        }

        public async Task CreateAsync(Facility entity, CallerProfile callerProfile)
        {
            if (!_db.IsActiveTransaction())
                throw new TransactionMissingException("Operation needs to be in transaction.");

            _facilityRepository.Create(entity);
            await _db.Save();
        }

        public async Task CreateEventAsync(Facility entity, FacilityEventType.Type eventType, CallerProfile callerProfile, string addInfo)
        {
            if (!_db.IsActiveTransaction())
                throw new TransactionMissingException("Operation needs to be in transaction.");

            _facilityRepository.CreateEvent(new FacilityEvent
            {
                TypeId = eventType,
                Facility = entity,
                AddInfo = addInfo,
                CallerProfile = callerProfile.GetProfileEntity()
            });
            await _db.Save();
        }
    }
}
