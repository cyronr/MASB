using MABS.Application.Features.FacilityFeatures.Commands.UpdateFacilityAddress;
using MABS.Application.Features.FacilityFeatures.Common;
using MABS.Application.UnitTests.Mocks.Common;
using MABS.Application.UnitTests.Mocks.DataAccess;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

namespace MABS.Application.UnitTests.Tests.FacilityFeatures.Commands;

public class UpdateFacilityAddressCommandHandlerTests
{
    private readonly ILogger<UpdateFacilityAddressCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IDbOperation> _mockDbOperation;
    private readonly Mock<IFacilityRepository> _mockFacilityRepository;
    private readonly Mock<ICurrentLoggedProfile> _mockCurrentLoggedProfile;

    private readonly UpdateFacilityAddressCommandHandler _handler;

    public UpdateFacilityAddressCommandHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<UpdateFacilityAddressCommandHandler>();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
        _mockDbOperation = MockDbOperation.GetDbOperation();
        _mockFacilityRepository = MockFacilityRepository.GetFacilityRepository();
        _mockCurrentLoggedProfile = MockCurrentLoggedProfile.GetAdminProfile();

        _handler = new UpdateFacilityAddressCommandHandler(
            _logger,
            _mapper,
            _mockDbOperation.Object,
            _mockFacilityRepository.Object,
            _mockCurrentLoggedProfile.Object);
    }

    [Fact]
    public async Task UpdateAndValidateOutput()
    {
        var command = GetBasicUpdateFacilityAddressCommand();
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeOfType<FacilityDto>();
    }

    [Fact]
    public async Task UpdateNonExistingFacility()
    {
        var command = GetBasicUpdateFacilityAddressCommand();
        command.FacilityId = Guid.NewGuid();

        Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateNonExistingAddress()
    {
        var command = GetBasicUpdateFacilityAddressCommand();
        command.Id = Guid.NewGuid();

        Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }

    private UpdateFacilityAddressCommand GetBasicUpdateFacilityAddressCommand()
    {
        return new UpdateFacilityAddressCommand
        {
            FacilityId = Guid.Parse(Consts.Active_Facility_UUID),
            Id = Guid.Parse(Consts.Active_Address_UUID),
            Name = "Mock Address 1",
            StreetTypeId = 1,
            StreetName = "Mock Street Name",
            HouseNumber = 10,
            FlatNumber = 10,
            CountryId = "MC"
        };
    }
}
