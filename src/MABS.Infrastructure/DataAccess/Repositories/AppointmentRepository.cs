using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Models.AppointmentModels;
using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.PatientModels;
using MABS.Domain.Models.ScheduleModels;
using MABS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MABS.Infrastructure.DataAccess.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ILogger<AppointmentRepository> _logger;
    private readonly DataContext _context;

    public AppointmentRepository(ILogger<AppointmentRepository> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    public void Create(Appointment appointment)
    {
        _logger.LogInformation("Saving appointment to databse.");
        _context.Appointments.Add(appointment);
    }

    public void CreateEvent(AppointmentEvent appointmentEvent)
    {
        _logger.LogInformation($"Saving to database event {appointmentEvent.TypeId} for appointment with id = {appointmentEvent.Appointment.Id}.");
        _context.AppointmentEvents.Add(appointmentEvent);
    }

    public async Task<List<Appointment>> GetByDoctorAndFacilityAsync(Doctor doctor, Facility facility)
    {
        return await _context.Appointments
            .Include(a => a.Schedule)
            .Include(a => a.Events)
            .Where(a => 
                a.StatusId != AppointmentStatus.Status.Cancelled &&
                a.Schedule.Doctor == doctor &&
                a.Schedule.Facility == facility
            )
            .ToListAsync();
    }

    public async Task<List<Appointment>> GetByPatientAsync(Patient patient)
    {
        return await _context.Appointments
            .Where(s => s.Patient == patient)
            .ToListAsync();
    }

    public async Task<List<Appointment>> GetByScheduleAsync(Schedule schedule)
    {
        return await _context.Appointments
            .Where(s =>
                s.StatusId != AppointmentStatus.Status.Cancelled &&
                s.Schedule == schedule
            )
            .ToListAsync();
    }

    public async Task<Appointment?> GetByUUIDAsync(Guid uuid)
    {
        return await _context.Appointments
            .Include(p => p.Status)
            .Include(p => p.Events)
            .Include(p => p.Schedule)
                .ThenInclude(s => s.Doctor)
            .Include(p => p.Schedule)
                .ThenInclude(s => s.Facility)
            .Include(p => p.Patient)
                .ThenInclude(pa => pa.Profile)
            .FirstOrDefaultAsync(p => p.UUID == uuid);
    }
}
