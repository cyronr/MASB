using MABS.Application.Features.FacilityFeatures.Common;
using MABS.Application.Features.FacilityFeatures.Queries.GetFacilityByProfile;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

namespace MABS.Application.UnitTests.Tests.FacilityFeatures.Queries;

public class GetFacilityByProfileQueryHandlerTests
{
    private readonly ILogger<GetFacilityByProfileQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IFacilityRepository> _mockFacilityRepository;
    private readonly Mock<IProfileRepository> _mockProfileRepository;

    public GetFacilityByProfileQueryHandlerTests()
	{
        _logger = new LoggerFactory().CreateLogger<GetFacilityByProfileQueryHandler>();
        _mockFacilityRepository = MockFacilityRepository.GetFacilityRepository();
        _mockProfileRepository = MockProfileRepository.GetProfileRepository();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
    }

    [Fact]
    public async Task ShouldReturnFacilityDto()
    {
        var handler = new GetFacilityByProfileQueryHandler(_logger, _mapper, _mockFacilityRepository.Object, _mockProfileRepository.Object);
        var result = await handler.Handle(new GetFacilityByProfileQuery(Guid.Parse(Consts.Active_Facility_UUID)), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeOfType<FacilityDto>();
    }

    [Fact]
    public async Task ShouldThrowNotFoundException()
    {
        var handler = new GetFacilityByProfileQueryHandler(_logger, _mapper, _mockFacilityRepository.Object, _mockProfileRepository.Object);

        Func<Task> act = async () => { await handler.Handle(new GetFacilityByProfileQuery(Guid.NewGuid()), CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
