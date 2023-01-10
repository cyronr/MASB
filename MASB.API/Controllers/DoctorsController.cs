using MABS.Application.Common.Pagination;
using MABS.Application.DTOs.DoctorDtos;
using MABS.Application.Services.DoctorServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MABS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly ILogger<DoctorsController> _logger;
        private readonly IDoctorService _doctorService;
        public DoctorsController(ILogger<DoctorsController> logger, IDoctorService doctorService)
        {
            _logger = logger;
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<DoctorDto>>> GetAll([FromQuery] PagingParameters pagingParameters)
        {
            _logger.LogInformation("Fetching all doctors.");
            var response = await _doctorService.GetAll(pagingParameters);
            _logger.LogInformation($"Returning {response.Count} doctors.");

            Response.Headers.Add("X-Pagination", response.GetMetadata());
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDto>> GetById(Guid id)
        {
            _logger.LogInformation($"Fetching doctor of Id = {id}.");
            var response = await _doctorService.GetById(id);
            _logger.LogInformation($"Returning doctor of Id = {id}.");

            return Ok(response);
        }

        [HttpGet("BySpecialties")]
        public async Task<ActionResult<PagedList<DoctorDto>>> GetBySpecialtyIds([FromQuery]List<int> ids, [FromQuery] PagingParameters pagingParameters)
        {
            _logger.LogInformation($"Fetching doctors for specalties Ids = {String.Join(", ", ids.ToArray())}.");
            var response = await _doctorService.GetBySpecalties(ids, pagingParameters);
            _logger.LogInformation($"Returning {response.Count} doctors.");

            Response.Headers.Add("X-Pagination", response.GetMetadata());
            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<DoctorDto>> Create(CreateDoctorDto request)
        {
            _logger.LogInformation($"Creating doctor with data = {request.ToString()}.");
            var response = await _doctorService.Create(request);
            _logger.LogInformation($"Created doctor with Id = {response.Id}.");
            
            return Created(Request.Path, response);
        }

        [HttpPut]
        public async Task<ActionResult<DoctorDto>> Update(UpdateDoctorDto request)
        {
            _logger.LogInformation($"Updating doctor with data = {request.ToString()}.");
            var response = await _doctorService.Update(request);
            _logger.LogInformation($"Updated doctor of Id = {response.Id}.");

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"Deleting doctor with id = {id}.");
            await _doctorService.Delete(id);
            _logger.LogInformation($"Deleted doctor with id = {id}.");

            return NoContent();
        }

        [HttpGet("Dictonaries/Specialties")]
        public async Task<ActionResult<List<SpecialityExtendedDto>>> GetSpecialities()
        {
            _logger.LogInformation("Fetching all specialities.");
            var response = await _doctorService.GetAllSpecalties();
            _logger.LogInformation($"Returning {response.Count} specialities.");

            return Ok(response);
        }

        [HttpGet("Dictonaries/Titles")]
        public async Task<ActionResult<List<TitleExtendedDto>>> GetTitles()
        {
            _logger.LogInformation("Fetching all titles.");
            var response = await _doctorService.GetAllTitles();
            _logger.LogInformation($"Returning {response.Count} titles.");

            return Ok(response);
        }

    }
}