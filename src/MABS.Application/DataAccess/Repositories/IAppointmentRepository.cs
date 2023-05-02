using MABS.Domain.Models.AppointmentModels;
using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.PatientModels;
using MABS.Domain.Models.ScheduleModels;

namespace MABS.Application.DataAccess.Repositories;

public interface IAppointmentRepository
{
    Task<Appointment?> GetByUUIDAsync(Guid uuid);
    Task<List<Appointment>> GetByPatientAsync(Patient patient);
    Task<List<Appointment>> GetByDoctorAndAddressAsync(Doctor doctor, Address address);
    Task<List<Appointment>> GetByScheduleAsync(Schedule schedule);

    void Create(Appointment appointment);
    void CreateEvent(AppointmentEvent appointmentEvent);
}
