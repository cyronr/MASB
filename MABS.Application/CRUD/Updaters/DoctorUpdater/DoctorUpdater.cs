using MABS.Application.Common.AppProfile;
using MABS.Application.CRUD.Creators.DoctorCreator;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.DoctorModels;

namespace MABS.Application.CRUD.Updaters.DoctorUpdater
{
    public class DoctorUpdater : IDoctorUpdater
    {
        private readonly IDbOperation _db;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IDoctorCreator _doctorCreator;

        public DoctorUpdater(IDbOperation dbOperation, IDoctorRepository doctorRepository, IDoctorCreator doctorCreator)
        {
            _db = dbOperation;
            _doctorRepository = doctorRepository;
            _doctorCreator = doctorCreator;
        }

        public async Task UpdateDoctor(Doctor doctor, CallerProfile callerProfile)
        {
            if (!_db.IsActiveTransaction())
                throw new TransactionMissingException("Operation needs to be in transaction.");

            await _db.Save();
            await _doctorCreator.CreateDoctorEvent(doctor, DoctorEventType.Type.Updated, callerProfile, doctor.ToString());
        }
    }
}
