using MABS.Application.Common.Http;
using MABS.Application.Features.FacilityFeatures.Commands.UpdateFacility;
using MABS.Application.Features.FacilityFeatures.Commands.UpdateFacilityAddress;
using MABS.Application.Features.FacilityFeatures.Common;
using MABS.Application.UnitTests.Mocks.Common;
using MABS.Application.UnitTests.Mocks.DataAccess;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;
using MediatR;

namespace MABS.Application.UnitTests.Tests.FacilityFeatures.Commands;

public class UpdateFacilityCommandHandlerTests
{
    private readonly ILogger<UpdateFacilityCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IDbOperation> _mockDbWithoutActivteTransaction;
    private readonly Mock<IDbOperation> _mockDbWithActivteTransaction;
    private readonly Mock<IFacilityRepository> _mockFacilityRepository;
    private readonly Mock<ICurrentLoggedProfile> _mockCurrentLoggedProfile;

    private readonly UpdateFacilityCommandHandler _handlerWithoutActivteTransaction;

    public UpdateFacilityCommandHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<UpdateFacilityCommandHandler>();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
        _mockDbWithoutActivteTransaction = MockDbOperation.GetDbOperation();
        _mockDbWithActivteTransaction = MockDbOperation.GetDbOperation(true);
        _mockFacilityRepository = MockFacilityRepository.GetFacilityRepository();
        _mockCurrentLoggedProfile = MockCurrentLoggedProfile.GetAdminProfile();


        _handlerWithoutActivteTransaction = new UpdateFacilityCommandHandler(
            _logger,
            _mapper,
            _mockDbWithoutActivteTransaction.Object,
            _mockFacilityRepository.Object,
            _mockCurrentLoggedProfile.Object);
    }

    [Fact]
    public async Task CreateAndCheckReturnType()
    {
        var command = GetBasicUpdateFacilityCommand();
        var result = await _handlerWithoutActivteTransaction.Handle(command, CancellationToken.None);

        result.Should().BeOfType<FacilityDto>();
    }

    [Fact]
    public async Task UpdateAndValidateOutput()
    {
        var command = GetBasicUpdateFacilityCommand();
        var result = await _handlerWithoutActivteTransaction.Handle(command, CancellationToken.None);

        result.ShortName.Should().Be(command.ShortName);
        result.Name.Should().Be(command.Name);
    }

    [Fact]
    public async Task UpdateNonExistingFacility()
    {
        var command = GetBasicUpdateFacilityCommand();
        command.Id = Guid.NewGuid();

        Func<Task> act = async () => { await _handlerWithoutActivteTransaction.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }

    private UpdateFacilityCommand GetBasicUpdateFacilityCommand()
    {
        return new UpdateFacilityCommand
        {
            Id = Guid.Parse(Consts.Active_Facility_UUID),
            ShortName = "Fac2",
            Name = "Facility 2"
        };
    }
}
