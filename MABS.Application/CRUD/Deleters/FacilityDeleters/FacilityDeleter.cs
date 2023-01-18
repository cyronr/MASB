using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.ProfileModels;

namespace MABS.Application.CRUD.Deleters.FacilityDeleters
{
    public class FacilityDeleter : IFacilityDeleter
    {
        private readonly IDbOperation _db;
        private readonly IFacilityRepository _facilityRepository;

        public FacilityDeleter(IDbOperation dbOperation, IFacilityRepository facilityRepository)
        {
            _db = dbOperation;
            _facilityRepository = facilityRepository;
        }

        public async Task DeleteAsync(Facility entity, CallerProfile callerProfile)
        {
            if (!_db.IsActiveTransaction())
                throw new TransactionMissingException("Operation needs to be in transaction.");

            entity.StatusId = FacilityStatus.Status.Deleted;

            await _db.Save();
        }
    }
}
