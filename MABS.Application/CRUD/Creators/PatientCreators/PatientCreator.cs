using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.PatientModels;

namespace MABS.Application.CRUD.Creators.PatientCreators
{
    public class PatientCreator : IPatientCreator
    {
        private readonly IDbOperation _db;
        private readonly IPatientRepository _patientRepository;

        public PatientCreator(IDbOperation dbOperation, IPatientRepository patientRepository)
        {
            _db = dbOperation;
            _patientRepository = patientRepository;
        }

        public async Task CreateAsync(Patient entity, CallerProfile callerProfile)
        {
            if (!_db.IsActiveTransaction())
                throw new TransactionMissingException("Operation needs to be in transaction.");

            _patientRepository.Create(entity);
            await _db.Save();
        }

        public async Task CreateEventAsync(Patient entity, PatientEventType.Type eventType, CallerProfile callerProfile, string addInfo)
        {
            if (!_db.IsActiveTransaction())
                throw new TransactionMissingException("Operation needs to be in transaction.");

            _patientRepository.CreateEvent(new PatientEvent
            {
                TypeId = eventType,
                Patient = entity,
                AddInfo = addInfo,
                CallerProfile = callerProfile.GetProfileEntity()
            });
            await _db.Save();
        }
    }
}
