using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.ProfileModels;

namespace MABS.Application.CRUD.Updaters.ProfileUpdaters
{
    public class ProfileUpdater : IProfileUpdater
    {
        private readonly IDbOperation _db;
        private readonly IProfileRepository _profileRepository;

        public ProfileUpdater(IDbOperation dbOperation, IProfileRepository profileRepository)
        {
            _db = dbOperation;
            _profileRepository = profileRepository;
        }

        public async Task UpdateAsync(Profile entity, CallerProfile callerProfile)
        {
            if (!_db.IsActiveTransaction())
                throw new TransactionMissingException("Operation needs to be in transaction.");

            await _db.Save();
        }
    }
}
