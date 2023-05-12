using MABS.Application.Common.Http;
using MABS.Application.Features.DoctorFeatures.Common;
using MABS.Application.Features.FacilityFeatures.Commands.CreateFacility;
using MABS.Application.Features.FacilityFeatures.Commands.CreateFacilityAddress;
using MABS.Application.Features.FacilityFeatures.Common;
using MABS.Application.UnitTests.Mocks.Common;
using MABS.Application.UnitTests.Mocks.DataAccess;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;
using MediatR;

namespace MABS.Application.UnitTests.Tests.FacilityFeatures.Commands;

public class CreateFacilityCommandHandlerTests
{
    private readonly ILogger<CreateFacilityCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IDbOperation> _mockDbWithoutActivteTransaction;
    private readonly Mock<IDbOperation> _mockDbWithActivteTransaction;
    private readonly Mock<IFacilityRepository> _mockFacilityRepository;
    private readonly Mock<ICurrentLoggedProfile> _mockCurrentLoggedProfile;
    private readonly Mock<IProfileRepository> _mockProfileRepository;
    private readonly Mock<IHttpRequester> _mockHttpRequester;
    private readonly Mock<IMediator> _mediator;

    private readonly CreateFacilityCommandHandler _handlerWithoutActivteTransaction;
    private readonly CreateFacilityCommandHandler _handlerWithActivteTransaction;

    public CreateFacilityCommandHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<CreateFacilityCommandHandler>();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
        _mockDbWithoutActivteTransaction = MockDbOperation.GetDbOperation();
        _mockDbWithActivteTransaction = MockDbOperation.GetDbOperation(true);
        _mockFacilityRepository = MockFacilityRepository.GetFacilityRepository();
        _mockCurrentLoggedProfile = MockCurrentLoggedProfile.GetAdminProfile();
        _mockProfileRepository = MockProfileRepository.GetProfileRepository();
        _mockHttpRequester = MockHttpRequester.GetHttpRequester();
        _mediator = new Mock<IMediator>();


        _handlerWithoutActivteTransaction = new CreateFacilityCommandHandler(
            _logger,
            _mapper,
            _mockDbWithoutActivteTransaction.Object,
            _mockFacilityRepository.Object,
            _mockCurrentLoggedProfile.Object,
            _mockProfileRepository.Object,
            _mediator.Object,
            _mockHttpRequester.Object);

        _handlerWithActivteTransaction = new CreateFacilityCommandHandler(
            _logger,
            _mapper,
            _mockDbWithActivteTransaction.Object,
            _mockFacilityRepository.Object,
            _mockCurrentLoggedProfile.Object,
            _mockProfileRepository.Object,
            _mediator.Object,
            _mockHttpRequester.Object);
    }

    [Fact]
    public async Task CreateAndCheckReturnType()
    {
        var command = GetBasicCreateFacilityCommand();
        var result = await _handlerWithoutActivteTransaction.Handle(command, CancellationToken.None);

        result.Should().BeOfType<FacilityDto>();
    }

    [Fact]
    public async Task CreateAndValidateOutput()
    {
        var command = GetBasicCreateFacilityCommand();
        var result = await _handlerWithoutActivteTransaction.Handle(command, CancellationToken.None);

        result.ShortName.Should().Be(command.ShortName);
        result.Name.Should().Be(command.Name);
        result.TaxIdentificationNumber.Should().Be(command.TaxIdentificationNumber);
    }

    [Fact]
    public async Task CreateWithExistingTIN()
    {
        var command = GetBasicCreateFacilityCommand();
        command.TaxIdentificationNumber = Consts.Active_Facility_TIN;

        Func<Task> act = async () => { await _handlerWithoutActivteTransaction.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<AlreadyExistsException>();
    }

    private CreateFacilityCommand GetBasicCreateFacilityCommand()
    {
        return new CreateFacilityCommand
        {
            ShortName = "Fac2",
            Name = "Facility 2",
            TaxIdentificationNumber = "987654321",
            Address = new CreateFacilityAddressCommand
            {
                Name = "Mock Address 1",
                StreetTypeId = 1,
                StreetName = "Mock Street Name",
                HouseNumber = 1,
                FlatNumber = 1
            }
        };
    }
}
