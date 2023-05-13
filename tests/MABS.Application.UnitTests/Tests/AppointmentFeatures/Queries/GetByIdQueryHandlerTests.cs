using MABS.Application.Features.AppointmentFeatures.Common;
using MABS.Application.Features.AppointmentFeatures.Queries.GetByIdQuery;
using MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

namespace MABS.Application.UnitTests.Tests.AppointmentFeatures.Queries;

public class GetByIdQueryHandlerTests
{
    private readonly ILogger<GetByIdQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;

    private readonly GetByIdQueryHandler _handler;

    public GetByIdQueryHandlerTests()
    {
        _logger = new LoggerFactory().CreateLogger<GetByIdQueryHandler>();
        _mockAppointmentRepository = MockAppointmentRepository.GetAppointmentRepository();
        _mapper = new MapperConfiguration(
            c => c.AddProfile<AutoMapperProfile>()
        ).CreateMapper();

        _handler = new GetByIdQueryHandler(
            _logger,
            _mapper,
            _mockAppointmentRepository.Object);
    }

    [Fact]
    public async Task ShouldReturnAppointment()
    {
        var query = new GetByIdQuery(Guid.Parse(Consts.Prepared_Appointment_UUID));
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeOfType<AppointmentDto>();
    }

    [Fact]
    public async Task NonExistingAppointment()
    {
        Func<Task> act = async () => { await _handler.Handle(new GetByIdQuery(Guid.NewGuid()), CancellationToken.None); };
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
