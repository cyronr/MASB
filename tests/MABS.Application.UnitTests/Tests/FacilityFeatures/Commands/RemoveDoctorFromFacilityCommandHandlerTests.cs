using MABS.Application.Common.Pagination;
using MABS.Application.Features.DoctorFeatures.Common;
using MABS.Application.Features.FacilityFeatures.Commands.RemoveDoctorFromFacility;
using MABS.Application.UnitTests.Mocks.Common;
using MABS.Application.UnitTests.Mocks.DataAccess;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

namespace MABS.Application.UnitTests.Tests.FacilityFeatures.Commands;

public class RemoveDoctorFromFacilityCommandHandlerTests
{
    private readonly ILogger<RemoveDoctorFromFacilityCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IDbOperation> _mockDbOperation;
    private readonly Mock<IFacilityRepository> _mockFacilityRepository;
    private readonly Mock<ICurrentLoggedProfile> _mockCurrentLoggedProfile;
    private readonly Mock<IDoctorRepository> _mockDoctorRepository;
    private readonly PagingParameters _pagingParameters;

    private readonly RemoveDoctorFromFacilityCommandHandler _handler;

    public RemoveDoctorFromFacilityCommandHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<RemoveDoctorFromFacilityCommandHandler>();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
        _mockDbOperation = MockDbOperation.GetDbOperation();
        _mockFacilityRepository = MockFacilityRepository.GetFacilityRepository();
        _mockDoctorRepository = MockDoctorRepository.GetDoctorRepository();
        _mockCurrentLoggedProfile = MockCurrentLoggedProfile.GetAdminProfile();
        _pagingParameters = new PagingParameters { PageNumber = 1, PageSize = 5 };


        _handler = new RemoveDoctorFromFacilityCommandHandler(
            _logger,
            _mapper,
            _mockDbOperation.Object,
            _mockFacilityRepository.Object,
            _mockCurrentLoggedProfile.Object,
            _mockDoctorRepository.Object);
    }

    [Fact]
    public async Task RemoveAndCheckReturnType()
    {
        var result = await _handler.Handle(
            new RemoveDoctorFromFacilityCommand(_pagingParameters, Guid.Parse(Consts.Active_Facility_UUID), Guid.Parse(Consts.Active_Doctor_UUID)),
            CancellationToken.None);

        result.Should().BeOfType<PagedList<DoctorDto>>();
    }

    [Fact]
    public async Task RemoveFromNonExistingFacility()
    {
        Func<Task> act = async () => {
            await _handler.Handle(
                new RemoveDoctorFromFacilityCommand(_pagingParameters, Guid.NewGuid(), Guid.Parse(Consts.Active_Doctor_UUID)),
                CancellationToken.None); 
        };
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task RemoveNonExistingDoctor()
    {
        Func<Task> act = async () => {
            await _handler.Handle(
               new RemoveDoctorFromFacilityCommand(_pagingParameters, Guid.Parse(Consts.Active_Facility_UUID), Guid.NewGuid()),
               CancellationToken.None);
        };
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
