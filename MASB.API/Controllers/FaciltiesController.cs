using AutoMapper;
using MABS.API.Requests.FacilityRequests;
using MABS.API.Responses.FacilityResponses;
using MABS.Application.Common.Pagination;
using MABS.Application.Services.FacilityServices;
using MABS.Application.Services.FacilityServices.Commands.CreateFacilityAddress;
using MABS.Application.Services.FacilityServices.Queries.AllFacilities;
using MABS.Application.Services.FacilityServices.Queries.FacilityById;
using MABS.Services.FacilityServices.Commands.CreateFacility;
using MASB.API.Responses.DoctorResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MABS.API.Controllers
{
    [ApiController]
    [Route("api/facilities")]
    public class FaciltiesController : ControllerBase
    {
        private readonly ILogger<FaciltiesController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IFacilityService _facilityService;

        public FaciltiesController(ILogger<FaciltiesController> logger, IFacilityService facilityService, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _facilityService = facilityService;
            _mediator = mediator;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<PagedList<FacilityResponse>>> GetAll([FromQuery] PagingParameters pagingParameters)
        {
            _logger.LogInformation("Fetching all facilities.");

            var query = new AllFacilitiesQuery(pagingParameters);
            var response = await _mediator.Send(query);

            _logger.LogInformation($"Returning {response.Count} facilities.");

            Response.Headers.Add("X-Pagination", response.GetMetadata());
            return Ok(response.Select(f => _mapper.Map<FacilityResponse>(f)).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FacilityResponse>> GetById(Guid id)
        {
            _logger.LogInformation($"Fetching facility of Id = {id}.");

            var query = new FacilityByIdQuery(id);
            var response = await _mediator.Send(query);

            _logger.LogInformation($"Returning facility of Id = {id}.");

            return Ok(_mapper.Map<FacilityResponse>(response));
        }

        [HttpPost]
        public async Task<ActionResult<FacilityResponse>> Create(CreateFacilityRequest request)
        {
            _logger.LogInformation($"Creating Facility with data = {request.ToString()}.");

            var command = _mapper.Map<CreateFacilityCommand>(request);
            command.Address = _mapper.Map<CreateFacilityAddressCommand>(request.Address);
            var response = await _mediator.Send(command);

            _logger.LogInformation($"Created Facility with Id = {response.Id}.");
            
            return Created(Request.Path, response);
        }
        
        [HttpPut]
        public async Task<ActionResult<FacilityResponse>> Update(UpdateFacilityRequest request)
        {
            _logger.LogInformation($"Updating Facility with data = {request.ToString()}.");
            var response = await _facilityService.Update(request);
            _logger.LogInformation($"Updated Facility of Id = {response.Id}.");

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"Deleting facility with id = {id}.");
            await _facilityService.Delete(id);
            _logger.LogInformation($"Deleted facility with id = {id}.");

            return NoContent();
        }

        [HttpPost("{facilityId}/Addresses")]
        public async Task<ActionResult<FacilityResponse>> CreateAddress(Guid facilityId, CreateAddressRequest request)
        {
            _logger.LogInformation($"Creating address for facility ({facilityId}) with data = {request.ToString()}.");
            var response = await _facilityService.CreateAddress(facilityId, request);
            _logger.LogInformation($"Created address for facility ({facilityId}");

            return Created(Request.Path, response);
        }

        [HttpPut("{facilityId}/Addresses")]
        public async Task<ActionResult<FacilityResponse>> UpdateAddress(Guid facilityId, UpdateAddressRequest request)
        {
            _logger.LogInformation($"Updating address for facility ({facilityId}) with data = {request.ToString()}.");
            var response = await _facilityService.UpdateAddress(facilityId, request);
            _logger.LogInformation($"Updated address for facility ({facilityId})");

            return Ok(response);
        }

        [HttpDelete("{facilityId}/Addresses/{addressId}")]
        public async Task<ActionResult<FacilityResponse>> DeleteAddress(Guid facilityId, Guid addressId)
        {
            _logger.LogInformation($"Deleting address = {addressId} from facility with id = {facilityId}.");
            var response = await _facilityService.DeleteAddress(facilityId, addressId);
            _logger.LogInformation($"Deleted address = {addressId} from facility with id = {facilityId}.");

            return Ok(response);
        }

        [HttpGet("{facilityId}/Doctors")]
        public async Task<ActionResult<List<DoctorResponse>>> GetDoctors([FromQuery] PagingParameters pagingParameters, Guid facilityId)
        {
            _logger.LogInformation($"Fetching list of doctors for facility ({facilityId}).");
            var response = await _facilityService.GetAllDoctors(pagingParameters, facilityId);
            _logger.LogInformation($"Returning {response.Count} doctors for facility ({facilityId}).");

            return Ok(response);
        }

        [HttpPost("{facilityId}/Doctors/{doctorId}")]
        public async Task<ActionResult<List<DoctorResponse>>> AddDoctor([FromQuery] PagingParameters pagingParameters, Guid facilityId, Guid doctorId)
        {
            _logger.LogInformation($"Adding doctor ({doctorId}) to facility ({facilityId}) with data.");
            var response = await _facilityService.AddDoctor(pagingParameters, facilityId, doctorId);
            _logger.LogInformation($"Added doctor ({doctorId}) to facility ({facilityId}) with data.");

            return Created(Request.Path, response);
        }

        [HttpDelete("{facilityId}/Doctors/{doctorId}")]
        public async Task<ActionResult<List<DoctorResponse>>> RemoveDoctor([FromQuery] PagingParameters pagingParameters, Guid facilityId, Guid doctorId)
        {
            _logger.LogInformation($"Adding doctor ({doctorId}) to facility ({facilityId}) with data.");
            var response = await _facilityService.RemoveDoctor(pagingParameters, facilityId, doctorId);
            _logger.LogInformation($"Added doctor ({doctorId}) to facility ({facilityId}) with data.");

            return Ok(response);
        }

        [HttpGet("Dictonaries/Countries")]
        public async Task<ActionResult<List<CountryResponse>>> GetCountries()
        {
            _logger.LogInformation("Fetching all countries.");
            var response = await _facilityService.GetAllCountries();
            _logger.LogInformation($"Returning {response.Count} countries.");

            return Ok(response);
        }

        [HttpGet("Dictonaries/StreetTypes")]
        public async Task<ActionResult<List<StreetTypeResponse>>> GetStreetTypes()
        {
            _logger.LogInformation("Fetching all street types.");
            var response = await _facilityService.GetAllStreetTypes();
            _logger.LogInformation($"Returning {response.Count} street types.");

            return Ok(response);
        }
    }
}