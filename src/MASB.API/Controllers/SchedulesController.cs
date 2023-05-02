using AutoMapper;
using MABS.API.Requests.ScheduleRequests;
using MABS.Application.Features.ScheduleFeatures.Commands.CreateSchedule;
using MABS.Application.Features.ScheduleFeatures.Commands.DeleteSchedule;
using MABS.Application.Features.ScheduleFeatures.Commands.UpdateSchedule;
using MABS.Application.Features.ScheduleFeatures.Queries.GetSchedule;
using MASB.API.Responses.ScheduleResponses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MASB.API.Controllers;

[ApiController]
[Route("api/schedules")]
public class SchedulesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<SchedulesController> _logger;
    private readonly IMapper _mapper;

    public SchedulesController(IMediator mediator, ILogger<SchedulesController> logger, IMapper mapper)
    {
        _mediator = mediator;
        _logger = logger;
        _mapper = mapper;
    }


    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<ScheduleResponse>>> GetSchedules(Guid facilityId, Guid doctorId)
    {
        _logger.LogInformation($"Fetching schedules for facility = {facilityId} and doctor = {doctorId}.");

        var query = new GetScheduleQuery(doctorId, facilityId);
        var response = await _mediator.Send(query);

        _logger.LogInformation($"Fetched {response.Count} schedules.");

        return Ok(response.Select(s => _mapper.Map<ScheduleResponse>(s)).ToList());
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ScheduleResponse>> Create(CreateScheduleRequest request)
    {
        _logger.LogInformation($"Creating schedule for facility = {request.FacilityId} and doctor = {request.DoctorId} with data = {request.ToString()}.");

        var command = _mapper.Map<CreateScheduleCommand>(request);
        var response = await _mediator.Send(command);

        _logger.LogInformation($"Created schedule with id {response.Id}.");

        return Created(Request.Path, _mapper.Map<ScheduleResponse>(response));
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<ScheduleResponse>> Update(UpdateScheduleRequest request)
    {
        _logger.LogInformation($"Updating schedule with data = {request.ToString()}.");

        var command = _mapper.Map<UpdateScheduleCommand>(request);
        var response = await _mediator.Send(command);

        _logger.LogInformation($"Updated schedule with id {response.Id}.");

        return Ok(_mapper.Map<ScheduleResponse>(response));
    }

    [Authorize]
    [HttpDelete("id")]
    public async Task<ActionResult<ScheduleResponse>> Delete(Guid id)
    {
        _logger.LogInformation($"Deleting schedule with id = {id}.");

        var command = new DeleteScheduleCommand(id);
        await _mediator.Send(command);

        _logger.LogInformation($"Deleted schedule with id {id}.");

        return NoContent();
    }
}
