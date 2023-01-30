using MABS.Domain.Models.DoctorModels;

namespace MABS.Application.DataAccess.Repositories
{
    public interface IDoctorRepository
    {
        Task<List<Doctor>> GetAllAsync();
        Task<Doctor?> GetByUUIDAsync(Guid uuid);
        Task<List<Doctor>> GetBySpecaltiesAsync(List<int> ids);
        void Create(Doctor doctor);
        void CreateEvent(DoctorEvent doctorEvent);
        Task<Title?> GetTitleByIdAsync(int id);
        Task<List<Title>> GetAllTitlesAsync();
        Task<Specialty?> GetSpecialtyByIdAsync(int id);
        Task<List<Specialty>> GetAllSpecialtiesAsync();
    }
}