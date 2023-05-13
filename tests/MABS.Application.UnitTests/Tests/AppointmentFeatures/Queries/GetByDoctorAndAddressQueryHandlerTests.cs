using MABS.Application.Common.Pagination;
using MABS.Application.Features.AppointmentFeatures.Common;
using MABS.Application.Features.AppointmentFeatures.Queries.GetByDoctorAndAddress;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

namespace MABS.Application.UnitTests.Tests.AppointmentFeatures.Queries;

public class GetByDoctorAndAddressQueryHandlerTests
{
    private readonly ILogger<GetByDoctorAndAddressQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;
    private readonly Mock<IDoctorRepository> _mockDoctorRepository;
    private readonly Mock<IFacilityRepository> _mockFacilityRepository;
    private readonly PagingParameters _pagingParameters;

    private readonly GetByDoctorAndAddressQueryHandler _handler;

    public GetByDoctorAndAddressQueryHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<GetByDoctorAndAddressQueryHandler>();
        _mockAppointmentRepository = MockAppointmentRepository.GetAppointmentRepository();
        _mockDoctorRepository = MockDoctorRepository.GetDoctorRepository();
        _mockFacilityRepository = MockFacilityRepository.GetFacilityRepository();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();

        _pagingParameters = new PagingParameters { PageNumber = 1, PageSize = 5 };
        _handler = new GetByDoctorAndAddressQueryHandler(
            _logger,
            _mapper,
            _mockAppointmentRepository.Object,
            _mockDoctorRepository.Object,
            _mockFacilityRepository.Object);
    }

    [Fact]
    public async Task ShouldReturnAppointments()
    {
        var query = new GetByDoctorAndAddressQuery(Guid.Parse(Consts.Active_Doctor_UUID), Guid.Parse(Consts.Active_Address_UUID), _pagingParameters);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeOfType<PagedList<AppointmentDto>>();
    }

    [Fact]
    public async Task NonExistingDoctor()
    {
        var query = new GetByDoctorAndAddressQuery(Guid.NewGuid(), Guid.Parse(Consts.Active_Address_UUID), _pagingParameters);

        Func<Task> act = async () => { await _handler.Handle(query, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task NonExistingAddress()
    {
        var query = new GetByDoctorAndAddressQuery(Guid.Parse(Consts.Active_Doctor_UUID), Guid.NewGuid(), _pagingParameters);

        Func<Task> act = async () => { await _handler.Handle(query, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
