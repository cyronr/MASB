using MABS.Application.Features.FacilityFeatures.Commands.DeleteFacilityAddress;
using MABS.Application.Features.FacilityFeatures.Common;
using MABS.Application.UnitTests.Mocks.Common;
using MABS.Application.UnitTests.Mocks.DataAccess;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

namespace MABS.Application.UnitTests.Tests.FacilityFeatures.Commands;

public class DeleteFacilityAddressCommandHandlerTests
{
    private readonly ILogger<DeleteFacilityAddressCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IDbOperation> _mockDbOperation;
    private readonly Mock<IFacilityRepository> _mockFacilityRepository;
    private readonly Mock<ICurrentLoggedProfile> _mockCurrentLoggedProfile;
    private readonly Mock<IScheduleRepository> _mockScheduleRepository;

    private readonly DeleteFacilityAddressCommandHandler _handler;

    public DeleteFacilityAddressCommandHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<DeleteFacilityAddressCommandHandler>();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
        _mockDbOperation = MockDbOperation.GetDbOperation();
        _mockFacilityRepository = MockFacilityRepository.GetFacilityRepository();
        _mockCurrentLoggedProfile = MockCurrentLoggedProfile.GetAdminProfile();
        _mockScheduleRepository = MockScheduleRepository.GetScheduleRepository();

        _handler = new DeleteFacilityAddressCommandHandler(
            _logger,
            _mapper,
            _mockDbOperation.Object,
            _mockFacilityRepository.Object,
            _mockCurrentLoggedProfile.Object,
            _mockScheduleRepository.Object);
    }

    [Fact]
    public async Task DeleteAndValidateOutput()
    {
        var command = new DeleteFacilityAddressCommand(Guid.Parse(Consts.Active_Facility_UUID), Guid.Parse(Consts.Active_AddressWithoutSchedules_UUID));
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeOfType<FacilityDto>();
    }

    [Fact]
    public async Task DeleteAddressWithSchedules()
    {
        var command = new DeleteFacilityAddressCommand(Guid.Parse(Consts.Active_Facility_UUID), Guid.Parse(Consts.Active_Address_UUID));

        Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task DeleteNonExistingFacility()
    {
        var command = new DeleteFacilityAddressCommand(Guid.NewGuid(), Guid.Parse(Consts.Active_Address_UUID));

        Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteNonExistingAddress()
    {
        var command = new DeleteFacilityAddressCommand(Guid.Parse(Consts.Active_Facility_UUID), Guid.NewGuid());

        Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
