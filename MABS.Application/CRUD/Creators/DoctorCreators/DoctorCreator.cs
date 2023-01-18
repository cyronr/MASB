using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.DoctorModels;
using System.Numerics;

namespace MABS.Application.CRUD.Creators.DoctorCreators
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

        public async Task CreateAsync(Doctor entity, CallerProfile callerProfile)
        {
            if (!_db.IsActiveTransaction())
                throw new TransactionMissingException("Operation needs to be in transaction.");

            _doctorRepository.Create(entity);
            await _db.Save();
        }

        public async Task CreateEventAsync(Doctor entity, DoctorEventType.Type eventType, CallerProfile callerProfile, string addInfo)
        {
            if (!_db.IsActiveTransaction())
                throw new TransactionMissingException("Operation needs to be in transaction.");

            _doctorRepository.CreateEvent(new DoctorEvent
            {
                TypeId = eventType,
                Doctor = entity,
                AddInfo = addInfo,
                CallerProfile = callerProfile.GetProfileEntity()
            });
            await _db.Save();
        }
    }
}
