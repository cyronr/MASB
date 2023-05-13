using MABS.Application.Common.Pagination;
using MABS.Application.Features.AppointmentFeatures.Common;
using MABS.Application.Features.AppointmentFeatures.Queries.GetByDoctor;
using MABS.Application.Features.ScheduleFeatures.Common;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

namespace MABS.Application.UnitTests.Tests.AppointmentFeatures.Queries;

public class GetByDoctorQueryHandlerTests
{
    private readonly ILogger<GetByDoctorQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;
    private readonly Mock<IDoctorRepository> _mockDoctorRepository;
    private readonly PagingParameters _pagingParameters;

    private readonly GetByDoctorQueryHandler _handler;

    public GetByDoctorQueryHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<GetByDoctorQueryHandler>();
        _mockAppointmentRepository = MockAppointmentRepository.GetAppointmentRepository();
        _mockDoctorRepository = MockDoctorRepository.GetDoctorRepository();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();

        _pagingParameters = new PagingParameters { PageNumber = 1, PageSize = 5 };
        _handler = new GetByDoctorQueryHandler(
            _logger,
            _mapper,
            _mockAppointmentRepository.Object,
            _mockDoctorRepository.Object);
    }

    [Fact]
    public async Task ShouldReturnAppointments()
    {
        var query = new GetByDoctorQuery(Guid.Parse(Consts.Active_Doctor_UUID), _pagingParameters);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeOfType<PagedList<AppointmentDto>>();
    }

    [Fact]
    public async Task NonExistingDoctor()
    {
        var query = new GetByDoctorQuery(Guid.NewGuid(), _pagingParameters);

        Func<Task> act = async () => { await _handler.Handle(query, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
