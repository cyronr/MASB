using AutoMapper;
using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.AppointmentFeatures.Common;
using MABS.Application.Features.DoctorFeatures.Queries.GetTimeSlots;
using MABS.Application.Features.InternalFeatures.Notifications.SendEmail;
using MABS.Application.ModelsExtensions.DoctorModelsExtensions;
using MABS.Application.ModelsExtensions.FacilityModelsExtensions;
using MABS.Application.ModelsExtensions.PatientModelsExtensions;
using MABS.Application.ModelsExtensions.ScheduleModelsExtensions;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.AppointmentModels;
using MABS.Domain.Models.PatientModels;
using MABS.Domain.Models.ProfileModels;
using MABS.Domain.Models.ScheduleModels;
using MediatR;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Profile = MABS.Domain.Models.ProfileModels.Profile;

namespace MABS.Application.Features.AppointmentFeatures.Command.CreateAppointment;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, AppointmentDto>
{
    private readonly ILogger<CreateAppointmentCommandHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IDbOperation _db;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly ICurrentLoggedProfile _currentLoggedProfile;

    private static Profile callerProfile;

    public CreateAppointmentCommandHandler(
        ILogger<CreateAppointmentCommandHandler> logger,
        IMapper mapper,
        IDbOperation db,
        ICurrentLoggedProfile currentLoggedProfile,
        IAppointmentRepository appointmentRepository,
        IPatientRepository patientRepository,
        IMediator mediator,
        IScheduleRepository scheduleRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _db = db;
        _currentLoggedProfile = currentLoggedProfile;
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
        _mediator = mediator;
        _scheduleRepository = scheduleRepository;
    }


    public async Task<AppointmentDto> Handle(CreateAppointmentCommand command, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Getting current logged profile.");
        callerProfile = CallerProfile.GetCurrentLoggedProfile(_currentLoggedProfile).GetProfileEntity();

        _logger.LogDebug($"Fetching patient with id = {command.PatientId}.");
        var patient = await new Patient().GetByUUIDAsync(_patientRepository, command.PatientId);

        _logger.LogDebug($"Fetching schedule with id = {command.ScheduleId}.");
        var schedule = await new Schedule().GetByUUIDAsync(_scheduleRepository, command.ScheduleId);

        if (!await IsTimeSlotAvail(command, schedule.Doctor.UUID, schedule.Facility.UUID))
            throw new ConflictException("Wybrany termin nie jest dostępny");

        Appointment appointment;
        using (var tran = _db.BeginTransaction())
        {
            try
            {
                appointment = await CreateAppointment(patient, schedule, command.Date, command.Time);
                await SendEmail(appointment);

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw;
            }
        }

        return _mapper.Map<AppointmentDto>(appointment);
    }

    private async Task<bool> IsTimeSlotAvail(CreateAppointmentCommand timeSlot, Guid doctorId, Guid facilityId)
    {
        var query = new GetTimeSlotsQuery(doctorId, facilityId);
        var results = await _mediator.Send(query);
        
        return results.Any(r =>
            r.ScheduleId == timeSlot.ScheduleId &&
            r.Date == timeSlot.Date &&
            r.Time == timeSlot.Time &&
            r.Available
        );
    }

    private async Task<Appointment> CreateAppointment(Patient patient, Schedule schedule, DateOnly date, TimeOnly time)
    {
        var appointment = new Appointment
        {
            UUID = Guid.NewGuid(),
            StatusId = AppointmentStatus.Status.Prepared,
            Schedule = schedule,
            Patient = patient,
            Date = date,
            Time = time,
            ConfirmationCode = GenerateConfirmationCode()
        };

        _appointmentRepository.Create(appointment);
        await _db.Save();

        _appointmentRepository.CreateEvent(new AppointmentEvent
        {
            Appointment = appointment,
            TypeId = AppointmentEventType.Type.Created,
            CallerProfile = callerProfile
        });
        await _db.Save();

        return appointment;
    }

    private int GenerateConfirmationCode()
    {
        Random random = new Random();
        return random.Next(100000, 999999);
    }

    private async Task SendEmail(Appointment appointment)
    {
        string subject = "Potwierdź wizytę w serwisie MediReserve.";
        string body = @$"
            Witaj! <br />
            <br />
            Twoja wizyta u lekarza {appointment.Schedule.Doctor.Firstname} {appointment.Schedule.Doctor.Lastname} 
            na {appointment.Date} {appointment.Time.ToString("hh\\:mm")} została wstępnie zarezerwowana. 
            <br />
            Potwierdź wizytę wpisując kod <strong>{appointment.ConfirmationCode}</strong>. 
            <br />
            <br />  
            <i>Uwaga: Niepotwierdzenie wizyty w ciągu 15 minut skutkuje utratą wybranego temrinu.</i>
            <br />
            Pozdrowienia, <br />
            Zespół MediReserve :) <br />
        ";
        await _mediator.Send(new SendEmailCommand(subject, body, appointment.Patient.Profile.Email));
    }
}
