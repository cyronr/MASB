using MABS.Application.Features.DoctorFeatures.Commands.UpdateDoctor;
using MABS.Application.Features.DoctorFeatures.Common;

namespace MABS.Application.UnitTests.Tests.DoctorFeatures.Commands;

public class UpdateDoctorCommandHandlerTests
{
    private readonly ILogger<UpdateDoctorCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IDbOperation> _mockDb;
    private readonly Mock<IDoctorRepository> _mockDoctorRepository;
    private readonly Mock<ICurrentLoggedProfile> _mockCurrentLoggedProfile;

    private readonly UpdateDoctorCommandHandler _handler;

    public UpdateDoctorCommandHandlerTests()
	{
        _logger = new LoggerFactory().CreateLogger<UpdateDoctorCommandHandler>();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();
        _mockDb = MockDbOperation.GetDbOperation();
        _mockDoctorRepository = MockDoctorRepository.GetDoctorRepository();
        _mockCurrentLoggedProfile = MockCurrentLoggedProfile.GetAdminProfile();


        _handler = new UpdateDoctorCommandHandler(
                _logger,
                _mapper,
                _mockDb.Object,
                _mockDoctorRepository.Object,
                _mockCurrentLoggedProfile.Object
            );
    }

    [Fact]
    public async Task BasicUpdate()
    {
        var result = await _handler.Handle(GetBasicUpdateDoctorCommand(), CancellationToken.None);

        result.Should().BeOfType<DoctorDto>();
    }

    [Fact]
    public async Task BasicUpdate_ValidateOutput()
    {
        var command = GetBasicUpdateDoctorCommand();
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Firstname.Should().Be(command.Firstname);
        result.Lastname.Should().Be(command.Lastname);
        result.Status.Should().Be(DoctorStatus.Status.Active);
        result.Title.ShortName.Should().Be("MockTitle2");
        result.Specialties[0].Name.Should().Be("Mock Speciality 3");
    }

    [Fact]
    public async Task NonExistingDoctor()
    {
        var command = GetBasicUpdateDoctorCommand();
        command.Id = Guid.NewGuid();

        Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task NonExistingTitleId()
    {
        var command = GetBasicUpdateDoctorCommand();
        command.TitleId = 10;

        Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
        await act.Should()
            .ThrowAsync<DictionaryValueNotExistsException>()
            .WithMessage("Wrong TitleId.");
    }

    [Theory]
    [MemberData(nameof(GetListOfNonExistingSpecialties))]
    public async Task NonExistingSpecialties(List<int> specialtiesIds)
    {
        var command = GetBasicUpdateDoctorCommand();
        command.Specialties.RemoveAll(x => true);
        specialtiesIds.ForEach(s => command.Specialties.Add(s));

        Func<Task> act = async () => { await _handler.Handle(command, CancellationToken.None); };
        await act.Should()
            .ThrowAsync<DictionaryValueNotExistsException>()
            .WithMessage("Wrong Specailties.");
    }

    [Theory]
    [MemberData(nameof(GetListOfExistingSpecialties))]
    public async Task ExistingSpecialties(List<int> specialtiesIds)
    {
        var command = GetBasicUpdateDoctorCommand();
        command.Specialties.RemoveAll(x => true);
        specialtiesIds.ForEach(s => command.Specialties.Add(s));

        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().BeOfType<DoctorDto>();
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

    private UpdateDoctorCommand GetBasicUpdateDoctorCommand()
    {
        return new UpdateDoctorCommand
        {
            Id = Guid.Parse(Consts.Active_Doctor_UUID),
            Firstname = "Test_Firstname_Changed",
            Lastname = "Test_Lastname_Changed",
            TitleId = 2,
            Specialties = new List<int> { 3 }
        };
    }
}
