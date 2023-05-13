using MABS.Application.Common.Pagination;
using MABS.Application.Features.AppointmentFeatures.Common;
using MABS.Application.Features.AppointmentFeatures.Queries.GetByAddress;
using MABS.Application.Features.ScheduleFeatures.Common;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

namespace MABS.Application.UnitTests.Tests.AppointmentFeatures.Queries;

public class GetByAddressQueryHandlerTests
{
    private readonly ILogger<GetByAddressQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;
    private readonly Mock<IFacilityRepository> _mockFacilityRepository;
    private readonly PagingParameters _pagingParameters;

    private readonly GetByAddressQueryHandler _handler;

    public GetByAddressQueryHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<GetByAddressQueryHandler>();
        _mockAppointmentRepository = MockAppointmentRepository.GetAppointmentRepository();
        _mockFacilityRepository = MockFacilityRepository.GetFacilityRepository();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();

        _pagingParameters = new PagingParameters { PageNumber = 1, PageSize = 5 };
        _handler = new GetByAddressQueryHandler(
            _logger,
            _mapper,
            _mockAppointmentRepository.Object,
            _mockFacilityRepository.Object);
    }

    [Fact]
    public async Task ShouldReturnAppointments()
    {
        var query = new GetByAddressQuery(Guid.Parse(Consts.Active_Address_UUID), _pagingParameters);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeOfType<PagedList<AppointmentDto>>();
    }

    [Fact]
    public async Task NonExistingAddress()
    {
        var query = new GetByAddressQuery(Guid.NewGuid(), _pagingParameters);

        Func<Task> act = async () => { await _handler.Handle(query, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
