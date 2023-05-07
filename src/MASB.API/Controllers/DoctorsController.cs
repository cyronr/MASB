using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MASB.API.Requests.DoctorRequests;
using MASB.API.Responses.DoctorResponses;
using MABS.Application.Common.Pagination;
using MABS.Application.Features.DoctorFeatures.Commands.DeleteDoctor;
using MABS.Application.Features.DoctorFeatures.Queries.GetDoctorById;
using MABS.Application.Features.DoctorFeatures.Queries.GetAllSpecialties;
using MABS.Application.Features.DoctorFeatures.Queries.GetAllTitles;
using MABS.Application.Features.DoctorFeatures.Queries.SearchAllDoctors;
using MABS.Application.Features.DoctorFeatures.Commands.CreateDoctor;
using MABS.Application.Features.DoctorFeatures.Commands.UpdateDoctor;
using MABS.Application.Features.DoctorFeatures.Queries.FindExactDoctor;
using Nest;
using MABS.Application.Features.DoctorFeatures.Queries.GetTimeSlots;
using MABS.Application.Features.DoctorFeatures.Common;
using Microsoft.AspNetCore.Authorization;
using MABS.Application.Features.DoctorFeatures.Queries.GetAddresses;
using Azure;
using Swashbuckle.AspNetCore.Annotations;

namespace MABS.API.Controllers
{
    [ApiController]
    [Route("api/doctors")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerResponse(200, "Sukces. Zwr�cono odpowied�.")]
    [SwaggerResponse(204, "Sukces. Brak odpowiedzi.")]
    [SwaggerResponse(400, "B��d. Niepoprawny request.")]
    [SwaggerResponse(401, "B��d. Brak autoryzacji.")]
    [SwaggerResponse(403, "B��d. Zabroniono.")]
    [SwaggerResponse(404, "B��d. Nie znaleziono obiektu.")]
    [SwaggerResponse(407, "B��d. Wyst�pi� b��d biznesowy.")]
    [SwaggerResponse(500, "Nieoczekiwany b��d.")]
    public class DoctorsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DoctorsController> _logger;
        private readonly IMapper _mapper;

        public DoctorsController(ILogger<DoctorsController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("search")]
        [SwaggerOperation(
            Summary = "Pobierz list� lekarzy",
            Description = "Zwraca list� lekarzy na podstawie przekazanych parametr�w." +
                "Przyjmuje parametry SearchText, SpecialtyId oraz CityId. Wymagane jest podanie przynajmniej SearchText lub SpecialtyId." +
                "Dodatkowo ob�uguje paginacj�, przyjmuj�c parametry PageNumber i PageSize oraz zwracaj�c szczeg�y w nag��wku X-Pagination."
        )]
        public async Task<ActionResult<PagedList<DoctorResponse>>> SearchAll([FromQuery] PagingParameters pagingParameters, string? searchText, int? specialtyId, int? cityId)
        {
            _logger.LogInformation("Fetching all doctors.");

            var query = new SearchAllDoctorsQuery(pagingParameters, searchText, specialtyId, cityId);
            var response = await _mediator.Send(query);

            _logger.LogInformation($"Returning {response.Count} doctors.");

            Response.Headers.Add("X-Pagination", response.GetMetadata());

            return Ok(response.Select(d => _mapper.Map<DoctorResponse>(d)).ToList());
        }

