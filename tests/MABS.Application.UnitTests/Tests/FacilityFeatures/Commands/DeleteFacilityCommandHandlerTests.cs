using MABS.Application.Features.DoctorFeatures.Common;
using MABS.Application.Features.FacilityFeatures.Commands.DeleteFacility;
using MABS.Application.Features.FacilityFeatures.Common;
using MABS.Application.UnitTests.Mocks.Common;
using MABS.Application.UnitTests.Mocks.DataAccess;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

namespace MABS.Application.UnitTests.Tests.FacilityFeatures.Commands;

public class DeleteFacilityCommandHandlerTests
{
    private readonly ILogger<DeleteFacilityCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IDbOperation> _mockDb;
    private readonly Mock<IFacilityRepository> _mockFacilityRepository;
    private readonly Mock<ICurrentLoggedProfile> _mockCurrentLoggedProfile;

    private readonly DeleteFacilityCommandHandler _handler;

    public DeleteFacilityCommandHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<DeleteFacilityCommandHandler>();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
        _mockDb = MockDbOperation.GetDbOperation();
        _mockFacilityRepository = MockFacilityRepository.GetFacilityRepository();
        _mockCurrentLoggedProfile = MockCurrentLoggedProfile.GetAdminProfile();


        _handler = new DeleteFacilityCommandHandler(
            _logger,
            _mapper,
            _mockDb.Object,
            _mockFacilityRepository.Object,
            _mockCurrentLoggedProfile.Object);
    }

    [Fact]
    public async Task Delete()
    {
        var result = await _handler.Handle(new DeleteFacilityCommand(Guid.Parse(Consts.Active_Facility_UUID)), CancellationToken.None);

        result.Should().BeOfType<FacilityDto>();
    }

    [Fact]
    public async Task NonExistingDoctor()
    {
        Func<Task> act = async () => { await _handler.Handle(new DeleteFacilityCommand(Guid.NewGuid()), CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }

}
