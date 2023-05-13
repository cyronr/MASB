using MABS.Domain.Models.AppointmentModels;

namespace MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

public static class MockAppointmentRepository
{
    public static Mock<IAppointmentRepository> GetAppointmentRepository()
    {
        var mockAppointments = PrepareListOfMockAppointments();

        return new Mock<IAppointmentRepository>().SetupRepository(mockAppointments);
    }

    public static List<Appointment> PrepareListOfMockAppointments()
    {
        return new List<Appointment>
        {
            new Appointment
            {
                Id = 1,
                UUID = Guid.Parse(Consts.Prepared_Appointment_UUID),
                StatusId = AppointmentStatus.Status.Prepared,
                Status = new AppointmentStatus
                {
                    Id =  AppointmentStatus.Status.Prepared,
                    Name = "Prepared"
                },
                Patient = MockPatientRepository.PrepareListOfMockPatients().Single(p => p.UUID == Guid.Parse(Consts.Active_Patient_UUID)),
                Schedule = MockScheduleRepository.PrepareMockSchedules().Single(p => p.UUID == Guid.Parse(Consts.Active_Schedule_UUID)),
                Date = new DateOnly(2023, 05, 13),
                Time = new TimeOnly(13, 0),
                ConfirmationCode = Consts.Prepared_Appointment_ConfirmationCode
            },
            new Appointment
            {
                Id = 2,
                UUID = Guid.Parse(Consts.Cancelled_Appointment_UUID),
                StatusId = AppointmentStatus.Status.Cancelled,
                Status = new AppointmentStatus
                {
                    Id =  AppointmentStatus.Status.Cancelled,
                    Name = "Cancelled"
                },
                Patient = MockPatientRepository.PrepareListOfMockPatients().Single(p => p.UUID == Guid.Parse(Consts.Active_Patient_UUID)),
                Schedule = MockScheduleRepository.PrepareMockSchedules().Single(p => p.UUID == Guid.Parse(Consts.Active_Schedule_UUID)),
                Date = new DateOnly(2023, 05, 13),
                Time = new TimeOnly(13, 0),
                ConfirmationCode = 1234
            },
            new Appointment
            {
                Id = 3,
                UUID = Guid.Parse(Consts.Confirmed_Appointment_UUID),
                StatusId = AppointmentStatus.Status.Confirmed,
                Status = new AppointmentStatus
                {
                    Id =  AppointmentStatus.Status.Confirmed,
                    Name = "Confirmed"
                },
                Patient = MockPatientRepository.PrepareListOfMockPatients().Single(p => p.UUID == Guid.Parse(Consts.Active_Patient_UUID)),
                Schedule = MockScheduleRepository.PrepareMockSchedules().Single(p => p.UUID == Guid.Parse(Consts.Active_Schedule_UUID)),
                Date = new DateOnly(2023, 05, 13),
                Time = new TimeOnly(13, 0),
                ConfirmationCode = 4321
            }
        };
    }
}
