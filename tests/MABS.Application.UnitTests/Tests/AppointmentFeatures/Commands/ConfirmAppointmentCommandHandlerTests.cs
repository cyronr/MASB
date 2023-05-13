using MABS.Application.Features.AppointmentFeatures.Command.ConfirmAppointment;
using MABS.Application.Features.AppointmentFeatures.Common;
using MABS.Application.UnitTests.Mocks;
using MABS.Application.UnitTests.Mocks.Common;
using MABS.Application.UnitTests.Mocks.DataAccess;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;
using MediatR;

namespace MABS.Application.UnitTests.Tests.AppointmentFeatures.Commands;

public class ConfirmAppointmentCommandHandlerTests
{
    private readonly ILogger<ConfirmAppointmentCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;
    private readonly Mock<IDbOperation> _mockDbOperation;
    private readonly Mock<ICurrentLoggedProfile> _mockCurrentLoggedProfile;
    private readonly Mock<IMediator> _mediator;

    private readonly ConfirmAppointmentCommandHandler _handler;

    public ConfirmAppointmentCommandHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<ConfirmAppointmentCommandHandler>();
        _mockAppointmentRepository = MockAppointmentRepository.GetAppointmentRepository();
        _mockDbOperation = MockDbOperation.GetDbOperation();
        _mockCurrentLoggedProfile = MockCurrentLoggedProfile.GetAdminProfile();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
        _mediator = MockMediator.GetMediator();

        _handler = new ConfirmAppointmentCommandHandler(
            _logger,
            _mapper,
            _mockDbOperation.Object,
            _mockCurrentLoggedProfile.Object,
            _mockAppointmentRepository.Object,
            _mediator.Object);
    }

    [Fact]
    public async Task ConfirmPreparedAppointment()
    {
        var command = new ConfirmAppointmentCommand(Guid.Parse(Consts.Prepared_Appointment_UUID), Consts.Prepared_Appointment_ConfirmationCode);
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeOfType<AppointmentDto>();
    }

    [Fact]
    public async Task ConfirmPreparedAppointmentWithWrongCode()
    {
        var command = new ConfirmAppointmentCommand(Guid.Parse(Consts.Prepared_Appointment_UUID), -9999);

        Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task ConfirmConfirmedAppointment()
    {
        var command = new ConfirmAppointmentCommand(Guid.Parse(Consts.Confirmed_Appointment_UUID), Consts.Prepared_Appointment_ConfirmationCode);

        Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task ConfirmCancelledAppointment()
    {
        var command = new ConfirmAppointmentCommand(Guid.Parse(Consts.Cancelled_Appointment_UUID), Consts.Prepared_Appointment_ConfirmationCode);

        Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task ConfirmNonExistingAppointment()
    {
        var command = new ConfirmAppointmentCommand(Guid.NewGuid(), 0);

        Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
