using MABS.Application.Common.Http;
using MABS.Application.Features.PatientFeatures.Commands.CreatePatient;
using MABS.Application.Features.PatientFeatures.Common;
using MABS.Application.UnitTests.Mocks.Common;
using MABS.Application.UnitTests.Mocks.DataAccess;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;
using MediatR;

namespace MABS.Application.UnitTests.Tests.PatientFeatures.Commands;

public class CreatePatientCommandHandlerTests
{
    private readonly ILogger<CreatePatientCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IDbOperation> _mockDbOperation;
    private readonly Mock<IPatientRepository> _mockPatientRepository;
    private readonly Mock<ICurrentLoggedProfile> _mockCurrentLoggedProfile;
    private readonly Mock<IProfileRepository> _mockProfileRepository;
    private readonly Mock<IHttpRequester> _mockHttpRequester;
    private readonly Mock<IMediator> _mediator;

    private readonly CreatePatientCommandHandler _handler;

    public CreatePatientCommandHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<CreatePatientCommandHandler>();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
        _mockDbOperation = MockDbOperation.GetDbOperation();
        _mockPatientRepository = MockPatientRepository.GetPatientRepository();
        _mockCurrentLoggedProfile = MockCurrentLoggedProfile.GetAdminProfile();
        _mockProfileRepository = MockProfileRepository.GetProfileRepository();

        _handler = new CreatePatientCommandHandler(
            _logger,
            _mapper,
            _mockDbOperation.Object,
            _mockPatientRepository.Object,
            _mockCurrentLoggedProfile.Object,
            _mockProfileRepository.Object);
    }

    [Fact]
    public async Task CreateAndValidateOutput()
    {
        var command = GetBasicCreatePatientCommand();
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeOfType<PatientDto>();
        result.Firstname.Should().Be(command.Firstname);
        result.Lastname.Should().Be(command.Lastname);
    }

    private CreatePatientCommand GetBasicCreatePatientCommand()
    {
        return new CreatePatientCommand
        {
            Firstname = "Mock_Fisrstname",
            Lastname = "Mock_Lastname"
        };
    }
}
