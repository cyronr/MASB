using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.ScheduleModels;
using MABS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Numerics;

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

    public async Task<List<Schedule>> GetByAddressAsync(Address address)
    {
        return await _context.Schedules
            .Where(s =>
                s.StatusId == ScheduleStatus.Status.Active &&
                s.Address == address
            )
            .ToListAsync();
    }

    public async Task<List<Schedule>> GetByDoctorAndAddressAsync(Doctor doctor, Address address)
    {
        return await _context.Schedules
            .Where(s =>
                s.StatusId == ScheduleStatus.Status.Active &&
                s.Doctor == doctor &&
                s.Address == address
            )
            .ToListAsync();
    }

    public async Task<List<Schedule>> GetByDoctorAsync(Doctor doctor)
    {
        return await _context.Schedules
            .Where(s =>
                s.StatusId == ScheduleStatus.Status.Active &&
                s.Doctor == doctor
            )
            .ToListAsync();
    }

    public async Task<Schedule?> GetByUUIDAsync(Guid uuid)
    {
        return await _context.Schedules
            .Include(p => p.Status)
            .Include(p => p.Events)
            .Include(p => p.Doctor)
            .Include(p => p.Address)
            .FirstOrDefaultAsync(p => p.StatusId == ScheduleStatus.Status.Active && p.UUID == uuid);
    }
}
