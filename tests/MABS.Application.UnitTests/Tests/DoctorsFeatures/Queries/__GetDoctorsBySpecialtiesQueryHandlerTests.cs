using MABS.Application.Features.DoctorFeatures.Common;

namespace MABS.Application.UnitTests.Tests.DoctorsFeatures.Queries;

public class __GetDoctorsBySpecialtiesQueryHandlerTests
{
    /*
    private readonly ILogger<GetDoctorsBySpecialtiesQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IDoctorRepository> _mockDoctorRepsitory;

    private readonly PagingParameters _pagingParameters = new PagingParameters { PageNumber = 1, PageSize = 5 };

    public GetDoctorsBySpecialtiesQueryHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<GetDoctorsBySpecialtiesQueryHandler>();
        _mockDoctorRepsitory = MockDoctorRepository.GetDoctorRepository();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
    }

    [Fact]
    public async Task CheckReturnType()
    {
        var handler = new GetDoctorsBySpecialtiesQueryHandler(_logger, _mapper, _mockDoctorRepsitory.Object);
        var result = await handler.Handle(new GetDoctorsBySpecialtiesQuery(_pagingParameters, new List<int> { 1, 2, 3, 4, 5 }), CancellationToken.None);

        result.Should().BeOfType<PagedList<DoctorDto>>();
    }

    [Fact]
    public async Task ShouldFindAll()
    {
        var handler = new GetDoctorsBySpecialtiesQueryHandler(_logger, _mapper, _mockDoctorRepsitory.Object);
        var result = await handler.Handle(new GetDoctorsBySpecialtiesQuery(_pagingParameters, new List<int> { 1, 2, 3, 4, 5 }), CancellationToken.None);

        result.Count.Should().Be(3);
    }

    [Fact]
    public async Task ShouldFindOne()
    {
        var handler = new GetDoctorsBySpecialtiesQueryHandler(_logger, _mapper, _mockDoctorRepsitory.Object);
        var result = await handler.Handle(new GetDoctorsBySpecialtiesQuery(_pagingParameters, new List<int> { 1 }), CancellationToken.None);

        result.Count.Should().Be(1);
    }

    [Fact]
    public async Task ShouldFindTwo()
    {
        var handler = new GetDoctorsBySpecialtiesQueryHandler(_logger, _mapper, _mockDoctorRepsitory.Object);
        var result = await handler.Handle(new GetDoctorsBySpecialtiesQuery(_pagingParameters, new List<int> { 3 }), CancellationToken.None);

        result.Count.Should().Be(2);
    }

    [Fact]
    public async Task ShouldFindNone()
    {
        var handler = new GetDoctorsBySpecialtiesQueryHandler(_logger, _mapper, _mockDoctorRepsitory.Object);
        var result = await handler.Handle(new GetDoctorsBySpecialtiesQuery(_pagingParameters, new List<int> { 6 }), CancellationToken.None);

        result.Should().BeEmpty();
    }
    */
}
