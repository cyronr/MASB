using MABS.Application.Features.DoctorFeatures.Common;
using MABS.Application.Features.ScheduleFeatures.Common;
using MABS.Application.Features.ScheduleFeatures.Queries.GetSchedule;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

namespace MABS.Application.UnitTests.Tests.SchedulesFeatures.Queries;

public class GetScheduleQueryHandlerTests
{
    private readonly ILogger<GetScheduleQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IDoctorRepository> _mockDoctorRepsitory;
    private readonly Mock<IScheduleRepository> _mockScheduleRepository;
    private readonly Mock<IFacilityRepository> _mockFacilityRepository;

    private readonly GetScheduleQueryHandler _handler;

    public GetScheduleQueryHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<GetScheduleQueryHandler>();
        _mockDoctorRepsitory = MockDoctorRepository.GetDoctorRepository();
        _mockScheduleRepository = MockScheduleRepository.GetScheduleRepository();
        _mockFacilityRepository = MockFacilityRepository.GetFacilityRepository();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();

        _handler = new GetScheduleQueryHandler(
            _logger,
            _mapper,
            _mockScheduleRepository.Object,
            _mockFacilityRepository.Object,
            _mockDoctorRepsitory.Object);
    }

    [Fact]
    public async Task ShouldReturnSchedules()
    {
        var query = new GetScheduleQuery(Guid.Parse(Consts.Active_Doctor_UUID), Guid.Parse(Consts.Active_Address_UUID));
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeOfType<List<ScheduleDto>>();
    }

    [Fact]
    public async Task NonExistingDoctor()
    {
        var query = new GetScheduleQuery(Guid.NewGuid(), Guid.Parse(Consts.Active_Address_UUID));

        Func<Task> act = async () => { await _handler.Handle(query, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task NonExistingAddress()
    {
        var query = new GetScheduleQuery(Guid.Parse(Consts.Active_Doctor_UUID), Guid.NewGuid());

        Func<Task> act = async () => { await _handler.Handle(query, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
