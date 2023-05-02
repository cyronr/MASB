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

namespace MABS.Application.Features.ScheduleFeatures.Commands.CreateSchedule;

public class CreateScheduleCommandHandler : IRequestHandler<CreateScheduleCommand, ScheduleDto>
{
    private readonly ILogger<CreateScheduleCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IDbOperation _db;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly ICurrentLoggedProfile _currentLoggedProfile;
    private readonly IFacilityRepository _facilityRepository;
    private readonly IDoctorRepository _doctorRepository;

    public CreateScheduleCommandHandler(
        ILogger<CreateScheduleCommandHandler> logger,
        IMapper mapper,
        IDbOperation db,
        IScheduleRepository scheduleRepository,
        ICurrentLoggedProfile currentLoggedProfile,
        IFacilityRepository facilityRepository,
        IDoctorRepository doctorRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _db = db;
        _scheduleRepository = scheduleRepository;
        _currentLoggedProfile = currentLoggedProfile;
        _facilityRepository = facilityRepository;
        _doctorRepository = doctorRepository;
    }


    public async Task<ScheduleDto> Handle(CreateScheduleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Getting current logged profile.");
        var callerProfile = CallerProfile.GetCurrentLoggedProfile(_currentLoggedProfile).GetProfileEntity();

        _logger.LogDebug($"Fetching address with id = {command.AddressId}.");
        var address = await new Address().GetByUUIDAsync(_facilityRepository, command.AddressId);

        _logger.LogDebug($"Fetching doctor with id = {command.DoctorId}.");
        var doctor = await new Doctor().GetByUUIDAsync(_doctorRepository, command.DoctorId);


        var currentSchedules = await _scheduleRepository.GetByDoctorAndAddressAsync(doctor, address);
        var existingSchedule = currentSchedules
                .FirstOrDefault(s =>
                    s.DayOfWeek == command.DayOfWeek &&
                    s.StartTime >= command.StartTime.StripSeconds() &&
                    s.EndTime <= command.EndTime.StripSeconds() &&
                    s.ValidDateFrom >= command.ValidDateFrom &&
                    s.ValidDateTo <= command.ValidDateTo
                );
        if (existingSchedule is not null)
            throw new AlreadyExistsException("Istnieje już harmonogram dla podanego zakresu czasu.");

        Schedule schedule;
        using (var tran = _db.BeginTransaction())
        {
            try
            {
                schedule = new Schedule
                {
                    UUID = Guid.NewGuid(),
                    StatusId = ScheduleStatus.Status.Active,
                    Address = address,
                    Doctor = doctor,
                    DayOfWeek = command.DayOfWeek,
                    StartTime = command.StartTime.StripSeconds(),
                    EndTime = command.EndTime.StripSeconds(),
                    AppointmentDuration = command.AppointmentDuration,
                    ValidDateFrom = command.ValidDateFrom,
                    ValidDateTo = command.ValidDateTo
                };

                await _db.Save();

                _scheduleRepository.CreateEvent(new ScheduleEvent
                {
                    Schedule = schedule,
                    TypeId = ScheduleEventType.Type.Created,
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
}

