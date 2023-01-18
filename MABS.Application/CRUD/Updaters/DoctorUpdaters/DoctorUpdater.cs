using MABS.Application.Common.AppProfile;
using MABS.Application.CRUD.Creators.DoctorCreators;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.DoctorModels;

namespace MABS.Application.CRUD.Updaters.DoctorUpdaters
{
    public class DoctorUpdater : IDoctorUpdater
    {
        private readonly IDbOperation _db;
        private readonly IDoctorRepository _doctorRepository;

        public DoctorUpdater(IDbOperation dbOperation, IDoctorRepository doctorRepository)
        {
            _db = dbOperation;
            _doctorRepository = doctorRepository;
        }

        public async Task UpdateAsync(Doctor entity, CallerProfile callerProfile)
        {
            if (!_db.IsActiveTransaction())
                throw new TransactionMissingException("Operation needs to be in transaction.");

            await _db.Save();
        }
    }
}
