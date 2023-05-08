using MABS.Domain.Models.ScheduleModels;

namespace MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

public static class MockScheduleRepository
{
    public static Mock<IScheduleRepository> GetScheduleRepository()
    {
        var mockSchedules = new List<Schedule>();

        return new Mock<IScheduleRepository>().SetupRepository(mockSchedules);
    }

}

