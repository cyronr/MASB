using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.ScheduleModels;

namespace MABS.Application.ModelsExtensions.ScheduleModelsExtensions
{
    public static class ScheduleExtensions
    {
        public static async Task<Schedule> GetByUUIDAsync(this Schedule schedule, IScheduleRepository scheduleRepository, Guid uuid)
        {
            schedule = await scheduleRepository.GetByUUIDAsync(uuid);
            if (schedule is null)
                throw new NotFoundException($"Nie odnaleziono harmonogramu o podanym identyfikatorze.", $"ScheduleId = {uuid}");

            return schedule;
        }
    }
}
