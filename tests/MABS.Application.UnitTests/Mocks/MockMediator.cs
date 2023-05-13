using MABS.Application.Features.AppointmentFeatures.Queries.GetByPatient;
using MABS.Application.Features.DoctorFeatures.Common;
using MABS.Application.Features.DoctorFeatures.Queries.GetTimeSlots;
using MediatR;

namespace MABS.Application.UnitTests.Mocks;

public static class MockMediator
{
    public static Mock<IMediator> GetMediator()
    {
        var mockMediator = new Mock<IMediator>();

        mockMediator.Setup(m => m.Send(It.IsAny<GetTimeSlotsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>
            {
                return new List<TimeSlot>
                {
                    new TimeSlot(Guid.Parse(Consts.Active_Schedule_UUID),  new DateOnly(2023, 05, 13),  new TimeOnly(13, 0))
                };
            });

        return mockMediator;
    }
}
