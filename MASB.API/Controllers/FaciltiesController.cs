using MABS.Application.Common.Pagination;
using MABS.Application.DTOs.FacilityDtos;
using MABS.Application.Services.DoctorServices.Common;
using MABS.Application.Services.FacilityServices;
using Microsoft.AspNetCore.Mvc;

namespace MABS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaciltiesController : ControllerBase
    {
        private readonly ILogger<FaciltiesController> _logger;
        private readonly IFacilityService _facilityService;
        public FaciltiesController(ILogger<FaciltiesController> logger, IFacilityService facilityService)
        {
            _logger = logger;
            _facilityService = facilityService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<FacilityDto>>> GetAll([FromQuery] PagingParameters pagingParameters)
        {
            _logger.LogInformation("Fetching all facilities.");
            var response = await _facilityService.GetAll(pagingParameters);
            _logger.LogInformation($"Returning {response.Count} facilities.");

            Response.Headers.Add("X-Pagination", response.GetMetadata());
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FacilityDto>> GetById(Guid id)
        {
            _logger.LogInformation($"Fetching facility of Id = {id}.");
            var response = await _facilityService.GetById(id);
            _logger.LogInformation($"Returning facility of Id = {id}.");

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<FacilityDto>> Create(CreateFacilityDto request)
        {
            _logger.LogInformation($"Creating Facility with data = {request.ToString()}.");
            var response = await _facilityService.Create(request);
            _logger.LogInformation($"Created Facility with Id = {response.Id}.");
            
            return Created(Request.Path, response);
        }
        
        [HttpPut]
        public async Task<ActionResult<FacilityDto>> Update(UpdateFacilityDto request)
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
        public async Task<ActionResult<FacilityDto>> CreateAddress(Guid facilityId, CreateAddressDto request)
        {
            _logger.LogInformation($"Creating address for facility ({facilityId}) with data = {request.ToString()}.");
            var response = await _facilityService.CreateAddress(facilityId, request);
            _logger.LogInformation($"Created address for facility ({facilityId}");

            return Created(Request.Path, response);
        }

        [HttpPut("{facilityId}/Addresses")]
        public async Task<ActionResult<FacilityDto>> UpdateAddress(Guid facilityId, UpdateAddressDto request)
        {
            _logger.LogInformation($"Updating address for facility ({facilityId}) with data = {request.ToString()}.");
            var response = await _facilityService.UpdateAddress(facilityId, request);
            _logger.LogInformation($"Updated address for facility ({facilityId})");

            return Ok(response);
        }

        [HttpDelete("{facilityId}/Addresses/{addressId}")]
        public async Task<ActionResult<FacilityDto>> DeleteAddress(Guid facilityId, Guid addressId)
        {
            _logger.LogInformation($"Deleting address = {addressId} from facility with id = {facilityId}.");
            var response = await _facilityService.DeleteAddress(facilityId, addressId);
            _logger.LogInformation($"Deleted address = {addressId} from facility with id = {facilityId}.");

            return Ok(response);
        }

        [HttpGet("{facilityId}/Doctors")]
        public async Task<ActionResult<List<DoctorDto>>> GetDoctors([FromQuery] PagingParameters pagingParameters, Guid facilityId)
        {
            _logger.LogInformation($"Fetching list of doctors for facility ({facilityId}).");
            var response = await _facilityService.GetAllDoctors(pagingParameters, facilityId);
            _logger.LogInformation($"Returning {response.Count} doctors for facility ({facilityId}).");

            return Ok(response);
        }

        [HttpPost("{facilityId}/Doctors/{doctorId}")]
        public async Task<ActionResult<List<DoctorDto>>> AddDoctor([FromQuery] PagingParameters pagingParameters, Guid facilityId, Guid doctorId)
        {
            _logger.LogInformation($"Adding doctor ({doctorId}) to facility ({facilityId}) with data.");
            var response = await _facilityService.AddDoctor(pagingParameters, facilityId, doctorId);
            _logger.LogInformation($"Added doctor ({doctorId}) to facility ({facilityId}) with data.");

            return Created(Request.Path, response);
        }

        [HttpDelete("{facilityId}/Doctors/{doctorId}")]
        public async Task<ActionResult<List<DoctorDto>>> RemoveDoctor([FromQuery] PagingParameters pagingParameters, Guid facilityId, Guid doctorId)
        {
            _logger.LogInformation($"Adding doctor ({doctorId}) to facility ({facilityId}) with data.");
            var response = await _facilityService.RemoveDoctor(pagingParameters, facilityId, doctorId);
            _logger.LogInformation($"Added doctor ({doctorId}) to facility ({facilityId}) with data.");

            return Ok(response);
        }

        [HttpGet("Dictonaries/Countries")]
        public async Task<ActionResult<List<CountryDto>>> GetCountries()
        {
            _logger.LogInformation("Fetching all countries.");
            var response = await _facilityService.GetAllCountries();
            _logger.LogInformation($"Returning {response.Count} countries.");

            return Ok(response);
        }

        [HttpGet("Dictonaries/StreetTypes")]
        public async Task<ActionResult<List<CountryDto>>> GetStreetTypes()
        {
            _logger.LogInformation("Fetching all street types.");
            var response = await _facilityService.GetAllStreetTypes();
            _logger.LogInformation($"Returning {response.Count} street types.");

            return Ok(response);
        }
    }
}