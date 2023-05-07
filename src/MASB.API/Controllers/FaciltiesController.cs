using AutoMapper;
using MABS.API.Requests.FacilityRequests;
using MABS.API.Responses.FacilityResponses;
using MABS.Application.Common.Pagination;
using MABS.Application.Features.FacilityFeatures.Commands.CreateFacility;
using MABS.Application.Features.FacilityFeatures.Commands.CreateFacilityAddress;
using MABS.Application.Features.FacilityFeatures.Commands.UpdateFacility;
using MABS.Application.Features.FacilityFeatures.Commands.UpdateFacilityAddress;
using MABS.Application.Features.FacilityFeatures.Commands.AddDoctorToFacility;
using MABS.Application.Features.FacilityFeatures.Commands.DeleteFacility;
using MABS.Application.Features.FacilityFeatures.Commands.DeleteFacilityAddress;
using MABS.Application.Features.FacilityFeatures.Commands.RemoveDoctorFromFacility;
using MABS.Application.Features.FacilityFeatures.Queries.GetAllCountries;
using MABS.Application.Features.FacilityFeatures.Queries.GetAllFacilities;
using MABS.Application.Features.FacilityFeatures.Queries.GetAllStreetTypes;
using MABS.Application.Features.FacilityFeatures.Queries.GetFacilityById;
using MABS.Application.Features.FacilityFeatures.Queries.GetFacilityDoctors;
using MASB.API.Responses.DoctorResponses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MABS.Application.Features.FacilityFeatures.Queries.GetFacilityByProfile;
using Swashbuckle.AspNetCore.Annotations;

namespace MABS.API.Controllers
{
    [ApiController]
    [Route("api/facilities")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerResponse(200, "Sukces. Zwrócono odpowiedŸ.")]
    [SwaggerResponse(204, "Sukces. Brak odpowiedzi.")]
    [SwaggerResponse(400, "B³¹d. Niepoprawny request.")]
    [SwaggerResponse(401, "B³¹d. Brak autoryzacji.")]
    [SwaggerResponse(403, "B³¹d. Zabroniono.")]
    [SwaggerResponse(404, "B³¹d. Nie znaleziono obiektu.")]
    [SwaggerResponse(407, "B³¹d. Wyst¹pi³ b³¹d biznesowy.")]
    [SwaggerResponse(500, "Nieoczekiwany b³¹d.")]
    public class FaciltiesController : ControllerBase
    {
        private readonly ILogger<FaciltiesController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public FaciltiesController(ILogger<FaciltiesController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }


        [HttpGet]
        [SwaggerOperation(
            Summary = "Pobierz placówki medyczne",
             Description = "Zwraca listê placówek medycznych. " +
                "Dodatkowo ob³uguje paginacjê, przyjmuj¹c parametry PageNumber i PageSize oraz zwracaj¹c szczegó³y w nag³ówku X-Pagination."
        )]
        public async Task<ActionResult<PagedList<FacilityResponse>>> GetAll([FromQuery] PagingParameters pagingParameters)
        {
            _logger.LogInformation("Fetching all facilities.");

            var query = new GetAllFacilitiesQuery(pagingParameters);
            var response = await _mediator.Send(query);

            _logger.LogInformation($"Returning {response.Count} facilities.");

            Response.Headers.Add("X-Pagination", response.GetMetadata());
            return Ok(response.Select(f => _mapper.Map<FacilityResponse>(f)).ToList());
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Pobierz placówkê medyczn¹ na podstawie Id",
            Description = "Zwraca szczegó³y placówki medycznej na podstawie podanego Id (UUID)."
        )]
        public async Task<ActionResult<FacilityResponse>> GetById(Guid id)
        {
            _logger.LogInformation($"Fetching facility of Id = {id}.");

            var query = new GetFacilityByIdQuery(id);
            var response = await _mediator.Send(query);

            _logger.LogInformation($"Returning facility of Id = {id}.");

            return Ok(_mapper.Map<FacilityResponse>(response));
        }

        [Authorize]
        [HttpGet("byProfile/{profileId}")]
        [SwaggerOperation(
            Summary = "Pobierz placówkê medyczn¹ na podstawie profilu",
            Description = "Zwraca szczegó³y placówki medycznej na podstawie podanego Id profilu (UUID). Wymaga autoryzacji."
        )]
        public async Task<ActionResult<FacilityResponse>> GetByProfileId(Guid profileId)
        {
            _logger.LogInformation($"Fetching facility by profile of Id = {profileId}.");

            var query = new GetFacilityByProfileQuery(profileId);
            var response = await _mediator.Send(query);

            _logger.LogInformation($"Returning facility of Id = {response.Id}.");

            return Ok(_mapper.Map<FacilityResponse>(response));
        }

