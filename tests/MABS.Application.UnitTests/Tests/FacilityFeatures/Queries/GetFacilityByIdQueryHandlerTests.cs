using MABS.Application.Features.FacilityFeatures.Common;
using MABS.Application.Features.FacilityFeatures.Queries.GetFacilityById;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

namespace MABS.Application.UnitTests.Tests.FacilityFeatures.Queries;

public class GetFacilityByIdQueryHandlerTests
{
    private readonly ILogger<GetFacilityByIdQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IFacilityRepository> _mockFacilityRepository;
    public GetFacilityByIdQueryHandlerTests()
	{
        _logger = new LoggerFactory().CreateLogger<GetFacilityByIdQueryHandler>();
        _mockFacilityRepository = MockFacilityRepository.GetFacilityRepository();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
    }

    [Fact]
    public async Task ShouldReturnFacilityDto()
    {
        var handler = new GetFacilityByIdQueryHandler(_logger, _mapper, _mockFacilityRepository.Object);
        var result = await handler.Handle(new GetFacilityByIdQuery(Guid.Parse(Consts.Active_Facility_UUID)), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeOfType<FacilityDto>();
    }

    [Fact]
    public async Task ShouldThrowNotFoundException()
    {
        var handler = new GetFacilityByIdQueryHandler(_logger, _mapper, _mockFacilityRepository.Object);

        Func<Task> act = async () => { await handler.Handle(new GetFacilityByIdQuery(Guid.NewGuid()), CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
