using MABS.Application.Features.AppointmentFeatures.Command.CreateAppointment;
using MABS.Application.Features.AppointmentFeatures.Common;
using MABS.Application.UnitTests.Mocks;
using MABS.Application.UnitTests.Mocks.Common;
using MABS.Application.UnitTests.Mocks.DataAccess;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;
using MediatR;

namespace MABS.Application.UnitTests.Tests.AppointmentFeatures.Commands;

public class CreateAppointmentCommandHandlerTests
{
    private readonly ILogger<CreateAppointmentCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IPatientRepository> _mockPatientRepository;
    private readonly Mock<IScheduleRepository> _mockScheduleRepository;
    private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;
    private readonly Mock<IDbOperation> _mockDbOperation;
    private readonly Mock<ICurrentLoggedProfile> _mockCurrentLoggedProfile;
    private readonly Mock<IMediator> _mediator;

    private readonly CreateAppointmentCommandHandler _handler;

    public CreateAppointmentCommandHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<CreateAppointmentCommandHandler>();
        _mockPatientRepository = MockPatientRepository.GetPatientRepository();
        _mockAppointmentRepository = MockAppointmentRepository.GetAppointmentRepository();
        _mockScheduleRepository = MockScheduleRepository.GetScheduleRepository();
        _mockDbOperation = MockDbOperation.GetDbOperation();
        _mockCurrentLoggedProfile = MockCurrentLoggedProfile.GetAdminProfile();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
        _mediator = MockMediator.GetMediator();

        _handler = new CreateAppointmentCommandHandler(
            _logger,
            _mapper,
            _mockDbOperation.Object,
            _mockCurrentLoggedProfile.Object,
            _mockAppointmentRepository.Object,
            _mockPatientRepository.Object,
            _mediator.Object,
            _mockScheduleRepository.Object);
    }

    [Fact]
    public async Task CreateAndValidateOutput()
    {
        var command = GetBasicCreateAppointmentCommand();
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeOfType<AppointmentDto>();
        result.Date.Should().Be(command.Date);
        result.Time.Should().Be(command.Time);
    }

    [Fact]
    public async Task CreateWithNonExistingPatient()
    {
        var command = GetBasicCreateAppointmentCommand();
        command.PatientId = Guid.NewGuid();

        Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateWithNonExistingSchedule()
    {
        var command = GetBasicCreateAppointmentCommand();
        command.ScheduleId = Guid.NewGuid();

        Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }

    private CreateAppointmentCommand GetBasicCreateAppointmentCommand()
    {
        return new CreateAppointmentCommand
        {
            PatientId = Guid.Parse(Consts.Active_Patient_UUID),
            ScheduleId = Guid.Parse(Consts.Active_Schedule_UUID),
            Date = new DateOnly(2023, 05, 13),
            Time = new TimeOnly(13, 0)
        };
    }
}
