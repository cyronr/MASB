using MABS.Application.Features.DoctorFeatures.Common;
using MABS.Application.Features.DoctorFeatures.Queries.GetDoctorById;

namespace MABS.Application.UnitTests.Tests.DoctorFeatures.Queries;

public class GetDoctorByIdQueryHandlerTests
{
    private readonly ILogger<GetDoctorByIdQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IDoctorRepository> _mockDoctorRepsitory;

    public GetDoctorByIdQueryHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<GetDoctorByIdQueryHandler>();
        _mockDoctorRepsitory = MockDoctorRepository.GetDoctorRepository();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
    }

    [Fact]
    public async Task CheckReturnType()
    {
        var handler = new GetDoctorByIdQueryHandler(_logger, _mapper, _mockDoctorRepsitory.Object);
        var result = await handler.Handle(new GetDoctorByIdQuery(Guid.Parse(Consts.Active_Doctor_UUID)), CancellationToken.None);

        result.Should().BeOfType<DoctorDto>();
    }

    [Fact]
    public async Task ShouldFind()
    {
        var handler = new GetDoctorByIdQueryHandler(_logger, _mapper, _mockDoctorRepsitory.Object);
        var result = await handler.Handle(new GetDoctorByIdQuery(Guid.Parse(Consts.Active_Doctor_UUID)), CancellationToken.None);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldThrowNotFoundException()
    {
        var handler = new GetDoctorByIdQueryHandler(_logger, _mapper, _mockDoctorRepsitory.Object);

        Func<Task> act = async () => { await handler.Handle(new GetDoctorByIdQuery(Guid.NewGuid()), CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
