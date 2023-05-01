using AutoMapper;
using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.ScheduleFeatures.Common;
using MABS.Application.ModelsExtensions.DoctorModelsExtensions;
using MABS.Application.ModelsExtensions.FacilityModelsExtensions;
using MABS.Application.ModelsExtensions.ScheduleModelsExtensions;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.ScheduleModels;
using MABS.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Data;
using Profile = MABS.Domain.Models.ProfileModels.Profile;

namespace MABS.Application.Features.ScheduleFeatures.Commands.UpdateSchedule;

public class UpdateScheduleCommandHandler : IRequestHandler<UpdateScheduleCommand, List<ScheduleDto>>
{
    private readonly ILogger<UpdateScheduleCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IDbOperation _db;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IFacilityRepository _facilityRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ICurrentLoggedProfile _currentLoggedProfile;

    public UpdateScheduleCommandHandler(
        ILogger<UpdateScheduleCommandHandler> logger,
        IMapper mapper,
        IDbOperation db,
        IScheduleRepository scheduleRepository,
        ICurrentLoggedProfile currentLoggedProfile,
        IFacilityRepository facilityRepository,
        IDoctorRepository doctorRepository,
        IAppointmentRepository appointmentRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _db = db;
        _scheduleRepository = scheduleRepository;
        _currentLoggedProfile = currentLoggedProfile;
        _facilityRepository = facilityRepository;
        _doctorRepository = doctorRepository;
        _appointmentRepository = appointmentRepository;
    }


    public async Task<List<ScheduleDto>> Handle(UpdateScheduleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Getting current logged profile.");
        var callerProfile = CallerProfile.GetCurrentLoggedProfile(_currentLoggedProfile).GetProfileEntity();

        _logger.LogDebug($"Fetching facility with id = {command.FacilityId}.");
        var facility = await new Facility().GetByUUIDAsync(_facilityRepository, command.FacilityId);

        _logger.LogDebug($"Fetching doctor with id = {command.DoctorId}.");
        var doctor = await new Doctor().GetByUUIDAsync(_doctorRepository, command.DoctorId);

        var schedulesToDelete = command.Schedules
            .Where(s => s.OperationType == ScheduleOperationType.Delete)
            .Select(s => s.Id)
            .ToList();

        var schedulesToInsert = command.Schedules.Where(s => s.OperationType == ScheduleOperationType.Insert).ToList();

        using (var tran = _db.BeginTransaction())
        {
            try
            {
                await DeleteSchedules(schedulesToDelete, callerProfile);
                await InsertSchedules(schedulesToInsert, doctor, facility, callerProfile);

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw;
            }
        }
        
        var schedules = await _scheduleRepository.GetByDoctorAndFacilityAsync(doctor, facility);
        return schedules.Select(s => _mapper.Map<ScheduleDto>(s)).ToList();
    }

    private async Task DeleteSchedules(List<Guid> scheduleIds, Profile callerProfile)
    {
        foreach(var scheduleId in scheduleIds)
        {
            var schedule = await new Schedule().GetByUUIDAsync(_scheduleRepository, scheduleId);
            if (await AreAppointmentsForSchedule(schedule))
                throw new ConflictException("Dla harmonogramu istnieją wizyty.", $"ScheduleId = {schedule.UUID}");

            schedule.StatusId = ScheduleStatus.Status.Deleted;
            await _db.Save();

            _scheduleRepository.CreateEvent(new ScheduleEvent
            {
                Schedule = schedule,
                TypeId = ScheduleEventType.Type.Deleted,
                CallerProfile = callerProfile
            });
            await _db.Save();
        }
    }

    private async Task InsertSchedules(List<ScheduleDetails> schedules, Doctor doctor, Facility facility, Profile callerProfile)
    {
        var currentSchedules = await _scheduleRepository.GetByDoctorAndFacilityAsync(doctor, facility);

        foreach (var schedule in schedules)
        {
            var existingSchedule = currentSchedules
                .FirstOrDefault(s =>
                    s.DayOfWeek == schedule.DayOfWeek &&
                    s.StartTime >= schedule.StartTime.StripSeconds() &&
                    s.EndTime <= schedule.EndTime.StripSeconds() &&
                    s.ValidDateFrom >= schedule.ValidDateFrom &&
                    s.ValidDateTo <= schedule.ValidDateTo
                );
            if (existingSchedule is not null)
                throw new AlreadyExistsException("Istnieje już harmonogram dla podanego zakresu czasu.");

            var newSchedule = new Schedule
            {
                UUID = Guid.NewGuid(),
                StatusId = ScheduleStatus.Status.Active,
                Facility = facility,
                Doctor = doctor,
                DayOfWeek = schedule.DayOfWeek,
                StartTime = schedule.StartTime.StripSeconds(),
                EndTime = schedule.EndTime.StripSeconds(),
                AppointmentDuration = schedule.AppointmentDuration,
                ValidDateFrom = schedule.ValidDateFrom,
                ValidDateTo = schedule.ValidDateTo
            };

            _scheduleRepository.Create(newSchedule);
            await _db.Save();

            _scheduleRepository.CreateEvent(new ScheduleEvent
            {
                Schedule = newSchedule,
                TypeId = ScheduleEventType.Type.Created,
                CallerProfile = callerProfile
            });
            await _db.Save();
        }
    }

    private async Task<bool> AreAppointmentsForSchedule(Schedule schedule)
    {
        var appointments = await _appointmentRepository.GetByScheduleAsync(schedule);
        return appointments.Any();
    }
}
