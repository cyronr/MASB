using MABS.Domain.Models.DoctorModels;

namespace MABS.Application.CRUD.Readers.DoctorReader
{
    public interface IDoctorReader
    {
        Task<Doctor> GetDoctorByUUID(Guid uuid);
        Task<Title> GetTitleById(int id);
        Task<List<Specialty>> GetSpecialtiesByIds(List<int> ids);
    }
}
