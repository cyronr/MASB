using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.DoctorModels;

namespace MABS.Application.CRUD.Deleters.DoctorDeleters
{
    public class DoctorDeleter : IDoctorDeleter
    {
        private readonly IDbOperation _db;
        private readonly IDoctorRepository _doctorRepository;

        public DoctorDeleter(IDbOperation dbOperation, IDoctorRepository doctorRepository)
        {
            _db = dbOperation;
            _doctorRepository = doctorRepository;
        }

        public async Task DeleteAsync(Doctor entity, CallerProfile callerProfile)
        {
            if (!_db.IsActiveTransaction())
                throw new TransactionMissingException("Operation needs to be in transaction.");

            entity.StatusId = DoctorStatus.Status.Deleted;

            await _db.Save();
        }
    }
}