        [Authorize]
        [HttpPost]
        [SwaggerOperation(
            Summary = "Stwórz placówkê medyczn¹",
            Description = "Tworzy now¹ placówkê medyczn¹ na podstawie przekazanego w JSON obiektu CreateFacilityRequest. Wymaga autoryzacji."
        )]
        public async Task<ActionResult<FacilityResponse>> Create(CreateFacilityRequest request)
        {
            _logger.LogInformation($"Creating Facility with data = {request.ToString()}.");

            var command = _mapper.Map<CreateFacilityCommand>(request);
            command.Address = _mapper.Map<CreateFacilityAddressCommand>(request.Address);
            var response = await _mediator.Send(command);

            _logger.LogInformation($"Created Facility with Id = {response.Id}.");
            
            return Created(Request.Path, _mapper.Map<FacilityResponse>(response));
        }

        [Authorize]
        [HttpPut]
        [SwaggerOperation(
            Summary = "Aktualizuj placówkê medyczn¹",
            Description = "Aktualizuje dane placówki medycznej na podstawie przekazanego w JSON obiektu UpdateFacilityRequest. Wymaga autoryzacji."
        )]
        public async Task<ActionResult<FacilityResponse>> Update(UpdateFacilityRequest request)
        {
            _logger.LogInformation($"Updating Facility with data = {request.ToString()}.");

            var command = _mapper.Map<UpdateFacilityCommand>(request);
            var response = await _mediator.Send(command);

            _logger.LogInformation($"Updated Facility of Id = {response.Id}.");

            return Ok(_mapper.Map<FacilityResponse>(response));
        }

