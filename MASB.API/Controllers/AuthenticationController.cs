using MABS.Application.DTOs.ProfileDtos;
using MABS.Application.Services.AuthenticationServices;
using Microsoft.AspNetCore.Mvc;

namespace MABS.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginProfileDto request)
        {
            _logger.LogInformation($"Logging {request.Email}.");
            var response = await _authenticationService.Login(request);
            _logger.LogInformation($"{request.Email} logged with token {response}.");

            return Ok(response);
        }

        [HttpPost("register/patient")]
        public async Task<ActionResult<ProfileDto>> RegisterPatient(RegisterPatientProfileDto request)
        {
            _logger.LogInformation($"Registering new Patient profile {request.Email}.");
            var response = await _authenticationService.RegisterPatientProfile(request);
            _logger.LogInformation($"Registered new Patient profile for {request.Email} ({response.Id}).");

            return Created(Request.Path, response);
        }
    }
}
