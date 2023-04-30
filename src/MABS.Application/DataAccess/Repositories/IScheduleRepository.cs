using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.ScheduleModels;

namespace MABS.Application.DataAccess.Repositories;

public interface IScheduleRepository
{
    Task<List<Schedule>> GetByDoctorAndFacilityAsync(Doctor doctor, Facility facility);
    void Create(Schedule schedule);
    void CreateEvent(ScheduleEvent scheduleEvent);
}
