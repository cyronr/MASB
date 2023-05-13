using MABS.Application.Features.ScheduleFeatures.Commands.DeleteSchedule;
using MABS.Application.Features.ScheduleFeatures.Common;
using MABS.Application.UnitTests.Mocks.Common;
using MABS.Application.UnitTests.Mocks.DataAccess;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

namespace MABS.Application.UnitTests.Tests.SchedulesFeatures.Commands;

public class DeleteScheduleCommandHandlerTests
{
    private readonly ILogger<DeleteScheduleCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IScheduleRepository> _mockScheduleRepository;
    private readonly Mock<IDbOperation> _mockDbOperation;
    private readonly Mock<ICurrentLoggedProfile> _mockCurrentLoggedProfile;
    private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;

    private readonly DeleteScheduleCommandHandler _handler;

    public DeleteScheduleCommandHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<DeleteScheduleCommandHandler>();
        _mockScheduleRepository = MockScheduleRepository.GetScheduleRepository();
        _mockDbOperation = MockDbOperation.GetDbOperation();
        _mockCurrentLoggedProfile = MockCurrentLoggedProfile.GetAdminProfile();
        _mockAppointmentRepository = MockAppointmentRepository.GetAppointmentRepository();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();

        _handler = new DeleteScheduleCommandHandler(
            _logger,
            _mapper,
            _mockDbOperation.Object,
            _mockScheduleRepository.Object,
            _mockCurrentLoggedProfile.Object,
            _mockAppointmentRepository.Object);
    }

    [Fact]
    public async Task DeleteAndValidateOutput()
    {
        var result = await _handler.Handle(new DeleteScheduleCommand(Guid.Parse(Consts.Active_ScheduleWithoutAppointments_UUID)), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeOfType<ScheduleDto>();
    }

    [Fact]
    public async Task DeleteScheduleWithAppointments()
    {
        Func<Task> act = async () => { await _handler.Handle(new DeleteScheduleCommand(Guid.Parse(Consts.Active_Schedule_UUID)), CancellationToken.None); };
        await act.Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task DeleteNonExistingSchedule()
    {
        Func<Task> act = async () => { await _handler.Handle(new DeleteScheduleCommand(Guid.NewGuid()), CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