        [HttpGet("find")]
        [SwaggerOperation(
            Summary = "Znajd� lekarza",
            Description = "Zwraca lekarza na podstawie przekazanych parametr�w." +
                "Przyjmuje parametry FirstName, LastName oraz lsit� specjalizacji. " +
                "Zwraca lekarza, je�eli znajdzie takiego, kt�ry dok�adnie odpowiada przekazanym parametrom."
        )]
        public async Task<ActionResult<DoctorResponse>> FindExact(string firstName, string lastName, [FromQuery]List<int> specialtiesIds)
        {
            _logger.LogInformation("FInding exact doctor.");

            var query = new FindExactDoctorQuery(firstName, lastName, specialtiesIds);
            var response = await _mediator.Send(query);

            _logger.LogInformation($"Returning doctor.");

            return Ok(_mapper.Map<DoctorResponse>(response));
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Pobierz lekarza na podstawie Id",
            Description = "Zwraca szczeg�y lekarza na podstawie podanego Id (UUID)."
        )]
        public async Task<ActionResult<DoctorResponse>> GetById(Guid id)
        {
            _logger.LogInformation($"Fetching doctor of Id = {id}.");

            var query = new GetDoctorByIdQuery(id);
            var response = await _mediator.Send(query);

            _logger.LogInformation($"Returning doctor of Id = {id}.");

            return Ok(_mapper.Map<DoctorResponse>(response));
        }

        [Authorize]
        [HttpPost]
        [SwaggerOperation(
            Summary = "Stw�rz lekarza",
            Description = "Tworzy nowego lekarza na podstawie przekazanego w JSON obiektu CreateDoctorRequest. Wymaga autoryzacji."
        )]
        public async Task<ActionResult<DoctorResponse>> Create(CreateDoctorRequest request)
        {
            _logger.LogInformation($"Creating doctor with data = {request.ToString()}.");

            var command = _mapper.Map<CreateDoctorCommand>(request);
            var response = await _mediator.Send(command);

            _logger.LogInformation($"Created doctor with Id = {response.Id}.");

            return base.Created(Request.Path, _mapper.Map<DoctorResponse>(response));
        }

        [Authorize]
        [HttpPut]
        [SwaggerOperation(
            Summary = "Aktualizuj lekarza",
            Description = "Aktualizuje dane lekarza na podstawie przekazanego w JSON obiektu UpdateDoctorRequest. Wymaga autoryzacji."
        )]
        public async Task<ActionResult<DoctorResponse>> Update(UpdateDoctorRequest request)
        {
            _logger.LogInformation($"Updating doctor with data = {request.ToString()}.");

            var command = _mapper.Map<UpdateDoctorCommand>(request);
            var response = await _mediator.Send(command);

            _logger.LogInformation($"Updated doctor of Id = {response.Id}.");

            return Ok(_mapper.Map<DoctorResponse>(response));
        }

        [Authorize]
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Usu� lekarza",
            Description = "Usuwa lekarza na podstawie przekazanego Id (UUID). Wymaga autoryzacji."
        )]
        public async Task<ActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"Deleting doctor with id = {id}.");

            var command = new DeleteDoctorCommand(id);
            await _mediator.Send(command);

            _logger.LogInformation($"Deleted doctor with id = {id}.");

            return NoContent();
        }

        [HttpGet("{id}/timeSlots")]
        [SwaggerOperation(
            Summary = "Pobierz okna czasowe",
            Description = "Zwraca list� okien czasowych, na kt�re mo�na rezerwowa� wizyty do lekarza w adresie."
        )]
        public async Task<ActionResult<List<TimeSlot>>> GetTimeSlots(Guid id, Guid addressId)
        {
            _logger.LogInformation($"Getting time slots for doctor with id = {id} and address with id = {addressId}.");

            var query = new GetTimeSlotsQuery(id, addressId);
            var results = await _mediator.Send(query);

            _logger.LogInformation($"Returning {results.Count} time slots for doctor with id = {id} and address with id = {addressId}.");

            return Ok(results.OrderBy(r => r.Date).ThenBy(r => r.Time));
        }

        [HttpGet("{id}/addresses")]
        [SwaggerOperation(
            Summary = "Pobierz adresy",
            Description = "Zwraca list� adres�w, w kt�rych przyjmuje lekarz."
        )]
        public async Task<ActionResult<List<AddressResponse>>> GetAddresses(Guid id)
        {
            _logger.LogInformation($"Getting addresses for doctor with id = {id}");

            var query = new GetAddressesQuery(id);
            var results = await _mediator.Send(query);

            _logger.LogInformation($"Returning {results.Count} addresses for doctor with id = {id}");

            return Ok(results.Select(d => _mapper.Map<AddressResponse>(d)).ToList());
        }

        [HttpGet("dict/specialties")]
        [SwaggerOperation(
            Summary = "Pobierz specjalizacj�",
            Description = "Zwraca list� specjalizacji dost�pnych w systemie."
        )]
        public async Task<ActionResult<List<SpecialtyResponse>>> GetSpecialities()
        {
            _logger.LogInformation("Fetching all specialities.");

            var command = new GetAllSpecialtiesQuery();
            var response = await _mediator.Send(command);

            _logger.LogInformation($"Returning {response.Count} specialities.");

            return Ok(response.Select(s => _mapper.Map<SpecialtyResponse>(s)).ToList());
        }

        [HttpGet("dict/titles")]
        [SwaggerOperation(
            Summary = "Pobierz tytu�y",
            Description = "Zwraca list� tytu��w naukowych dost�pnych w systemie."
        )]
        public async Task<ActionResult<List<TitleResponse>>> GetTitles()
        {
            _logger.LogInformation("Fetching all titles.");

            var command = new GetAllTitlesQuery();
            var response = await _mediator.Send(command);

            _logger.LogInformation($"Returning {response.Count} titles.");

            return Ok(response.Select(t => _mapper.Map<TitleResponse>(t)).ToList());
        }

    }
}