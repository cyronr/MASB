using MABS.Application.Features.DoctorFeatures.Commands.CreateDoctor;
using MABS.Application.Features.DoctorFeatures.Common;

namespace MABS.Application.UnitTests.Tests.DoctorFeatures.Commands;

public class CreateDoctorCommandHandlerTests
{
    private readonly ILogger<CreateDoctorCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IDbOperation> _mockDbWithoutActivteTransaction;
    private readonly Mock<IDbOperation> _mockDbWithActivteTransaction;
    private readonly Mock<IDoctorRepository> _mockDoctorRepository;
    private readonly Mock<ICurrentLoggedProfile> _mockCurrentLoggedProfile;
    private readonly Mock<IProfileRepository> _mockProfileRepository;

    private readonly CreateDoctorCommandHandler _handlerWithoutActivteTransaction;
    private readonly CreateDoctorCommandHandler _handlerWithActivteTransaction;

    public CreateDoctorCommandHandlerTests()
	{
        _logger = new LoggerFactory().CreateLogger<CreateDoctorCommandHandler>();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
        _mockDbWithoutActivteTransaction = MockDbOperation.GetDbOperation();
        _mockDbWithActivteTransaction = MockDbOperation.GetDbOperation(true);
        _mockDoctorRepository = MockDoctorRepository.GetDoctorRepository();
        _mockCurrentLoggedProfile = MockCurrentLoggedProfile.GetAdminProfile();
        _mockProfileRepository = MockProfileRepository.GetProfileRepository();


        _handlerWithoutActivteTransaction = new CreateDoctorCommandHandler(
                _logger,
                _mapper,
                _mockDbWithoutActivteTransaction.Object,
                _mockDoctorRepository.Object,
                _mockCurrentLoggedProfile.Object,
                _mockProfileRepository.Object
            );

        _handlerWithActivteTransaction = new CreateDoctorCommandHandler(
                _logger,
                _mapper,
                _mockDbWithActivteTransaction.Object,
                _mockDoctorRepository.Object,
                _mockCurrentLoggedProfile.Object,
                _mockProfileRepository.Object
            );
    }

    [Fact]
    public async Task BasicCreate()
    {
        var result = await _handlerWithoutActivteTransaction.Handle(GetBasicCreateDoctorCommand(), CancellationToken.None);

        result.Should().BeOfType<DoctorDto>();
    }

    [Fact]
    public async Task BasicCreate_ValidateOutput()
    {
        var command = GetBasicCreateDoctorCommand();
        var result = await _handlerWithoutActivteTransaction.Handle(command, CancellationToken.None);

        result.Firstname.Should().Be(command.Firstname);
        result.Lastname.Should().Be(command.Lastname);
        result.Status.Should().Be(DoctorStatus.Status.Active);
        result.Title.ShortName.Should().Be("MockTitle1");
        result.Specialties[0].Name.Should().Be("Mock Speciality 1");
    }

    [Fact]
    public async Task NonExistingTitleId()
    {
        var command = GetBasicCreateDoctorCommand();
        command.TitleId = 10;

        Func<Task> act = async () => { await _handlerWithoutActivteTransaction.Handle(command, CancellationToken.None); };
        await act.Should()
            .ThrowAsync<DictionaryValueNotExistsException>()
            .WithMessage("Wrong TitleId.");
    }

    [Theory]
    [MemberData(nameof(GetListOfNonExistingSpecialties))]
    public async Task NonExistingSpecialties(List<int> specialtiesIds)
    {
        var command = GetBasicCreateDoctorCommand();
        command.Specialties.RemoveAll(x => true);
        specialtiesIds.ForEach(s => command.Specialties.Add(s));

        Func<Task> act = async () => { await _handlerWithoutActivteTransaction.Handle(command, CancellationToken.None); };
        await act.Should()
            .ThrowAsync<DictionaryValueNotExistsException>()
            .WithMessage("Wrong Specailties.");
    }

    [Theory]
    [MemberData(nameof(GetListOfExistingSpecialties))]
    public async Task ExistingSpecialties(List<int> specialtiesIds)
    {
        var command = GetBasicCreateDoctorCommand();
        command.Specialties.RemoveAll(x => true);
        specialtiesIds.ForEach(s => command.Specialties.Add(s));

        var result = await _handlerWithoutActivteTransaction.Handle(command, CancellationToken.None);
        result.Should().BeOfType<DoctorDto>();
    }

    [Fact]
    public async Task CreateWithNonExistingProfileIdPassed()
    {
        var command = GetBasicCreateDoctorCommand();
        command.ProfileId = Guid.NewGuid();

        Func<Task> act = async () => { await _handlerWithActivteTransaction.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateWithExistingProfileIdPassed()
    {
        var command = GetBasicCreateDoctorCommand();
        command.ProfileId = Guid.Parse("271BC4CD-B541-40F6-B711-F274564ECA2F");

        var result = await _handlerWithActivteTransaction.Handle(command, CancellationToken.None);
        result.Should().BeOfType<DoctorDto>();
    }

    private CreateDoctorCommand GetBasicCreateDoctorCommand()
    {
        return new CreateDoctorCommand
        {
            Firstname = "Test_Firstname",
            Lastname = "Test_Lastname",
            TitleId = 1,
            Specialties = new List<int> { 1 }
        };
    }
    public static IEnumerable<object[]> GetListOfNonExistingSpecialties()
    {
        yield return new object[] { new List<int> { 10 } };
        yield return new object[] { new List<int> { 10, 15 } };
        yield return new object[] { new List<int> { 1, 10 } };
    }

    public static IEnumerable<object[]> GetListOfExistingSpecialties()
    {
        yield return new object[] { new List<int> { 1 } };
        yield return new object[] { new List<int> { 1, 5 } };
        yield return new object[] { new List<int> { 1, 2, 4 } };
    }
}
