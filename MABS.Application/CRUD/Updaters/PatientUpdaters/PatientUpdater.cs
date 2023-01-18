using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.PatientModels;

namespace MABS.Application.CRUD.Updaters.PatientUpdaters
{
    public class PatientUpdater : IPatientUpdater
    {
        private readonly IDbOperation _db;
        private readonly IPatientRepository _patientRepository;

        public PatientUpdater(IDbOperation dbOperation, IPatientRepository patientRepository)
        {
            _db = dbOperation;
            _patientRepository = patientRepository;
        }

        public async Task UpdateAsync(Patient entity, CallerProfile callerProfile)
        {
            if (!_db.IsActiveTransaction())
                throw new TransactionMissingException("Operation needs to be in transaction.");

            await _db.Save();
        }
    }
}
