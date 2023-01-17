using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.DoctorModels;

namespace MABS.Application.CRUD.Creators.DoctorCreator
{
    public class DoctorCreator : IDoctorCreator
    {
        private readonly IDbOperation _db;
        private readonly IDoctorRepository _doctorRepository;

        public DoctorCreator(IDbOperation dbOperation, IDoctorRepository doctorRepository)
        {
            _db = dbOperation;
            _doctorRepository = doctorRepository;
        }


        public async Task CreateDoctor(Doctor doctor, CallerProfile callerProfile)
        {
            if (!_db.IsActiveTransaction())
                throw new TransactionMissingException("Operation needs to be in transaction.");

            _doctorRepository.Create(doctor);
            await _db.Save();

            await CreateDoctorEvent(doctor, DoctorEventType.Type.Created, callerProfile, doctor.ToString());
        }

        public async Task CreateDoctorEvent(Doctor doctor, DoctorEventType.Type eventType, CallerProfile callerProfile, string AddInfo)
        {
            if (!_db.IsActiveTransaction())
                throw new TransactionMissingException("Operation needs to be in transaction.");

            _doctorRepository.CreateEvent(new DoctorEvent
            {
                TypeId = eventType,
                Doctor = doctor,
                AddInfo = doctor.ToString(),
                CallerProfile = callerProfile.GetProfileEntity()
            });
            await _db.Save();
        }
    }
}
