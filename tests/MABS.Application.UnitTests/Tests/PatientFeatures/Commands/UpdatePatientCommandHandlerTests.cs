using MABS.Application.Common.Http;
using MABS.Application.Features.PatientFeatures.Commands.UpdatePatient;
using MABS.Application.Features.PatientFeatures.Common;
using MABS.Application.Features.PatientFeatures.Queries.GetPatientByProfile;
using MABS.Application.UnitTests.Mocks.Common;
using MABS.Application.UnitTests.Mocks.DataAccess;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;
using MediatR;

namespace MABS.Application.UnitTests.Tests.PatientFeatures.Commands;

public class UpdatePatientCommandHandlerTests
{
    private readonly ILogger<UpdatePatientCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IDbOperation> _mockDbOperation;
    private readonly Mock<IPatientRepository> _mockPatientRepository;
    private readonly Mock<ICurrentLoggedProfile> _mockCurrentLoggedProfile;

    private readonly UpdatePatientCommandHandler _handler;

    public UpdatePatientCommandHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<UpdatePatientCommandHandler>();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
        _mockDbOperation = MockDbOperation.GetDbOperation();
        _mockPatientRepository = MockPatientRepository.GetPatientRepository();
        _mockCurrentLoggedProfile = MockCurrentLoggedProfile.GetAdminProfile();

        _handler = new UpdatePatientCommandHandler(
            _logger,
            _mapper,
            _mockDbOperation.Object,
            _mockPatientRepository.Object,
            _mockCurrentLoggedProfile.Object);
    }

    [Fact]
    public async Task CreateAndValidateOutput()
    {
        var command = GetBasicUpdatePatientCommand();
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeOfType<PatientDto>();
        result.Firstname.Should().Be(command.Firstname);
        result.Lastname.Should().Be(command.Lastname);
    }

    [Fact]
    public async Task UpdateNonExistingPatient()
    {
        var command = GetBasicUpdatePatientCommand();
        command.Id = Guid.NewGuid();

        Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }

    private UpdatePatientCommand GetBasicUpdatePatientCommand()
    {
        return new UpdatePatientCommand
        {
            Id = Guid.Parse(Consts.Active_Patient_UUID),
            Firstname = "Mock_Fisrstname_CHanged",
            Lastname = "Mock_Lastname_changed"
        };
    }
}
