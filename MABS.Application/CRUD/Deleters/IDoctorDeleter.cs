using MABS.Application.Common.AppProfile;
using MABS.Domain.Models.DoctorModels;

namespace MABS.Application.CRUD.Deleters.DoctorDeleter
{
    public interface IDoctorDeleter
    {
        Task DeleteDoctor(Doctor doctor, CallerProfile callerProfile);
    }
}
