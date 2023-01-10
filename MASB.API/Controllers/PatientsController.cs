using MABS.Application.DTOs.PatientDtos;
using MABS.Application.Services.PatientServices;
using Microsoft.AspNetCore.Mvc;

namespace MABS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly ILogger<PatientsController> _logger;
        private readonly IPatientService _patientService;

        public PatientsController(ILogger<PatientsController> logger, IPatientService patientService)
        {
            _logger = logger;
            _patientService = patientService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> GetById(Guid id)
        {
            _logger.LogInformation($"Fetching patient by Id({id}).");
            var resposne = await _patientService.GetById(id);
            _logger.LogInformation($"Returning patient by Id({id}).");

            return Ok(resposne);
        }

        [HttpPost]
        public async Task<ActionResult<PatientDto>> Create(CreatePatientDto request)
        {
            _logger.LogInformation($"Creating patient with data = {request.ToString()}.");
            var response = await _patientService.Create(request);
            _logger.LogInformation($"Created patient with Id = {response.Id}.");

            return Created(Request.Path, response);
        }

        [HttpPut]
        public async Task<ActionResult<PatientDto>> Update(UpdatePatientDto request)
        {
            _logger.LogInformation($"Updating patient with data = {request.ToString()}.");
            var response = await _patientService.Update(request);
            _logger.LogInformation($"Updating patient with Id = {request.Id}.");

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"Deleting patient with id = {id}.");
            await _patientService.Delete(id);
            _logger.LogInformation($"Deleted patient with id = {id}.");

            return NoContent();
        }
    }
}
