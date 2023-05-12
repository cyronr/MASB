﻿using MABS.Application.Common.Geolocation;
using MABS.Application.Common.Http;
using MABS.Application.Features.FacilityFeatures.Commands.CreateFacilityAddress;
using MABS.Application.Features.FacilityFeatures.Common;
using MABS.Application.UnitTests.Mocks.Common;
using MABS.Application.UnitTests.Mocks.DataAccess;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;
using MediatR;

namespace MABS.Application.UnitTests.Tests.FacilityFeatures.Commands;

public class CreateFacilityAddressCommandHandlerTests
{
    private readonly ILogger<CreateFacilityAddressCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IDbOperation> _mockDbOperation;
    private readonly Mock<IFacilityRepository> _mockFacilityRepository;
    private readonly Mock<IProfileRepository> _mockProfileRepository;
    private readonly Mock<ICurrentLoggedProfile> _mockCurrentLoggedProfile;
    private readonly Mock<IGeolocator> _mockGeolocator;

    private readonly CreateFacilityAddressCommandHandler _handler;

    public CreateFacilityAddressCommandHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<CreateFacilityAddressCommandHandler>();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
        _mockDbOperation = MockDbOperation.GetDbOperation();
        _mockFacilityRepository = MockFacilityRepository.GetFacilityRepository();
        _mockProfileRepository = MockProfileRepository.GetProfileRepository();
        _mockCurrentLoggedProfile = MockCurrentLoggedProfile.GetAdminProfile();
        _mockGeolocator = MockGeolocator.GetGeolocator();


        _handler = new CreateFacilityAddressCommandHandler(
            _logger,
            _mapper,
            _mockDbOperation.Object,
            _mockFacilityRepository.Object,
            _mockCurrentLoggedProfile.Object,
            _mockProfileRepository.Object,
            _mockGeolocator.Object);
    }

    [Fact]
    public async Task UpdateAndValidateOutput()
    {
        var command = GetBasicCreateFacilityAddressCommand();
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeOfType<FacilityDto>();
    }

    [Fact]
    public async Task UpdateNonExistingFacility()
    {
        var command = GetBasicCreateFacilityAddressCommand();
        command.FacilityId = Guid.NewGuid();

        Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }

    private CreateFacilityAddressCommand GetBasicCreateFacilityAddressCommand()
    {
        return new CreateFacilityAddressCommand
        {
            FacilityId = Guid.Parse(Consts.Active_Facility_UUID),
            Name = "Mock Address 1",
            StreetTypeId = 1,
            StreetName = "Mock Street Name",
            HouseNumber = 10,
            FlatNumber = 10,
            CountryId = "MC"
        };
    }
}
