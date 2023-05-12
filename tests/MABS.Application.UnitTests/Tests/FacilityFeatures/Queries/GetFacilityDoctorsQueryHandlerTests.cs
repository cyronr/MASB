using MABS.Application.Common.Pagination;
using MABS.Application.Features.DoctorFeatures.Common;
using MABS.Application.Features.FacilityFeatures.Common;
using MABS.Application.Features.FacilityFeatures.Queries.GetFacilityDoctors;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

namespace MABS.Application.UnitTests.Tests.FacilityFeatures.Queries;

public class GetFacilityDoctorsQueryHandlerTests
{
    private readonly ILogger<GetFacilityDoctorsQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IFacilityRepository> _mockFacilityRepository;
    private readonly PagingParameters _pagingParams;

    public GetFacilityDoctorsQueryHandlerTests()
	{
        _logger = new LoggerFactory().CreateLogger<GetFacilityDoctorsQueryHandler>();
        _mockFacilityRepository = MockFacilityRepository.GetFacilityRepository();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
        _pagingParams = new PagingParameters { PageNumber = 1, PageSize = 5};
    }

    [Fact]
    public async Task ShouldReturnPagedListOfDoctorDto()
    {
        var handler = new GetFacilityDoctorsQueryHandler(_logger, _mapper, _mockFacilityRepository.Object);
        var result = await handler.Handle(new GetFacilityDoctorsQuery(_pagingParams, Guid.Parse(Consts.Active_Facility_UUID)), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeOfType<PagedList<DoctorDto>>();
    }

    [Fact]
    public async Task ShouldThrowNotFoundException()
    {
        var handler = new GetFacilityDoctorsQueryHandler(_logger, _mapper, _mockFacilityRepository.Object);

        Func<Task> act = async () => { await handler.Handle(new GetFacilityDoctorsQuery(_pagingParams, Guid.NewGuid()), CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
