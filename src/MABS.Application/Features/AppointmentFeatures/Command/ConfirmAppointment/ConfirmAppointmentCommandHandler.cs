using AutoMapper;
using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.AppointmentFeatures.Common;
using MABS.Application.Features.InternalFeatures.Notifications.SendEmail;
using MABS.Application.ModelsExtensions.AppointmentModelsExtensions;
using MABS.Application.ModelsExtensions.DoctorModelsExtensions;
using MABS.Application.ModelsExtensions.FacilityModelsExtensions;
using MABS.Application.ModelsExtensions.PatientModelsExtensions;
using MABS.Application.ModelsExtensions.ScheduleModelsExtensions;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.AppointmentModels;
using MediatR;
using Microsoft.Extensions.Logging;
using Profile = MABS.Domain.Models.ProfileModels.Profile;

namespace MABS.Application.Features.AppointmentFeatures.Command.ConfirmAppointment;

public class ConfirmAppointmentCommandHandler : IRequestHandler<ConfirmAppointmentCommand, AppointmentDto>
{
    private readonly ILogger<ConfirmAppointmentCommandHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IDbOperation _db;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ICurrentLoggedProfile _currentLoggedProfile;

    private static Profile callerProfile;

    public ConfirmAppointmentCommandHandler(
        ILogger<ConfirmAppointmentCommandHandler> logger,
        IMapper mapper,
        IDbOperation db,
        ICurrentLoggedProfile currentLoggedProfile,
        IAppointmentRepository appointmentRepository,
        IMediator mediator)
    {
        _logger = logger;
        _mapper = mapper;
        _db = db;
        _currentLoggedProfile = currentLoggedProfile;
        _appointmentRepository = appointmentRepository;
        _mediator = mediator;
    }


    public async Task<AppointmentDto> Handle(ConfirmAppointmentCommand command, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Getting current logged profile.");
        callerProfile = CallerProfile.GetCurrentLoggedProfile(_currentLoggedProfile).GetProfileEntity();

        _logger.LogDebug($"Fetching appointment with id = {command.AppointmentId}.");
        var appointment = await new Appointment().GetByUUIDAsync(_appointmentRepository, command.AppointmentId);

        if (appointment.StatusId == AppointmentStatus.Status.Confirmed)
            throw new ConflictException("Wizyta została już potwierdzona.");

        if (appointment.StatusId == AppointmentStatus.Status.Cancelled)
            throw new ConflictException("Wizyta została anulowana.");

        if (appointment.ConfirmationCode != command.ConfirmationCode)
            throw new ConflictException("Nieporawny kod potwierdzający.");

        using (var tran = _db.BeginTransaction())
        {
            try
            {
                appointment.StatusId = AppointmentStatus.Status.Confirmed;
                await _db.Save();

                _appointmentRepository.CreateEvent(new AppointmentEvent
                {
                    Appointment = appointment,
                    TypeId = AppointmentEventType.Type.Confirmed,
                    CallerProfile = callerProfile
                });
                await _db.Save();

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

    private async Task SendEmail(Appointment appointment)
    {
        string subject = "Potwierdzono wizytę w serwisie MediReserve.";
        string body = @$"
            Witaj! <br />
            <br />
            Twoja wizyta u lekarza {appointment.Schedule.Doctor.Firstname} {appointment.Schedule.Doctor.Lastname} 
            na {appointment.Date} {appointment.Time.ToString("hh\\:mm")} została potwierdzona. 
            <br />
            Pozdrowienia, <br />
            Zespół MediReserve :) <br />
        ";
        await _mediator.Send(new SendEmailCommand(subject, body, appointment.Patient.Profile.Email));
    }
}
