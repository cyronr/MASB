using MABS.Application.Features.DoctorFeatures.Common;
using MABS.Application.Features.DoctorFeatures.Queries.GetAllTitles;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

namespace MABS.Application.UnitTests.Tests.DoctorsFeatures.Queries;

public class GetAllTitlesQueryHandlerTests
{
    private readonly ILogger<GetAllTitlesQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IDoctorRepository> _mockDoctorRepsitory;
    private readonly Mock<IDoctorRepository> _mockEmptyDoctorRepsitory;

    public GetAllTitlesQueryHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<GetAllTitlesQueryHandler>();
        _mockDoctorRepsitory = MockDoctorRepository.GetDoctorRepository();
        _mockEmptyDoctorRepsitory = MockDoctorRepository.GetEmptyDoctorRepository();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
    }


    [Fact]
    public async Task CheckReturnType()
    {
        var handler = new GetAllTitlesQueryHandler(_logger, _mapper, _mockDoctorRepsitory.Object);
        var result = await handler.Handle(new GetAllTitlesQuery(), CancellationToken.None);

        result.Should().BeOfType<List<TitleExtendedDto>>();
    }

    [Fact]
    public async Task CheckCountWhenNotEmpty()
    {
        var handler = new GetAllTitlesQueryHandler(_logger, _mapper, _mockDoctorRepsitory.Object);
        var result = await handler.Handle(new GetAllTitlesQuery(), CancellationToken.None);

        result.Count.Should().Be(2);
    }

    [Fact]
    public async Task CheckCountWhenEmpty()
    {
        var handler = new GetAllTitlesQueryHandler(_logger, _mapper, _mockEmptyDoctorRepsitory.Object);
        var result = await handler.Handle(new GetAllTitlesQuery(), CancellationToken.None);

        result.Count.Should().Be(0);
    }
}

