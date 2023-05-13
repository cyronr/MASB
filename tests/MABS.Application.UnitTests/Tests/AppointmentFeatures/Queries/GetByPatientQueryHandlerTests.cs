using MABS.Application.Common.Pagination;
using MABS.Application.Features.AppointmentFeatures.Common;
using MABS.Application.Features.AppointmentFeatures.Queries.GetByPatient;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

namespace MABS.Application.UnitTests.Tests.AppointmentFeatures.Queries;

public class GetByPatientQueryHandlerTests
{
    private readonly ILogger<GetByPatientQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;
    private readonly Mock<IPatientRepository> _mockPatientRepository;
    private readonly PagingParameters _pagingParameters;

    private readonly GetByPatientQueryHandler _handler;

    public GetByPatientQueryHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<GetByPatientQueryHandler>();
        _mockAppointmentRepository = MockAppointmentRepository.GetAppointmentRepository();
        _mockPatientRepository = MockPatientRepository.GetPatientRepository();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();

        _pagingParameters = new PagingParameters { PageNumber = 1, PageSize = 5 };
        _handler = new GetByPatientQueryHandler(
            _logger,
            _mapper,
            _mockAppointmentRepository.Object,
            _mockPatientRepository.Object);
    }

    [Fact]
    public async Task ShouldReturnAppointments()
    {
        var query = new GetByPatientQuery(Guid.Parse(Consts.Active_Patient_UUID), _pagingParameters);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeOfType<PagedList<AppointmentDto>>();
    }

    [Fact]
    public async Task NonExistingPatient()
    {
        var query = new GetByPatientQuery(Guid.NewGuid(), _pagingParameters);

        Func<Task> act = async () => { await _handler.Handle(query, CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
