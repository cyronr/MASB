using AutoMapper;
using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.DoctorFeatures.Common;
using MABS.Application.Features.ScheduleFeatures.Common;
using MABS.Application.ModelsExtensions.DoctorModelsExtensions;
using MABS.Application.ModelsExtensions.FacilityModelsExtensions;
using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.ProfileModels;
using MABS.Domain.Models.ScheduleModels;
using MABS.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.ScheduleFeatures.Commands.UpdateSchedule;

public class UpdateScheduleCommandHandler : IRequestHandler<UpdateScheduleCommand, List<ScheduleDto>>
{
    private readonly ILogger<UpdateScheduleCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IDbOperation _db;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IFacilityRepository _facilityRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly ICurrentLoggedProfile _currentLoggedProfile;

    public UpdateScheduleCommandHandler(
        ILogger<UpdateScheduleCommandHandler> logger,
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


    public async Task<List<ScheduleDto>> Handle(UpdateScheduleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Getting current logged profile.");
        var profile = CallerProfile.GetCurrentLoggedProfile(_currentLoggedProfile).GetProfileEntity();

        _logger.LogDebug($"Fetching facility with id = {command.FacilityId}.");
        var facility = await new Facility().GetByUUIDAsync(_facilityRepository, command.FacilityId);

        _logger.LogDebug($"Fetching doctor with id = {command.DoctorId}.");
        var doctor = await new Doctor().GetByUUIDAsync(_doctorRepository, command.DoctorId);

/*      var currentSchedules = await _scheduleRepository.GetByDoctorAndFacilityAsync(doctor, facility);
        var schedulesToCreat = currentSchedules.Select(s => s.)*/

        using (var tran = _db.BeginTransaction())
        {
            try
            {
                foreach(var schedule in command.Schedules)
                {
                    var newSchedule = new Schedule
                    {
                        UUID = Guid.NewGuid(),
                        StatusId = ScheduleStatus.Status.Active,
                        Facility = facility,
                        Doctor = doctor,
                        DayOfWeek = schedule.DayOfWeek,
                        StartTime = schedule.StartTime,
                        EndTime = schedule.EndTime,
                        AppointmentDuration = schedule.AppointmentDuration,
                        ValidDateFrom = schedule.ValidDateFrom.StartOfDay(),
                        ValidDateTo = schedule.ValidDateTo.EndOfDay()
                    };

                    _scheduleRepository.Create(newSchedule);
                    await _db.Save();

                    _scheduleRepository.CreateEvent(new ScheduleEvent
                    {
                        Schedule = newSchedule,
                        TypeId = ScheduleEventType.Type.Created,
                        CallerProfile = profile
                    });
                    await _db.Save();
                }

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

    private async Task ProcessSchedule(ScheduleDetails schedule)
    {

    }
}
