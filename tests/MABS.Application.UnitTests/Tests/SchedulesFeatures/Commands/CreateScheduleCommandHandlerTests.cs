using MABS.Application.Features.ScheduleFeatures.Commands.CreateSchedule;
using MABS.Application.Features.ScheduleFeatures.Common;
using MABS.Application.UnitTests.Mocks.Common;
using MABS.Application.UnitTests.Mocks.DataAccess;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

namespace MABS.Application.UnitTests.Tests.SchedulesFeatures.Commands;

public class CreateScheduleCommandHandlerTests
{
    private readonly ILogger<CreateScheduleCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IDoctorRepository> _mockDoctorRepsitory;
    private readonly Mock<IScheduleRepository> _mockScheduleRepository;
    private readonly Mock<IFacilityRepository> _mockFacilityRepository;
    private readonly Mock<IDbOperation> _mockDbOperation;
    private readonly Mock<ICurrentLoggedProfile> _mockCurrentLoggedProfile;

    private readonly CreateScheduleCommandHandler _handler;

    public CreateScheduleCommandHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<CreateScheduleCommandHandler>();
        _mockDoctorRepsitory = MockDoctorRepository.GetDoctorRepository();
        _mockScheduleRepository = MockScheduleRepository.GetScheduleRepository();
        _mockFacilityRepository = MockFacilityRepository.GetFacilityRepository();
        _mockDbOperation = MockDbOperation.GetDbOperation();
        _mockCurrentLoggedProfile = MockCurrentLoggedProfile.GetAdminProfile();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();

        _handler = new CreateScheduleCommandHandler(
            _logger,
            _mapper,
            _mockDbOperation.Object,
            _mockScheduleRepository.Object,
            _mockCurrentLoggedProfile.Object,
            _mockFacilityRepository.Object,
            _mockDoctorRepsitory.Object);
    }

    [Fact]
    public async Task CreateAndValidateOutput()
    {
        var command = GetBasicCreateScheduleCommand();
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
    public async Task CreateWithNonExistingDoctor()
    {
        var command = GetBasicCreateScheduleCommand();
        command.DoctorId = Guid.NewGuid();

        Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateExistingSchedule()
    {
        var command = GetBasicCreateScheduleCommand();
        command.AddressId = Guid.Parse(Consts.Active_Address_UUID);

        Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<AlreadyExistsException>();
    }

    [Fact]
    public async Task CreateWithNonExistingAddress()
    {
        var command = GetBasicCreateScheduleCommand();
        command.AddressId = Guid.NewGuid();

        Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }

    private CreateScheduleCommand GetBasicCreateScheduleCommand()
    {
        return new CreateScheduleCommand
        {
            DoctorId = Guid.Parse(Consts.Active_Doctor_UUID),
            AddressId = Guid.Parse(Consts.Active_AddressWithoutSchedules_UUID),
            DayOfWeek = DayOfWeek.Monday,
            StartTime = new TimeOnly(12, 0),
            EndTime = new TimeOnly(15, 0),
            AppointmentDuration = 20,
            ValidDateFrom = new DateOnly(2023, 05, 13),
            ValidDateTo = new DateOnly(2023, 06, 13)
        };
    }
}
