using MABS.Domain.Models.AppointmentModels;

namespace MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

public static class MockAppointmentRepository
{
    public static Mock<IAppointmentRepository> GetAppointmentRepository()
    {
        var mockAppointments = new List<Appointment>();

        return new Mock<IAppointmentRepository>().SetupRepository(mockAppointments);
    }
}
