using MABS.Application.Common.Pagination;
using MABS.Application.Features.DoctorFeatures.Common;
using MABS.Application.Features.FacilityFeatures.Commands.AddDoctorToFacility;
using MABS.Application.Features.FacilityFeatures.Common;
using MABS.Application.UnitTests.Mocks.Common;
using MABS.Application.UnitTests.Mocks.DataAccess;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

namespace MABS.Application.UnitTests.Tests.FacilityFeatures.Commands;

public class AddDoctorToFacilityCommandHandlerTests
{
    private readonly ILogger<AddDoctorToFacilityCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IDbOperation> _mockDbOperation;
    private readonly Mock<IFacilityRepository> _mockFacilityRepository;
    private readonly Mock<ICurrentLoggedProfile> _mockCurrentLoggedProfile;
    private readonly Mock<IDoctorRepository> _mockDoctorRepository;
    private readonly PagingParameters _pagingParameters;

    private readonly AddDoctorToFacilityCommandHandler _handler;

    public AddDoctorToFacilityCommandHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<AddDoctorToFacilityCommandHandler>();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
        _mockDbOperation = MockDbOperation.GetDbOperation();
        _mockFacilityRepository = MockFacilityRepository.GetFacilityRepository();
        _mockDoctorRepository = MockDoctorRepository.GetDoctorRepository();
        _mockCurrentLoggedProfile = MockCurrentLoggedProfile.GetAdminProfile();
        _pagingParameters = new PagingParameters { PageNumber = 1, PageSize = 5 };


        _handler = new AddDoctorToFacilityCommandHandler(
            _logger,
            _mapper,
            _mockDbOperation.Object,
            _mockFacilityRepository.Object,
            _mockCurrentLoggedProfile.Object,
            _mockDoctorRepository.Object);
    }

    [Fact]
    public async Task AddAndCheckReturnType()
    {
        var newDoctor = new Doctor
        {
            Id = 100,
            UUID = Guid.NewGuid(),
            StatusId = DoctorStatus.Status.Active,
            Status = new DoctorStatus
            {
                Id = DoctorStatus.Status.Active,
                Name = "Active"
            },
            Firstname = "Mock_Firstname_1",
            Lastname = "Mock_Lastname_1",
            Title = new Title { Id = 1, ShortName = "MockTitle1", Name = "Mock Testing Title 1" },
            Specialties = { new Specialty { Id = 1, Name = "Mock Speciality 1" } }
        };
        _mockDoctorRepository.Object.Create(newDoctor);

        var result = await _handler.Handle(
            new AddDoctorToFacilityCommand(_pagingParameters, Guid.Parse(Consts.Active_Facility_UUID), newDoctor.UUID),
            CancellationToken.None);

        result.Should().BeOfType<PagedList<DoctorDto>>();
    }

    [Fact]
    public async Task AddToNonExistingFacility()
    {
        var newDoctor = new Doctor
        {
            Id = 100,
            UUID = Guid.NewGuid(),
            StatusId = DoctorStatus.Status.Active,
            Status = new DoctorStatus
            {
                Id = DoctorStatus.Status.Active,
                Name = "Active"
            },
            Firstname = "Mock_Firstname_1",
            Lastname = "Mock_Lastname_1",
            Title = new Title { Id = 1, ShortName = "MockTitle1", Name = "Mock Testing Title 1" },
            Specialties = { new Specialty { Id = 1, Name = "Mock Speciality 1" } }
        };
        _mockDoctorRepository.Object.Create(newDoctor);


        Func<Task> act = async () => {
            await _handler.Handle(
                new AddDoctorToFacilityCommand(_pagingParameters, Guid.NewGuid(), newDoctor.UUID),
                CancellationToken.None); 
        };
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AddNonExistingDoctor()
    {
        Func<Task> act = async () => {
            await _handler.Handle(
               new AddDoctorToFacilityCommand(_pagingParameters, Guid.Parse(Consts.Active_Facility_UUID), Guid.NewGuid()),
               CancellationToken.None);
        };
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AddAlreadyAddedDoctor()
    {
        Func<Task> act = async () => {
            await _handler.Handle(
               new AddDoctorToFacilityCommand(_pagingParameters, Guid.Parse(Consts.Active_Facility_UUID), Guid.Parse(Consts.Active_Doctor_UUID)),
               CancellationToken.None);
        };
        await act.Should().ThrowAsync<AlreadyExistsException>();
    }

}
