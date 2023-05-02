using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.ScheduleModels;

namespace MABS.Application.DataAccess.Repositories;

public interface IScheduleRepository
{
    Task<Schedule?> GetByUUIDAsync(Guid uuid);
    Task<List<Schedule>> GetByDoctorAsync(Doctor doctor);
    Task<List<Schedule>> GetByDoctorAndAddressAsync(Doctor doctor, Address address);
    void Create(Schedule schedule);
    void CreateEvent(ScheduleEvent scheduleEvent);
}
