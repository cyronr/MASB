using AutoMapper;
using Azure;
using MABS.Application.Features.AppointmentFeatures.Command.CancelAppointment;
using MABS.Application.Features.AppointmentFeatures.Command.ConfirmAppointment;
using MABS.Application.Features.AppointmentFeatures.Command.CreateAppointment;
using MABS.Application.Features.AppointmentFeatures.Queries.GetByDoctorAndFacility;
using MABS.Application.Features.AppointmentFeatures.Queries.GetByPatient;
using MABS.Application.Features.ScheduleFeatures.Queries.GetSchedule;
using MASB.API.Requests.AppointmentRequests;
using MASB.API.Requests.AppointmentResponses;
using MASB.API.Responses.ScheduleResponses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

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
    [HttpGet("byDoctorAndFacility")]
    [SwaggerOperation(
        Summary = "Get resource by ID",
        Description = "Retrieve a resource by its ID",
        OperationId = "GetResourceById"
    )]
    public async Task<ActionResult<List<ScheduleResponse>>> GetByDoctorAndFacility(Guid doctorId, Guid facilityId)
    {
        _logger.LogInformation($"Fetching appointments for facility = {facilityId} and doctor = {doctorId}.");

        var query = new GetByDoctorAndFacilityQuery(doctorId, facilityId);
        var response = await _mediator.Send(query);

        _logger.LogInformation($"Fetched {response.Count} appointments.");

        return Ok(response.Select(s => _mapper.Map<ScheduleResponse>(s)).ToList());
    }

    [Authorize]
    [HttpGet("byPatient")]
    public async Task<ActionResult<List<ScheduleResponse>>> GetByPatient(Guid patientId)
    {
        _logger.LogInformation($"Fetching appointments for patient = {patientId}.");

        var query = new GetByPatientQuery(patientId);
        var response = await _mediator.Send(query);

        _logger.LogInformation($"Fetched {response.Count} appointments.");

        return Ok(response.Select(s => _mapper.Map<ScheduleResponse>(s)).ToList());
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
