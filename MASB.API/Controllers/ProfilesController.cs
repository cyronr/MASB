using MABS.Application.DTOs.ProfileDtos;
using MABS.Application.Services.ProfileServices;
using Microsoft.AspNetCore.Mvc;

namespace MABS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfilesController : ControllerBase
    {
        private readonly ILogger<ProfilesController> _logger;
        private readonly IProfileService _profileService;

        public ProfilesController(ILogger<ProfilesController> logger, IProfileService profileService)
        {
            _logger = logger;
            _profileService = profileService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(LoginProfileDto request)
        {
            _logger.LogInformation($"Logging {request.Email}.");
            var response = await _profileService.Login(request);
            _logger.LogInformation($"{request.Email} logged with token {response}.");

            return Ok(response);
        }

        [HttpPost("Register/Patient")]
        public async Task<ActionResult<ProfileDto>> RegisterPatient(RegisterPatientProfileDto request)
        {
            _logger.LogInformation($"Registering new Patient profile {request.Email}.");
            var response = await _profileService.RegisterPatientProfile(request);
            _logger.LogInformation($"Registered new Patient profile for {request.Email} ({response.Id}).");

            return Created(Request.Path, response);
        }
    }
}
