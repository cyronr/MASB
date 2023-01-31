using MABS.Application.Services.DoctorServices.Queries.GetAllDoctors;

namespace MABS.Application.UnitTests.Tests.Paging;
public class PagingTests
{
    private readonly ILogger<GetAllDoctorsQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IDoctorRepository> _mockDoctorRepsitory;
    private readonly GetAllDoctorsQueryHandler _handler;

    public PagingTests()
    {
        _logger = new LoggerFactory().CreateLogger<GetAllDoctorsQueryHandler>();
        _mockDoctorRepsitory = MockDoctorRepository.GetDoctorRepository();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();

        _handler = new GetAllDoctorsQueryHandler(_logger, _mapper, _mockDoctorRepsitory.Object);
    }

    [Fact]
    public async Task PagedListParams_CheckTotalCount()
    {
        PagingParameters pagingParameters = new PagingParameters { PageNumber = 1, PageSize = 5 };
        var result = await _handler.Handle(new GetAllDoctorsQuery(pagingParameters), CancellationToken.None);

        result.TotalCount.Should().Be(result.Count);
    }

    [Fact]
    public async Task PagedListParams_WhenAllResultsOnOnePage()
    {
        PagingParameters pagingParameters = new PagingParameters { PageNumber = 1, PageSize = 5 };
        var result = await _handler.Handle(new GetAllDoctorsQuery(pagingParameters), CancellationToken.None);

        result.HasNext.Should().BeFalse();
        result.HasPrevious.Should().BeFalse();
        result.TotalPages.Should().Be(1);
    }

    [Fact]
    public async Task PagedListParams_WhenOneResultPerPage_PageOne()
    {
        PagingParameters pagingParameters = new PagingParameters { PageNumber = 1, PageSize = 1 };
        var result = await _handler.Handle(new GetAllDoctorsQuery(pagingParameters), CancellationToken.None);

        result.HasNext.Should().BeTrue();
        result.HasPrevious.Should().BeFalse();
        result.TotalPages.Should().Be(result.TotalCount);
        result.CurrentPage.Should().Be(1);
    }

    [Fact]
    public async Task PagedListParams_WhenOneResultPerPage_PageTwo()
    {
        PagingParameters pagingParameters = new PagingParameters { PageNumber = 2, PageSize = 1 };
        var result = await _handler.Handle(new GetAllDoctorsQuery(pagingParameters), CancellationToken.None);

        result.HasNext.Should().BeTrue();
        result.HasPrevious.Should().BeTrue();
        result.TotalPages.Should().Be(result.TotalCount);
        result.CurrentPage.Should().Be(2);
    }
}
