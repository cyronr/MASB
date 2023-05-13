using MABS.Application.Features.AppointmentFeatures.Command.CancelAppointment;
using MABS.Application.Features.AppointmentFeatures.Common;
using MABS.Application.UnitTests.Mocks;
using MABS.Application.UnitTests.Mocks.Common;
using MABS.Application.UnitTests.Mocks.DataAccess;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;
using MediatR;

namespace MABS.Application.UnitTests.Tests.AppointmentFeatures.Commands;

public class CancelAppointmentCommandHandlerTests
{
    private readonly ILogger<CancelAppointmentCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;
    private readonly Mock<IDbOperation> _mockDbOperation;
    private readonly Mock<ICurrentLoggedProfile> _mockCurrentLoggedProfile;
    private readonly Mock<IMediator> _mediator;

    private readonly CancelAppointmentCommandHandler _handler;

    public CancelAppointmentCommandHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<CancelAppointmentCommandHandler>();
        _mockAppointmentRepository = MockAppointmentRepository.GetAppointmentRepository();
        _mockDbOperation = MockDbOperation.GetDbOperation();
        _mockCurrentLoggedProfile = MockCurrentLoggedProfile.GetAdminProfile();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
        _mediator = MockMediator.GetMediator();

        _handler = new CancelAppointmentCommandHandler(
            _logger,
            _mapper,
            _mockDbOperation.Object,
            _mockCurrentLoggedProfile.Object,
            _mockAppointmentRepository.Object,
            _mediator.Object);
    }

    [Fact]
    public async Task CancelPreparedAppointment()
    {
        var result = await _handler.Handle(new CancelAppointmentCommand(Guid.Parse(Consts.Prepared_Appointment_UUID)), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeOfType<AppointmentDto>();
    }

    [Fact]
    public async Task CancelConfirmedAppointment()
    {
        var result = await _handler.Handle(new CancelAppointmentCommand(Guid.Parse(Consts.Confirmed_Appointment_UUID)), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeOfType<AppointmentDto>();
    }

    [Fact]
    public async Task CancelCancelledAppointment()
    {
        Func<Task> act = async () => { await _handler.Handle(new CancelAppointmentCommand(Guid.Parse(Consts.Cancelled_Appointment_UUID)), CancellationToken.None); };
        await act.Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task CancelNonExistingAppointment()
    {
        Func<Task> act = async () => { await _handler.Handle(new CancelAppointmentCommand(Guid.NewGuid()), CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
