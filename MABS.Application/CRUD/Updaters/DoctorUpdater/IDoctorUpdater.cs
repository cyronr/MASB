using MABS.Application.Common.AppProfile;
using MABS.Domain.Models.DoctorModels;

namespace MABS.Application.CRUD.Updaters.DoctorUpdater
{
    public interface IDoctorUpdater
    {
        Task UpdateDoctor(Doctor doctor, CallerProfile callerProfile);
    }
}
