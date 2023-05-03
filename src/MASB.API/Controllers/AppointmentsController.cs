using AutoMapper;
using MABS.Application.Features.AppointmentFeatures.Command.CancelAppointment;
using MABS.Application.Features.AppointmentFeatures.Command.ConfirmAppointment;
using MABS.Application.Features.AppointmentFeatures.Command.CreateAppointment;
using MABS.Application.Features.AppointmentFeatures.Queries.GetByDoctorAndAddress;
using MABS.Application.Features.AppointmentFeatures.Queries.GetByIdQuery;
using MABS.Application.Features.AppointmentFeatures.Queries.GetByPatient;
using MASB.API.Requests.AppointmentRequests;
using MASB.API.Requests.AppointmentResponses;
using MASB.API.Responses.ScheduleResponses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MASB.API.Controllers;

[ApiController]
[Route("api/appointments")]
[Produces("application/json")]
[Consumes("application/json")]
[SwaggerResponse(200, "The resource was found")]
[SwaggerResponse(404, "The resource was not found")]
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
    public async Task<ActionResult<AppointmentResponse>> Get(Guid id)
    {
        _logger.LogInformation($"Fetching appointments with id = {id}.");

        var query = new GetByIdQuery(id);
        var response = await _mediator.Send(query);

        _logger.LogInformation($"Fetched appointment with id = {id}.");

        return Ok(_mapper.Map<AppointmentResponse>(response));
    }

    [Authorize]
    [HttpGet("byDoctorAnAddress")]
    [SwaggerOperation(
        Summary = "Get resource by ID",
        Description = "Retrieve a resource by its ID",
        OperationId = "GetResourceById"
    )]
    public async Task<ActionResult<List<AppointmentResponse>>> GetByDoctorAndAddress(Guid doctorId, Guid addressId)
    {
        _logger.LogInformation($"Fetching appointments for facility = {addressId} and doctor = {doctorId}.");

        var query = new GetByDoctorAndAddressQuery(doctorId, addressId);
        var response = await _mediator.Send(query);

        _logger.LogInformation($"Fetched {response.Count} appointments.");

        return Ok(response.Select(s => _mapper.Map<AppointmentResponse>(s)).ToList());
    }

    [Authorize]
    [HttpGet("byPatient")]
    public async Task<ActionResult<List<AppointmentResponse>>> GetByPatient(Guid patientId)
    {
        _logger.LogInformation($"Fetching appointments for patient = {patientId}.");

        var query = new GetByPatientQuery(patientId);
        var response = await _mediator.Send(query);

        _logger.LogInformation($"Fetched {response.Count} appointments.");

        return Ok(response.Select(s => _mapper.Map<AppointmentResponse>(s)).ToList());
    }

    [Authorize]
    [HttpPost]
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
    public async Task<ActionResult<AppointmentResponse>> Cancel(Guid id)
    {
        _logger.LogInformation($"Canceling Appointment with id = {id}.");

        var command = new CancelAppointmentCommand(id);
        await _mediator.Send(command);

        _logger.LogInformation($"Cancelled Appointment with Id = {id}.");

        return NoContent();
    }
}
