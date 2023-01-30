using MABS.Application.Services.DoctorServices.Common;
using MABS.Application.Services.DoctorServices.Queries.GetAllDoctors;

namespace MABS.Application.UnitTests.Tests.DoctorServices.Queries;

public class GetAllDoctorsQueryHandlerTests
{
    private readonly ILogger<GetAllDoctorsQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IDoctorRepository> _mockDoctorRepsitory;
    private readonly Mock<IDoctorRepository> _mockEmptyDoctorRepsitory;

    public GetAllDoctorsQueryHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<GetAllDoctorsQueryHandler>();
        _mockDoctorRepsitory = MockDoctorRepository.GetDoctorRepository();
        _mockEmptyDoctorRepsitory = MockDoctorRepository.GetEmptyDoctorRepository();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
    }


    [Fact]
    public async Task CheckReturnType()
    {
        var handler = new GetAllDoctorsQueryHandler(_logger, _mapper, _mockDoctorRepsitory.Object);

        PagingParameters pagingParameters = new PagingParameters { PageNumber = 1, PageSize = 5 };
        var result = await handler.Handle(new GetAllDoctorsQuery(pagingParameters), CancellationToken.None);

        result.Should().BeOfType<PagedList<DoctorDto>>();
    }

    [Fact]
    public async Task CheckCountWhenNotEmpty()
    {
        var handler = new GetAllDoctorsQueryHandler(_logger, _mapper, _mockDoctorRepsitory.Object);

        PagingParameters pagingParameters = new PagingParameters { PageNumber = 1, PageSize = 5 };
        var result = await handler.Handle(new GetAllDoctorsQuery(pagingParameters), CancellationToken.None);

        result.Count.Should().Be(3);
    }

    [Fact]
    public async Task CheckCountWhenEmpty()
    {
        var handler = new GetAllDoctorsQueryHandler(_logger, _mapper, _mockEmptyDoctorRepsitory.Object);

        PagingParameters pagingParameters = new PagingParameters { PageNumber = 1, PageSize = 5 };
        var result = await handler.Handle(new GetAllDoctorsQuery(pagingParameters), CancellationToken.None);

        result.Count.Should().Be(0);
    }
}

