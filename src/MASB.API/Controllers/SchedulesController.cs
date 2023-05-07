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
using Swashbuckle.AspNetCore.Annotations;

namespace MASB.API.Controllers;

[ApiController]
[Route("api/schedules")]
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
    [SwaggerOperation(
        Summary = "Pobierz listę harmonogramów",
        Description = "Zwraca listę harmonogramów pracy na podstawie Id lekarza oraz Id adresu. Wymaga autoryzacji."
    )]
    public async Task<ActionResult<List<ScheduleResponse>>> GetSchedules(Guid addressId, Guid doctorId)
    {
        _logger.LogInformation($"Fetching schedules for addressId = {addressId} and doctor = {doctorId}.");

        var query = new GetScheduleQuery(doctorId, addressId);
        var response = await _mediator.Send(query);

        _logger.LogInformation($"Fetched {response.Count} schedules.");

        return Ok(response.Select(s => _mapper.Map<ScheduleResponse>(s)).ToList());
    }

    [Authorize]
    [HttpPost]
    [SwaggerOperation(
        Summary = "Stwóz harmonogram",
        Description = "Tworzy nowy harmonogram pracy na podstawie przekazanego w JSON obiektu CreateScheduleRequest. Wymaga autoryzacji."
    )]
    public async Task<ActionResult<ScheduleResponse>> Create(CreateScheduleRequest request)
    {
        _logger.LogInformation($"Creating schedule for address = {request.AddressId} and doctor = {request.DoctorId} with data = {request.ToString()}.");

        var command = _mapper.Map<CreateScheduleCommand>(request);
        var response = await _mediator.Send(command);

        _logger.LogInformation($"Created schedule with id {response.Id}.");

        return Created(Request.Path, _mapper.Map<ScheduleResponse>(response));
    }

    [Authorize]
    [HttpPut]
    [SwaggerOperation(
        Summary = "Aktualizuj harmonogram",
        Description = "Aktualizuje harmonogram pracy na podstawie przekazanego w JSON obiektu UpdateScheduleRequest. Wymaga autoryzacji."
    )]
    public async Task<ActionResult<ScheduleResponse>> Update(UpdateScheduleRequest request)
    {
        _logger.LogInformation($"Updating schedule with data = {request.ToString()}.");

        var command = _mapper.Map<UpdateScheduleCommand>(request);
        var response = await _mediator.Send(command);

        _logger.LogInformation($"Updated schedule with id {response.Id}.");

        return Ok(_mapper.Map<ScheduleResponse>(response));
    }

    [Authorize]
    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Usuń harmonogram",
        Description = "Usuwa harmonogram pracy na podstawie Id harmonogramu (UUID). Wymaga autoryzacji."
    )]
    public async Task<ActionResult<ScheduleResponse>> Delete(Guid id)
    {
        _logger.LogInformation($"Deleting schedule with id = {id}.");

        var command = new DeleteScheduleCommand(id);
        await _mediator.Send(command);

        _logger.LogInformation($"Deleted schedule with id {id}.");

        return NoContent();
    }
}
