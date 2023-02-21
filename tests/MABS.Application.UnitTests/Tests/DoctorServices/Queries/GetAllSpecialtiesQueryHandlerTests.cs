using MABS.Application.Features.DoctorFeatures.Common;
using MABS.Application.Features.DoctorFeatures.Queries.GetAllSpecialties;

namespace MABS.Application.UnitTests.Tests.DoctorFeatures.Queries;

public class GetAllSpecialtiesQueryHandlerTests
{
    private readonly ILogger<GetAllSpecialtiesQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IDoctorRepository> _mockDoctorRepsitory;
    private readonly Mock<IDoctorRepository> _mockEmptyDoctorRepsitory;

    public GetAllSpecialtiesQueryHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<GetAllSpecialtiesQueryHandler>();
        _mockDoctorRepsitory = MockDoctorRepository.GetDoctorRepository();
        _mockEmptyDoctorRepsitory = MockDoctorRepository.GetEmptyDoctorRepository();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
    }


    [Fact]
    public async Task CheckReturnType()
    {
        var handler = new GetAllSpecialtiesQueryHandler(_logger, _mapper, _mockDoctorRepsitory.Object);
        var result = await handler.Handle(new GetAllSpecialtiesQuery(), CancellationToken.None);

        result.Should().BeOfType<List<SpecialityExtendedDto>>();
    }

    [Fact]
    public async Task CheckCountWhenNotEmpty()
    {
        var handler = new GetAllSpecialtiesQueryHandler(_logger, _mapper, _mockDoctorRepsitory.Object);
        var result = await handler.Handle(new GetAllSpecialtiesQuery(), CancellationToken.None);

        result.Count.Should().Be(5);
    }

    [Fact]
    public async Task CheckCountWhenEmpty()
    {
        var handler = new GetAllSpecialtiesQueryHandler(_logger, _mapper, _mockEmptyDoctorRepsitory.Object);
        var result = await handler.Handle(new GetAllSpecialtiesQuery(), CancellationToken.None);

        result.Count.Should().Be(0);
    }
}

