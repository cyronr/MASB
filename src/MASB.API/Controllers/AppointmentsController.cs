using AutoMapper;
using MABS.Application.Common.Pagination;
using MABS.Application.Features.AppointmentFeatures.Command.CancelAppointment;
using MABS.Application.Features.AppointmentFeatures.Command.ConfirmAppointment;
using MABS.Application.Features.AppointmentFeatures.Command.CreateAppointment;
using MABS.Application.Features.AppointmentFeatures.Queries.GetByAddress;
using MABS.Application.Features.AppointmentFeatures.Queries.GetByDoctor;
using MABS.Application.Features.AppointmentFeatures.Queries.GetByDoctorAndAddress;
using MABS.Application.Features.AppointmentFeatures.Queries.GetByIdQuery;
using MABS.Application.Features.AppointmentFeatures.Queries.GetByPatient;
using MASB.API.Requests.AppointmentRequests;
using MASB.API.Requests.AppointmentResponses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MASB.API.Controllers;

[ApiController]
[Route("api/appointments")]
[Produces("application/json")]
[Consumes("application/json")]
[SwaggerResponse(200, "Sukces. Zwrócono odpowiedź.")]
[SwaggerResponse(204, "Sukces. Brak odpowiedzi.")]
[SwaggerResponse(400, "Błąd. Niepoprawny request.")]
[SwaggerResponse(401, "Błąd. Brak autoryzacji.")]
[SwaggerResponse(403, "Błąd. Zabroniono.")]
[SwaggerResponse(404, "Błąd. Nie znaleziono obiektu.")]
[SwaggerResponse(407, "Błąd. Wystąpił błąd biznesowy.")]
[SwaggerResponse(500, "Nieoczekiwany błąd.")]
public class AppointmentsController : ControllerBase
{
    private readonly ILogger<AppointmentsController> _logger;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AppointmentsController(ILogger<AppointmentsController> logger, IMediator mediator, IMapper mapper)
    {
        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }

