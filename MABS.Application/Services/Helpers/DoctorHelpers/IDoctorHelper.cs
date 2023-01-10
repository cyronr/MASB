using MABS.Domain.Models.DoctorModels;

namespace MABS.Application.Services.Helpers.DoctorHelpers
{
    public interface IDoctorHelper
    {
        Task<Doctor> GetDoctorByUUID(Guid uuid);
        Task<Title> GetTitleById(int id);
        Task<List<Specialty>> GetSpecialtiesByIds(List<int> ids);
    }
}
