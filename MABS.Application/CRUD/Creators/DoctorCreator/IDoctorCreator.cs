using MABS.Application.Common.AppProfile;
using MABS.Domain.Models.DoctorModels;

namespace MABS.Application.CRUD.Creators.DoctorCreator
{
    public interface IDoctorCreator
    {
        Task CreateDoctor(Doctor doctor, CallerProfile callerProfile);
        Task CreateDoctorEvent(Doctor doctor, DoctorEventType.Type eventType, CallerProfile callerProfile, string AddInfo);
    }
}