    [Authorize]
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Pobierz wizytę na podstawie Id",
        Description = "Zwraca szczegóły wizyty na podstawie podanego Id (UUID). Wymaga autoryzacji."
    )]
    public async Task<ActionResult<AppointmentResponse>> Get(Guid id)
    {
        _logger.LogInformation($"Fetching appointments with id = {id}.");

        var query = new GetByIdQuery(id);
        var response = await _mediator.Send(query);

        _logger.LogInformation($"Fetched appointment with id = {id}.");

        return Ok(_mapper.Map<AppointmentResponse>(response));
    }

    [Authorize]
    [HttpGet("doctor/{doctorId}/address/{addressId}")]
    [SwaggerOperation(
        Summary = "Pobierz listę wizytę dla lekarza i adresu",
        Description = "Zwraca listę wizyt na podstawie Id lekarza oraz Id adresu. " +
            "Dodatkowo obługuje paginację, przyjmując parametry PageNumber i PageSize oraz zwracając szczegóły w nagłówku X-Pagination." +
            "Wymaga autoryzacji."
    )]
    public async Task<ActionResult<List<AppointmentResponse>>> GetByDoctorAndAddress(Guid doctorId, Guid addressId, [FromQuery] PagingParameters pagingParameters)
    {
        _logger.LogInformation($"Fetching appointments for facility = {addressId} and doctor = {doctorId}.");

        var query = new GetByDoctorAndAddressQuery(doctorId, addressId, pagingParameters);
        var response = await _mediator.Send(query);

        _logger.LogInformation($"Fetched {response.Count} appointments.");

        Response.Headers.Add("X-Pagination", response.GetMetadata());
        return Ok(response.Select(s => _mapper.Map<AppointmentResponse>(s)).ToList());
    }

    [Authorize]
    [HttpGet("patient/{id}")]
    [SwaggerOperation(
        Summary = "Pobierz listę wizytę dla pacjenta",
        Description = "Zwraca listę wizyt na podstawie Id pacjenta. " +
            "Dodatkowo obługuje paginację, przyjmując parametry PageNumber i PageSize oraz zwracając szczegóły w nagłówku X-Pagination." +
            "Wymaga autoryzacji."
    )]
    public async Task<ActionResult<List<AppointmentResponse>>> GetByPatient(Guid id, [FromQuery] PagingParameters pagingParameters)
    {
        _logger.LogInformation($"Fetching appointments for patient = {id}.");

        var query = new GetByPatientQuery(id, pagingParameters);
        var response = await _mediator.Send(query);

        _logger.LogInformation($"Fetched {response.Count} appointments.");

        Response.Headers.Add("X-Pagination", response.GetMetadata());
        return Ok(response.Select(s => _mapper.Map<AppointmentResponse>(s)).ToList());
    }

    [Authorize]
    [HttpGet("doctor/{id}")]
    [SwaggerOperation(
        Summary = "Pobierz listę wizytę dla lekarza",
        Description = "Zwraca listę wizyt na podstawie Id lekarza. " +
            "Dodatkowo obługuje paginację, przyjmując parametry PageNumber i PageSize oraz zwracając szczegóły w nagłówku X-Pagination." +
            "Wymaga autoryzacji."
    )]
    public async Task<ActionResult<List<AppointmentResponse>>> GetByDoctor(Guid id, [FromQuery] PagingParameters pagingParameters)
    {
        _logger.LogInformation($"Fetching appointments for doctor = {id}.");

        var query = new GetByDoctorQuery(id, pagingParameters);
        var response = await _mediator.Send(query);

        _logger.LogInformation($"Fetched {response.Count} appointments.");

        Response.Headers.Add("X-Pagination", response.GetMetadata());
        return Ok(response.Select(s => _mapper.Map<AppointmentResponse>(s)).ToList());
    }

    [Authorize]
    [HttpGet("address/{id}")]
    [SwaggerOperation(
        Summary = "Pobierz listę wizytę dla adresu",
        Description = "Zwraca listę wizyt na podstawie Id adresu. " +
            "Dodatkowo obługuje paginację, przyjmując parametry PageNumber i PageSize oraz zwracając szczegóły w nagłówku X-Pagination." +
            "Wymaga autoryzacji."
    )]
    public async Task<ActionResult<List<AppointmentResponse>>> GetByAddress(Guid id, [FromQuery] PagingParameters pagingParameters)
    {
        _logger.LogInformation($"Fetching appointments for address = {id}.");

        var query = new GetByAddressQuery(id, pagingParameters);
        var response = await _mediator.Send(query);

        _logger.LogInformation($"Fetched {response.Count} appointments.");

        Response.Headers.Add("X-Pagination", response.GetMetadata());
        return Ok(response.Select(s => _mapper.Map<AppointmentResponse>(s)).ToList());
    }

    [Authorize]
    [HttpPost]
    [SwaggerOperation(
        Summary = "Stwórz wizytę",
        Description = "Tworzy nową wizytę na podstawie przekazanego w JSON obiektu CreateAppointmentRequest. Wymaga autoryzacji."
    )]
    public async Task<ActionResult<AppointmentResponse>> Create(CreateAppointmentRequest request)
    {
        _logger.LogInformation($"Creating Appointment with data = {request.ToString()}.");

        var command = _mapper.Map<CreateAppointmentCommand>(request);
        var response = await _mediator.Send(command);

        _logger.LogInformation($"Created Appointment with Id = {response.Id}.");

        return Ok(_mapper.Map<AppointmentResponse>(response));
    }

    [Authorize]
    [HttpPost("{id}/confirm")]
    [SwaggerOperation(
        Summary = "Potiwedź wizytę",
        Description = "Potwierdza wizytę na podstawie Id wizyty. Wymaga autoryzacji."
    )]
    public async Task<ActionResult<AppointmentResponse>> Confirm(Guid id, int confirmationCode)
    {
        _logger.LogInformation($"Confirming Appointment with id = {id}.");

        var command = new ConfirmAppointmentCommand(id, confirmationCode);
        var response = await _mediator.Send(command);

        _logger.LogInformation($"Confirmed Appointment with Id = {response.Id}.");

        return Ok(_mapper.Map<AppointmentResponse>(response));
    }

    [Authorize]
    [HttpPost("{id}/cancel")]
    [SwaggerOperation(
        Summary = "Anuluj wizytę",
        Description = "Anuluje wizytę na podstawie Id wizyty. Wymaga autoryzacji."
    )]
    public async Task<ActionResult<AppointmentResponse>> Cancel(Guid id)
    {
        _logger.LogInformation($"Canceling Appointment with id = {id}.");

        var command = new CancelAppointmentCommand(id);
        var response = await _mediator.Send(command);

        _logger.LogInformation($"Cancelled Appointment with Id = {id}.");

        return Ok(_mapper.Map<AppointmentResponse>(response));
    }
}
