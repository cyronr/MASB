using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.ProfileModels;

namespace MABS.Application.CRUD.Updaters.FacilityUpdaters
{
    public class FacilityUpdater : IFacilityUpdater
    {
        private readonly IDbOperation _db;
        private readonly IFacilityRepository _facilityRepository;

        public FacilityUpdater(IDbOperation dbOperation, IFacilityRepository facilityRepository)
        {
            _db = dbOperation;
            _facilityRepository = facilityRepository;
        }

        public async Task UpdateAsync(Facility entity, CallerProfile callerProfile)
        {
            if (!_db.IsActiveTransaction())
                throw new TransactionMissingException("Operation needs to be in transaction.");

            await _db.Save();
        }
    }
}
