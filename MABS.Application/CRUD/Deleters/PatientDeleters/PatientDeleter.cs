using MABS.Application.Common.AppProfile;
using MABS.Application.CRUD.Creators.PatientCreators;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.PatientModels;

namespace MABS.Application.CRUD.Deleters.PatientDeleters
{
    public class PatientDeleter : IPatientDeleter
    {
        private readonly IDbOperation _db;
        private readonly IPatientRepository _patientRepository;

        public PatientDeleter(IDbOperation dbOperation, IPatientRepository patientRepository)
        {
            _db = dbOperation;
            _patientRepository = patientRepository;
        }

        public async Task DeleteAsync(Patient entity, CallerProfile callerProfile)
        {
            if (!_db.IsActiveTransaction())
                throw new TransactionMissingException("Operation needs to be in transaction.");

            entity.StatusId = PatientStatus.Status.Deleted;

            await _db.Save();
        }
    }
}
