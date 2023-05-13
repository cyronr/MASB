using MABS.Application.Features.DoctorFeatures.Common;
using MABS.Application.Features.DoctorFeatures.Queries.GetTimeSlots;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

namespace MABS.Application.UnitTests.Tests.DoctorsFeatures.Queries;

public class GetTimeSlotsQueryHandlerTests
{
    private readonly ILogger<GetTimeSlotsQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IDoctorRepository> _mockDoctorRepsitory;
    private readonly Mock<IScheduleRepository> _mockScheduleRepository;
    private readonly Mock<IFacilityRepository> _mockFacilityRepository;
    private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;

    private readonly GetTimeSlotsQueryHandler _handler;

    public GetTimeSlotsQueryHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<GetTimeSlotsQueryHandler>();
        _mockDoctorRepsitory = MockDoctorRepository.GetDoctorRepository();
        _mockScheduleRepository = MockScheduleRepository.GetScheduleRepository();
        _mockFacilityRepository = MockFacilityRepository.GetFacilityRepository();
        _mockAppointmentRepository = MockAppointmentRepository.GetAppointmentRepository();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();

        _handler = new GetTimeSlotsQueryHandler(
            _logger,
            _mapper,
            _mockScheduleRepository.Object,
            _mockFacilityRepository.Object,
            _mockDoctorRepsitory.Object,
            _mockAppointmentRepository.Object);
    }

    [Fact]
    public async Task ShouldReturnTimeSlots()
    {
        var query = new GetTimeSlotsQuery(Guid.Parse(Consts.Active_Doctor_UUID), Guid.Parse(Consts.Active_Address_UUID));
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeOfType<List<TimeSlot>>();
    }

    [Fact]
    public async Task NonExistingDoctor()
    {
        var query = new GetTimeSlotsQuery(Guid.NewGuid(), Guid.Parse(Consts.Active_Address_UUID));

        Func<Task> act = async () => { await _handler.Handle(query, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task NonExistingAddress()
    {
        var query = new GetTimeSlotsQuery(Guid.Parse(Consts.Active_Doctor_UUID), Guid.NewGuid());

        Func<Task> act = async () => { await _handler.Handle(query, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
