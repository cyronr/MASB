using AutoMapper;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.DoctorFeatures.Common;
using MABS.Application.Features.ScheduleFeatures.Common;
using MABS.Application.ModelsExtensions.DoctorModelsExtensions;
using MABS.Application.ModelsExtensions.FacilityModelsExtensions;
using MABS.Domain.Models.AppointmentModels;
using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.DoctorFeatures.Queries.GetTimeSlots;

public class GetTimeSlotsQueryHandler : IRequestHandler<GetTimeSlotsQuery, List<TimeSlot>>
{
    private readonly ILogger<GetTimeSlotsQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IFacilityRepository _facilityRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    private readonly DateOnly currentDate;
    private readonly TimeOnly currentTime;


    public GetTimeSlotsQueryHandler(
        ILogger<GetTimeSlotsQueryHandler> logger,
        IMapper mapper,
        IScheduleRepository scheduleRepository,
        IFacilityRepository facilityRepository,
        IDoctorRepository doctorRepository,
        IAppointmentRepository appointmentRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _scheduleRepository = scheduleRepository;
        _facilityRepository = facilityRepository;
        _doctorRepository = doctorRepository;
        _appointmentRepository = appointmentRepository;

        currentDate = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
        currentTime = new TimeOnly(DateTime.Now.Hour, DateTime.Now.Minute);
    }

    public async Task<List<TimeSlot>> Handle(GetTimeSlotsQuery query, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Fetching address with id = {query.AddressId}.");
        var address = await new Address().GetByUUIDAsync(_facilityRepository, query.AddressId);

        _logger.LogDebug($"Fetching doctor with id = {query.DoctorId}.");
        var doctor = await new Doctor().GetByUUIDAsync(_doctorRepository, query.DoctorId);

        var schedules = await GetSchedules(doctor, address);
        var timeSlots = GetTimeSlots(schedules);

        SetTimeSlotsAvail(timeSlots, await GetAppointments(doctor, address));

        return timeSlots;
    }

    private async Task<List<ScheduleDto>> GetSchedules(Doctor doctor, Address address)
    {
        _logger.LogDebug($"Fetching schedules for doctor with id = {doctor.UUID} and address with id = {address.UUID}.");
        var schedules = await _scheduleRepository.GetByDoctorAndAddressAsync(doctor, address);

        return schedules.Select(s => _mapper.Map<ScheduleDto>(s)).ToList();
    }

    private async Task<List<Appointment>> GetAppointments(Doctor doctor, Address address)
    {
        _logger.LogDebug($"Fetching appointments for doctor id = {doctor.UUID} and address with id = {address.UUID}.");
        var appointments = await _appointmentRepository.GetByDoctorAndAddressAsync(doctor, address);

        return appointments;
    }

    private List<TimeSlot> GetTimeSlots(List<ScheduleDto> schedules)
    {
        var timeSlots = new List<TimeSlot>();
        foreach (var schedule in schedules)
        {
            var startDate = schedule.ValidDateFrom < currentDate ? currentDate : schedule.ValidDateFrom;
            for (var day = startDate; day <= schedule.ValidDateTo; day = day.AddDays(1))
            {
                if (day.DayOfWeek != schedule.DayOfWeek)
                    continue;

                var startTime = GetStartTime(day, schedule.StartTime, schedule.AppointmentDuration);
                var endTime = schedule.EndTime.AddMinutes(-schedule.AppointmentDuration);
                for (var time = startTime; time <= endTime; time = time.AddMinutes(schedule.AppointmentDuration))
                {
                    timeSlots.Add(new TimeSlot(schedule.Id, day, time));
                }
            }
        }

        return timeSlots;
    }

    private void SetTimeSlotsAvail(List<TimeSlot> timeSlots, List<Appointment> appointments)
    {
        foreach (var timeSlot in timeSlots)
        {
            var isAppointment = appointments.Any(a => 
                a.Schedule.UUID == timeSlot.ScheduleId &&
                a.Date == timeSlot.Date &&
                a.Time == timeSlot.Time
            );

            if (isAppointment)
                timeSlot.Available = false;
        }
    }

    private TimeOnly GetStartTime(DateOnly day, TimeOnly time, int interval)
    {
        if (currentDate != day)
            return time;

        TimeOnly startTime = time;
        while(startTime < currentTime.AddMinutes(60))
        {
            startTime = startTime.AddMinutes(interval);
        }

        return startTime;
    }
}
