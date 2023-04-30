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

namespace MABS.API.Controllers
{
    [ApiController]
    [Route("api/facilities")]
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
        public async Task<ActionResult<FacilityResponse>> GetById(Guid id)
        {
            _logger.LogInformation($"Fetching facility of Id = {id}.");

            var query = new GetFacilityByIdQuery(id);
            var response = await _mediator.Send(query);

            _logger.LogInformation($"Returning facility of Id = {id}.");

            return Ok(_mapper.Map<FacilityResponse>(response));
        }

        [HttpGet("byProfile/{profileId}")]
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
        public async Task<ActionResult<FacilityResponse>> Create(CreateFacilityRequest request)
        {
            _logger.LogInformation($"Creating Facility with data = {request.ToString()}.");

            var command = _mapper.Map<CreateFacilityCommand>(request);
            command.Address = _mapper.Map<CreateFacilityAddressCommand>(request.Address);
            var response = await _mediator.Send(command);

            _logger.LogInformation($"Created Facility with Id = {response.Id}.");
            
            return Created(Request.Path, _mapper.Map<FacilityResponse>(response));
        }

        //[Authorize]
        [HttpPut]
        public async Task<ActionResult<FacilityResponse>> Update(UpdateFacilityRequest request)
        {
            _logger.LogInformation($"Updating Facility with data = {request.ToString()}.");

            var command = _mapper.Map<UpdateFacilityCommand>(request);
            var response = await _mediator.Send(command);

            _logger.LogInformation($"Updated Facility of Id = {response.Id}.");

            return Ok(_mapper.Map<FacilityResponse>(response));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"Deleting facility with id = {id}.");

            var command = new DeleteFacilityCommand(id);
            await _mediator.Send(command);

            _logger.LogInformation($"Deleted facility with id = {id}.");

            return NoContent();
        }

        [HttpPost("{facilityId}/addresses")]
        public async Task<ActionResult<FacilityResponse>> CreateAddress(Guid facilityId, CreateAddressRequest request)
        {
            _logger.LogInformation($"Creating address for facility ({facilityId}) with data = {request.ToString()}.");

            var command = _mapper.Map<CreateFacilityAddressCommand>(request);
            command.FacilityId = facilityId;

            var response = await _mediator.Send(command);

            _logger.LogInformation($"Created address for facility ({facilityId}");

            return Created(Request.Path, _mapper.Map<FacilityResponse>(response));
        }

        [HttpPut("{facilityId}/addresses")]
        public async Task<ActionResult<FacilityResponse>> UpdateAddress(Guid facilityId, UpdateAddressRequest request)
        {
            _logger.LogInformation($"Updating address for facility ({facilityId}) with data = {request.ToString()}.");

            var command = _mapper.Map<UpdateFacilityAddressCommand>(request);
            command.FacilityId = facilityId;

            var response = await _mediator.Send(command);

            _logger.LogInformation($"Updated address for facility ({facilityId})");
            
            return Ok(_mapper.Map<FacilityResponse>(response));
        }

        [HttpDelete("{facilityId}/addresses/{addressId}")]
        public async Task<ActionResult<FacilityResponse>> DeleteAddress(Guid facilityId, Guid addressId)
        {
            _logger.LogInformation($"Deleting address = {addressId} from facility with id = {facilityId}.");

            var command = new DeleteFacilityAddressCommand(facilityId, addressId);
            var response = await _mediator.Send(command);

            _logger.LogInformation($"Deleted address = {addressId} from facility with id = {facilityId}.");
            
            return Ok(_mapper.Map<FacilityResponse>(response));
        }

        [HttpGet("{facilityId}/doctors")]
        public async Task<ActionResult<List<DoctorResponse>>> GetDoctors([FromQuery] PagingParameters pagingParameters, Guid facilityId)
        {
            _logger.LogInformation($"Fetching list of doctors for facility ({facilityId}).");

            var query = new GetFacilityDoctorsQuery(pagingParameters, facilityId);
            var response = await _mediator.Send(query);

            _logger.LogInformation($"Returning {response.Count} doctors for facility ({facilityId}).");

            Response.Headers.Add("X-Pagination", response.GetMetadata());
            return Ok(response.Select(f => _mapper.Map<DoctorResponse>(f)).ToList()); 
        }

        [HttpPost("{facilityId}/doctors/{doctorId}")]
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

        [HttpDelete("{facilityId}/doctors/{doctorId}")]
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
        public async Task<ActionResult<List<CountryResponse>>> GetCountries()
        {
            _logger.LogInformation("Fetching all countries.");

            var response = await _mediator.Send(new GetAllCountriesQuery());

            _logger.LogInformation($"Returning {response.Count} countries.");
            
            return Ok(response.Select(c => _mapper.Map<CountryResponse>(c)).ToList());
        }

        [HttpGet("dict/streetTypes")]
        public async Task<ActionResult<List<StreetTypeResponse>>> GetStreetTypes()
        {
            _logger.LogInformation("Fetching all street types.");

            var response = await _mediator.Send(new GetAllStreetTypesQuery());

            _logger.LogInformation($"Returning {response.Count} street types.");

            return Ok(response.Select(c => _mapper.Map<StreetTypeResponse>(c)).ToList());
        }
    }
}