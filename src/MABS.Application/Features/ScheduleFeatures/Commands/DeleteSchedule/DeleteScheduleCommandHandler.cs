using AutoMapper;
using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.ScheduleFeatures.Common;
using MABS.Application.ModelsExtensions.ScheduleModelsExtensions;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.ScheduleModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.ScheduleFeatures.Commands.DeleteSchedule;

public class DeleteScheduleCommandHandler : IRequestHandler<DeleteScheduleCommand, ScheduleDto>
{
    private readonly ILogger<DeleteScheduleCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IDbOperation _db;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly ICurrentLoggedProfile _currentLoggedProfile;
    private readonly IAppointmentRepository _appointmentRepository;

    public DeleteScheduleCommandHandler(
        ILogger<DeleteScheduleCommandHandler> logger,
        IMapper mapper,
        IDbOperation db,
        IScheduleRepository scheduleRepository,
        ICurrentLoggedProfile currentLoggedProfile,
        IAppointmentRepository appointmentRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _db = db;
        _scheduleRepository = scheduleRepository;
        _currentLoggedProfile = currentLoggedProfile;
        _appointmentRepository = appointmentRepository;
    }


    public async Task<ScheduleDto> Handle(DeleteScheduleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Getting current logged profile.");
        var callerProfile = CallerProfile.GetCurrentLoggedProfile(_currentLoggedProfile).GetProfileEntity();

        _logger.LogDebug($"Getting schedule with id = {command.ScheduleId}.");
        var schedule = await new Schedule().GetByUUIDAsync(_scheduleRepository, command.ScheduleId);

        if (await AreAppointmentsForSchedule(schedule))
            throw new ConflictException("Dla harmonogramu istnieją wizyty.", $"ScheduleId = {schedule.UUID}");

        using (var tran = _db.BeginTransaction())
        {
            try
            {
                schedule.StatusId = ScheduleStatus.Status.Deleted;
                await _db.Save();

                _scheduleRepository.CreateEvent(new ScheduleEvent
                {
                    Schedule = schedule,
                    TypeId = ScheduleEventType.Type.Deleted,
                    CallerProfile = callerProfile
                });
                await _db.Save();

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw;
            }
        }

        return _mapper.Map<ScheduleDto>(schedule);
    }

    private async Task<bool> AreAppointmentsForSchedule(Schedule schedule)
    {
        var currentDate = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
        var currentTime = new TimeOnly(DateTime.Now.Hour, DateTime.Now.Minute);

        var appointments = await _appointmentRepository.GetByScheduleAsync(schedule);
        return appointments.Any(a => 
            a.Date >= currentDate &&
            a.Time >= currentTime
        );
    }
}

