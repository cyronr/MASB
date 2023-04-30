using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.ScheduleModels;
using MABS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MABS.Infrastructure.DataAccess.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    private readonly ILogger<ScheduleRepository> _logger;
    private readonly DataContext _context;

    public ScheduleRepository(ILogger<ScheduleRepository> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }


    public void Create(Schedule schedule)
    {
        _logger.LogInformation("Saving schedule to databse.");
        _context.Schedules.Add(schedule);
    }

    public void CreateEvent(ScheduleEvent scheduleEvent)
    {
        _logger.LogInformation($"Saving to database event {scheduleEvent.TypeId} for schedule with id = {scheduleEvent.Schedule.Id}.");
        _context.ScheduleEvents.Add(scheduleEvent);
    }

    public async Task<List<Schedule>> GetByDoctorAndFacilityAsync(Doctor doctor, Facility facility)
    {
        return await _context.Schedules
            .Where(s =>
                s.StatusId == ScheduleStatus.Status.Active &&
                s.Doctor == doctor &&
                s.Facility == facility
            )
            .ToListAsync();
    }
}
