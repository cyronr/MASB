using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.ProfileModels;

namespace MABS.Application.CRUD.Deleters.ProfileDeleters
{
    public class ProfileDeleter : IProfileDeleter
    {
        private readonly IDbOperation _db;
        private readonly IProfileRepository _profileRepository;

        public ProfileDeleter(IDbOperation dbOperation, IProfileRepository profileRepository)
        {
            _db = dbOperation;
            _profileRepository = profileRepository;
        }

        public async Task DeleteAsync(Profile entity, CallerProfile callerProfile)
        {
            if (!_db.IsActiveTransaction())
                throw new TransactionMissingException("Operation needs to be in transaction.");

            entity.StatusId = ProfileStatus.Status.Deleted;

            await _db.Save();
        }
    }
}
