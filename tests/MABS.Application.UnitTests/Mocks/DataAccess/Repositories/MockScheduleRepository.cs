namespace MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

public static class MockScheduleRepository
{
    public static Mock<IScheduleRepository> GetScheduleRepository()
    {
        var mockSchedules = PrepareMockSchedules();

        return new Mock<IScheduleRepository>().SetupRepository(mockSchedules);
    }

    public static List<Schedule> PrepareMockSchedules()
    {
        return new List<Schedule>
        {
            new Schedule
            {
                Id = 1,
                UUID = Guid.Parse(Consts.Active_Schedule_UUID),
                Doctor = MockDoctorRepository.PrepareListOfMockDoctors().Single(d => d.UUID == Guid.Parse(Consts.Active_Doctor_UUID)),
                Address = MockFacilityRepository.PrepareListOfMockAddresses().Single(d => d.UUID == Guid.Parse(Consts.Active_Address_UUID)),
                StatusId = ScheduleStatus.Status.Active,
                Status = new ScheduleStatus
                {
                    Id = ScheduleStatus.Status.Active,
                    Name = "Active"
                },
                DayOfWeek = DayOfWeek.Monday,
                StartTime = new TimeOnly(12, 0),
                EndTime = new TimeOnly(15, 0),
                AppointmentDuration = 20,
                ValidDateFrom = new DateOnly(2023, 05, 13),
                ValidDateTo = new DateOnly(2023, 06, 13)
            },
            new Schedule
            {
                Id = 2,
                UUID = Guid.Parse(Consts.Active_ScheduleWithoutAppointments_UUID),
                Doctor = MockDoctorRepository.PrepareListOfMockDoctors().Single(d => d.UUID == Guid.Parse(Consts.Active_Doctor_UUID)),
                Address = MockFacilityRepository.PrepareListOfMockAddresses().Single(d => d.UUID == Guid.Parse(Consts.Active_Address_UUID)),
                StatusId = ScheduleStatus.Status.Active,
                Status = new ScheduleStatus
                {
                    Id = ScheduleStatus.Status.Active,
                    Name = "Active"
                },
                DayOfWeek = DayOfWeek.Monday,
                StartTime = new TimeOnly(12, 0),
                EndTime = new TimeOnly(15, 0),
                AppointmentDuration = 20,
                ValidDateFrom = new DateOnly(2024, 05, 13),
                ValidDateTo = new DateOnly(2024, 06, 13)
            },
            new Schedule
            {
                Id = 3,
                UUID = Guid.NewGuid(),
                StatusId = ScheduleStatus.Status.Active,
                Status = new ScheduleStatus
                {
                    Id = ScheduleStatus.Status.Active,
                    Name = "Active"
                },
                DayOfWeek = DayOfWeek.Monday,
                StartTime = new TimeOnly(12, 0),
                EndTime = new TimeOnly(15, 0),
                AppointmentDuration = 20,
                ValidDateFrom = new DateOnly(2023, 05, 13),
                ValidDateTo = new DateOnly(2023, 06, 13)
            }
        };
    }

}

