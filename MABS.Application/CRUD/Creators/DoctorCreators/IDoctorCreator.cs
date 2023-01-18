using MABS.Application.Common.AppProfile;
using MABS.Domain.Models.DoctorModels;

namespace MABS.Application.CRUD.Creators.DoctorCreators
{
    public interface IDoctorCreator : ICreator<Doctor, DoctorEventType.Type>
    {
    }
}
