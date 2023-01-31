using MABS.Application.Services.DoctorServices.Commands.DeleteDoctor;
using MABS.Application.Services.DoctorServices.Common;

namespace MABS.Application.UnitTests.Tests.DoctorServices.Commands;

public class DeleteDoctorCommandHandlerTests
{
    private readonly ILogger<DeleteDoctorCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IDbOperation> _mockDb;
    private readonly Mock<IDoctorRepository> _mockDoctorRepository;
    private readonly Mock<ICurrentLoggedProfile> _mockCurrentLoggedProfile;

    private readonly DeleteDoctorCommandHandler _handler;

    public DeleteDoctorCommandHandlerTests()
	{
        _logger = new LoggerFactory().CreateLogger<DeleteDoctorCommandHandler>();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
        _mockDb = MockDbOperation.GetDbOperation();
        _mockDoctorRepository = MockDoctorRepository.GetDoctorRepository();
        _mockCurrentLoggedProfile = MockCurrentLoggedProfile.GetAdminProfile();


        _handler = new DeleteDoctorCommandHandler(
                _logger,
                _mapper,
                _mockDb.Object,
                _mockDoctorRepository.Object,
                _mockCurrentLoggedProfile.Object
            );
    }

    [Fact]
    public async Task BasicDelete()
    {
        var result = await _handler.Handle(new DeleteDoctorCommand(Guid.Parse(Consts.Active_Doctor_UUID)), CancellationToken.None);

        result.Should().BeOfType<DoctorDto>();
    }

    [Fact]
    public async Task BasicDelete_ValidateOutput()
    {
        var result = await _handler.Handle(new DeleteDoctorCommand(Guid.Parse(Consts.Active_Doctor_UUID)), CancellationToken.None);

        result.Status.Should().Be(DoctorStatus.Status.Deleted);
    }

    [Fact]
    public async Task NonExistingDoctor()
    {
        Func<Task> act = async () => { await _handler.Handle(new DeleteDoctorCommand(Guid.NewGuid()), CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }
 
}
