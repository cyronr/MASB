using MABS.Domain.Models.DoctorModels;

namespace MABS.Application.DataAccess.Repositories
{
    public interface IDoctorRepository
    {
        Task<List<Doctor>> GetAll();
        Task<Doctor> GetByUUID(Guid uuid);
        Task<List<Doctor>> GetBySpecalties(List<int> ids);
        void Create(Doctor doctor);
        void CreateEvent(DoctorEvent doctorEvent);
        Task<Title> GetTitleById(int id);
        Task<List<Title>> GetAllTitles();
        Task<List<Specialty>> GetSpecialtiesByIds(List<int> ids);
        Task<List<Specialty>> GetAllSpecialties();
    }
}