using MABS.Domain.Models.DoctorModels;

namespace MABS.Application.CRUD.Readers.DoctorReaders
{
    public interface IDoctorReader : IReader<Doctor>
    {
        Task<List<Doctor>> GetBySpecaltiesAsync(List<int> ids);
        Task<Title> GetTitleByIdAsync(int id);
        Task<List<Title>> GetAllTitlesAsync();
        Task<List<Specialty>> GetSpecialtiesByIdsAsync(List<int> ids);
        Task<List<Specialty>> GetAllSpecialtiesAsync();
    }
}
