using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Models.AppointmentModels;
using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.PatientModels;
using MABS.Domain.Models.ScheduleModels;
using MABS.Infrastructure.Data;
using MABSAPI.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Numerics;
using static Nest.JoinField;

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

    public async Task<List<Appointment>> GetByAddressAsync(Address address)
    {
        var appointments = await _context.Appointments
            .Include(p => p.Status)
            .Include(p => p.Events)
            .Include(p => p.Schedule)
                .ThenInclude(s => s.Doctor)
                    .ThenInclude(d => d.Title)
            .Include(p => p.Schedule)
                .ThenInclude(s => s.Doctor)
                    .ThenInclude(d => d.Specialties)
            .Include(p => p.Schedule)
                .ThenInclude(s => s.Address)
                    .ThenInclude(a => a.StreetType)
            .Include(p => p.Schedule)
                .ThenInclude(s => s.Address)
                    .ThenInclude(a => a.Facility)
            .Include(p => p.Patient)
                .ThenInclude(pa => pa.Profile)
            .Where(s => s.Schedule.Address == address)
            .ToListAsync();

        return appointments.OrderByDescending(a => a.Events
                .Where(e => e.TypeId == AppointmentEventType.Type.Created)
                .Max(e => e.Timestamp)
            )
            .ToList();
    }

    public async Task<List<Appointment>> GetByDoctorAndAddressAsync(Doctor doctor, Address address)
    {
        var appointments = await _context.Appointments
            .Include(p => p.Status)
            .Include(p => p.Events)
            .Include(p => p.Schedule)
                .ThenInclude(s => s.Doctor)
                    .ThenInclude(d => d.Title)
            .Include(p => p.Schedule)
                .ThenInclude(s => s.Address)
                    .ThenInclude(a => a.StreetType)
            .Include(p => p.Schedule)
                .ThenInclude(s => s.Address)
                    .ThenInclude(a => a.Facility)
            .Include(p => p.Patient)
                .ThenInclude(pa => pa.Profile)
            .Where(a => 
                a.Schedule.Doctor == doctor &&
                a.Schedule.Address == address
            )
            .ToListAsync();

        return appointments.OrderByDescending(a => a.Events
                .Where(e => e.TypeId == AppointmentEventType.Type.Created)
                .Max(e => e.Timestamp)
            )
            .ToList();
    }

    public async Task<List<Appointment>> GetByDoctorAsync(Doctor doctor)
    {
        var appointments = await _context.Appointments
            .Include(p => p.Status)
            .Include(p => p.Events)
            .Include(p => p.Schedule)
                .ThenInclude(s => s.Doctor)
                    .ThenInclude(d => d.Title)
            .Include(p => p.Schedule)
                .ThenInclude(s => s.Address)
                    .ThenInclude(a => a.StreetType)
            .Include(p => p.Schedule)
                .ThenInclude(s => s.Address)
                    .ThenInclude(a => a.Facility)
            .Include(p => p.Patient)
                .ThenInclude(pa => pa.Profile)
            .Where(s => s.Schedule.Doctor == doctor)
            .ToListAsync();

        return appointments.OrderByDescending(a => a.Events
                .Where(e => e.TypeId == AppointmentEventType.Type.Created)
                .Max(e => e.Timestamp)
            )
            .ToList();
    }

    public async Task<List<Appointment>> GetByPatientAsync(Patient patient)
    {
        var appointments = await _context.Appointments
            .Include(p => p.Status)
            .Include(p => p.Events)
            .Include(p => p.Schedule)
                .ThenInclude(s => s.Doctor)
                    .ThenInclude(d => d.Title)
            .Include(p => p.Schedule)
                .ThenInclude(s => s.Doctor)
                    .ThenInclude(d => d.Specialties)
            .Include(p => p.Schedule)
                .ThenInclude(s => s.Address)
                    .ThenInclude(a => a.StreetType)
            .Include(p => p.Schedule)
                .ThenInclude(s => s.Address)
                    .ThenInclude(a => a.Facility)
            .Include(p => p.Patient)
                .ThenInclude(pa => pa.Profile)
            .Where(s => s.Patient == patient)
            .OrderBy(a => a.Id)
            .ToListAsync();

        return appointments.OrderByDescending(a => a.Events
                .Where(e => e.TypeId == AppointmentEventType.Type.Created)
                .Max(e => e.Timestamp)
            )
            .ToList();
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
                    .ThenInclude(d => d.Title)
            .Include(p => p.Schedule)
                .ThenInclude(s => s.Doctor)
                    .ThenInclude(d => d.Specialties)
            .Include(p => p.Schedule)
                .ThenInclude(s => s.Address)
                    .ThenInclude(a => a.StreetType)
            .Include(p => p.Schedule)
                .ThenInclude(s => s.Address)
                    .ThenInclude(a => a.Facility)
            .Include(p => p.Patient)
                .ThenInclude(pa => pa.Profile)
            .FirstOrDefaultAsync(p => p.UUID == uuid);
    }
}
