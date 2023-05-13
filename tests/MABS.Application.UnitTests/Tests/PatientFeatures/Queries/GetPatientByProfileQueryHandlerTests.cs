using MABS.Application.Features.PatientFeatures.Common;
using MABS.Application.Features.PatientFeatures.Queries.GetPatientByProfile;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

namespace MABS.Application.UnitTests.Tests.PatientFeatures.Queries;

public class GetPatientByProfileQueryHandlerTests
{
    private readonly ILogger<GetPatientByProfileQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IPatientRepository> _mockPatientRepository;
    private readonly Mock<IProfileRepository> _mockProfileRepository;

    private readonly GetPatientByProfileQueryHandler _handler;

    public GetPatientByProfileQueryHandlerTests()
	{
        _logger = new LoggerFactory().CreateLogger<GetPatientByProfileQueryHandler>();
        _mockPatientRepository = MockPatientRepository.GetPatientRepository();
        _mockProfileRepository = MockProfileRepository.GetProfileRepository();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();

        _handler = new GetPatientByProfileQueryHandler(_logger, _mapper, _mockProfileRepository.Object, _mockPatientRepository.Object);
    }

    [Fact]
    public async Task ShouldReturnPatientDto()
    {
        
        var result = await _handler.Handle(new GetPatientByProfileQuery(Guid.Parse(Consts.Active_Patient_UUID)), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeOfType<PatientDto>();
    }

    [Fact]
    public async Task ShouldThrowNotFoundException()
    {
        Func<Task> act = async () => { await _handler.Handle(new GetPatientByProfileQuery(Guid.NewGuid()), CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
