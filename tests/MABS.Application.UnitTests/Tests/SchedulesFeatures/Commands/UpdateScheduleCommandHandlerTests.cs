using MABS.Application.Features.ScheduleFeatures.Commands.DeleteSchedule;
using MABS.Application.Features.ScheduleFeatures.Commands.UpdateSchedule;
using MABS.Application.Features.ScheduleFeatures.Common;
using MABS.Application.UnitTests.Mocks.Common;
using MABS.Application.UnitTests.Mocks.DataAccess;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

namespace MABS.Application.UnitTests.Tests.SchedulesFeatures.Commands;

public class UpdateScheduleCommandHandlerTests
{
    private readonly ILogger<UpdateScheduleCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IScheduleRepository> _mockScheduleRepository;
    private readonly Mock<IDbOperation> _mockDbOperation;
    private readonly Mock<ICurrentLoggedProfile> _mockCurrentLoggedProfile;
    private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;

    private readonly UpdateScheduleCommandHandler _handler;

    public UpdateScheduleCommandHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<UpdateScheduleCommandHandler>();
        _mockScheduleRepository = MockScheduleRepository.GetScheduleRepository();
        _mockDbOperation = MockDbOperation.GetDbOperation();
        _mockCurrentLoggedProfile = MockCurrentLoggedProfile.GetAdminProfile();
        _mockAppointmentRepository = MockAppointmentRepository.GetAppointmentRepository();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();

        _handler = new UpdateScheduleCommandHandler(
            _logger,
            _mapper,
            _mockDbOperation.Object,
            _mockScheduleRepository.Object,
            _mockCurrentLoggedProfile.Object,
            _mockAppointmentRepository.Object);
    }

    [Fact]
    public async Task UpdateAndValidateOutput()
    {
        var command = GetBasicUpdateScheduleCommand();
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeOfType<ScheduleDto>();
        result.DayOfWeek.Should().Be(command.DayOfWeek);
        result.StartTime.Should().Be(command.StartTime);
        result.EndTime.Should().Be(command.EndTime);
        result.AppointmentDuration.Should().Be(command.AppointmentDuration);
        result.ValidDateFrom.Should().Be(command.ValidDateFrom);
        result.ValidDateTo.Should().Be(command.ValidDateTo);
    }

    [Fact]
    public async Task UpdateScheduleWithAppointments()
    {
        var command = GetBasicUpdateScheduleCommand();
        command.Id = Guid.Parse(Consts.Active_Schedule_UUID);

        Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task UpdateNonExistingSchedule()
    {
        var command = GetBasicUpdateScheduleCommand();
        command.Id = Guid.NewGuid();

        Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }

    private UpdateScheduleCommand GetBasicUpdateScheduleCommand()
    {
        return new UpdateScheduleCommand
        {
            Id = Guid.Parse(Consts.Active_ScheduleWithoutAppointments_UUID),
            DayOfWeek = DayOfWeek.Wednesday,
            StartTime = new TimeOnly(15, 0),
            EndTime = new TimeOnly(18, 0),
            AppointmentDuration = 15,
            ValidDateFrom = new DateOnly(2025, 06, 23),
            ValidDateTo = new DateOnly(2025, 06, 23)
        };
    }
}