        [Authorize]
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Usuñ placówkê medyczn¹",
            Description = "Usuwa placówkê medyczn¹ na podstawie przekazanego Id (UUID). Wymaga autoryzacji."
        )]
        public async Task<ActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"Deleting facility with id = {id}.");

            var command = new DeleteFacilityCommand(id);
            await _mediator.Send(command);

            _logger.LogInformation($"Deleted facility with id = {id}.");

            return NoContent();
        }

        [Authorize]
        [HttpPost("{facilityId}/addresses")]
        [SwaggerOperation(
            Summary = "Dodaj adres placówki medycznej",
            Description = "Dodaje nowy adres placówki medycznej. " +
                "Przyjmuje Id placówki oraz szczegó³y adresu jako obiekt CreateAddressRequest przekazany w JSON." +
                "Wymaga autoryzacji."
        )]
        public async Task<ActionResult<FacilityResponse>> CreateAddress(Guid facilityId, CreateAddressRequest request)
        {
            _logger.LogInformation($"Creating address for facility ({facilityId}) with data = {request.ToString()}.");

            var command = _mapper.Map<CreateFacilityAddressCommand>(request);
            command.FacilityId = facilityId;

            var response = await _mediator.Send(command);

            _logger.LogInformation($"Created address for facility ({facilityId}");

            return Created(Request.Path, _mapper.Map<FacilityResponse>(response));
        }

        [Authorize]
        [HttpPut("{facilityId}/addresses")]
        [SwaggerOperation(
            Summary = "Aktualizuj adres placówki medycznej",
            Description = "Aktualizuje dane adres placówki medycznej. " +
                "Przyjmuje Id placówki oraz szczegó³y adresu jako obiekt UpdateAddressRequest przekazany w JSON." +
                "Wymaga autoryzacji."
        )]
        public async Task<ActionResult<FacilityResponse>> UpdateAddress(Guid facilityId, UpdateAddressRequest request)
        {
            _logger.LogInformation($"Updating address for facility ({facilityId}) with data = {request.ToString()}.");

            var command = _mapper.Map<UpdateFacilityAddressCommand>(request);
            command.FacilityId = facilityId;

            var response = await _mediator.Send(command);

            _logger.LogInformation($"Updated address for facility ({facilityId})");
            
            return Ok(_mapper.Map<FacilityResponse>(response));
        }

        [Authorize]
        [HttpDelete("{facilityId}/addresses/{addressId}")]
        [SwaggerOperation(
            Summary = "Usuñ adres placówki medycznej",
            Description = "Usuwa adres placówki medycznej. Przyjmuje Id placówki oraz Id adresu." +
                "Wymaga autoryzacji."
        )]
        public async Task<ActionResult<FacilityResponse>> DeleteAddress(Guid facilityId, Guid addressId)
        {
            _logger.LogInformation($"Deleting address = {addressId} from facility with id = {facilityId}.");

            var command = new DeleteFacilityAddressCommand(facilityId, addressId);
            var response = await _mediator.Send(command);

            _logger.LogInformation($"Deleted address = {addressId} from facility with id = {facilityId}.");
            
            return Ok(_mapper.Map<FacilityResponse>(response));
        }

        [HttpGet("{facilityId}/doctors")]
        [SwaggerOperation(
            Summary = "Pobierz listê lekarzy w placówce medycznej",
            Description = "Zwraca listê lekarzy pracuj¹cych w placówce medycznej na podstawie Id placówki." +
               "Dodatkowo ob³uguje paginacjê, przyjmuj¹c parametry PageNumber i PageSize oraz zwracaj¹c szczegó³y w nag³ówku X-Pagination."
        )]
        public async Task<ActionResult<List<DoctorResponse>>> GetDoctors([FromQuery] PagingParameters pagingParameters, Guid facilityId)
        {
            _logger.LogInformation($"Fetching list of doctors for facility ({facilityId}).");

            var query = new GetFacilityDoctorsQuery(pagingParameters, facilityId);
            var response = await _mediator.Send(query);

            _logger.LogInformation($"Returning {response.Count} doctors for facility ({facilityId}).");

            Response.Headers.Add("X-Pagination", response.GetMetadata());
            return Ok(response.Select(f => _mapper.Map<DoctorResponse>(f)).ToList()); 
        }

        [Authorize]
        [HttpPost("{facilityId}/doctors/{doctorId}")]
        [SwaggerOperation(
            Summary = "Dodaj lekarza do placówki medycznej",
            Description = "Dodaje lekarza do placówki medycznej oraz zwraca listê wszystkich lekarzy pracuj¹cych w placówkce." +
               "Przyjmue Id placówki oraz Id lekarza." +
               "Dodatkowo ob³uguje paginacjê, przyjmuj¹c parametry PageNumber i PageSize oraz zwracaj¹c szczegó³y w nag³ówku X-Pagination." +
               "Wymaga autoryzacji."
        )]
        public async Task<ActionResult<List<DoctorResponse>>> AddDoctor([FromQuery] PagingParameters pagingParameters, Guid facilityId, Guid doctorId)
        {
            _logger.LogInformation($"Adding doctor ({doctorId}) to facility ({facilityId}) with data.");

            var command = new AddDoctorToFacilityCommand(pagingParameters, facilityId, doctorId);
            var response = await _mediator.Send(command);

            _logger.LogInformation($"Added doctor ({doctorId}) to facility ({facilityId}) with data.");

            Response.Headers.Add("X-Pagination", response.GetMetadata());
            return Created(
                Request.Path,
                response.Select(f => _mapper.Map<DoctorResponse>(f)).ToList()
            );
        }

        [Authorize]
        [HttpDelete("{facilityId}/doctors/{doctorId}")]
        [SwaggerOperation(
            Summary = "Usuñ lekarza z placówki medycznej",
            Description = "Usuwa lekarza z placówki medycznej oraz zwraca listê wszystkich lekarzy pracuj¹cych w placówkce." +
               "Przyjmue Id placówki oraz Id lekarza." +
               "Dodatkowo ob³uguje paginacjê, przyjmuj¹c parametry PageNumber i PageSize oraz zwracaj¹c szczegó³y w nag³ówku X-Pagination." +
               "Wymaga autoryzacji."
        )]
        public async Task<ActionResult<List<DoctorResponse>>> RemoveDoctor([FromQuery] PagingParameters pagingParameters, Guid facilityId, Guid doctorId)
        {
            _logger.LogInformation($"Adding doctor ({doctorId}) to facility ({facilityId}) with data.");

            var command = new RemoveDoctorFromFacilityCommand(pagingParameters, facilityId, doctorId);
            var response = await _mediator.Send(command);

            _logger.LogInformation($"Added doctor ({doctorId}) to facility ({facilityId}) with data.");

            Response.Headers.Add("X-Pagination", response.GetMetadata());
            return Ok(response.Select(f => _mapper.Map<DoctorResponse>(f)).ToList());
        }

        [HttpGet("dict/countries")]
        [SwaggerOperation(
            Summary = "Pobierz kraje",
            Description = "Zwraca listê krajów dostêpnych w systemie."
        )]
        public async Task<ActionResult<List<CountryResponse>>> GetCountries()
        {
            _logger.LogInformation("Fetching all countries.");

            var response = await _mediator.Send(new GetAllCountriesQuery());

            _logger.LogInformation($"Returning {response.Count} countries.");
            
            return Ok(response.Select(c => _mapper.Map<CountryResponse>(c)).ToList());
        }

        [HttpGet("dict/streetTypes")]
        [SwaggerOperation(
            Summary = "Pobierz typy ulicy",
            Description = "Zwraca listê typów ulicy dostêpnych w systemie."
        )]
        public async Task<ActionResult<List<StreetTypeResponse>>> GetStreetTypes()
        {
            _logger.LogInformation("Fetching all street types.");

            var response = await _mediator.Send(new GetAllStreetTypesQuery());

            _logger.LogInformation($"Returning {response.Count} street types.");

            return Ok(response.Select(c => _mapper.Map<StreetTypeResponse>(c)).ToList());
        }
    }
}