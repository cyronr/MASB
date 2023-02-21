using AutoMapper;
using MABS.API.Requests.AuthenticationRequests;
using MABS.Application.Features.DoctorFeatures.Commands.CreateDoctor;
using MABS.Application.Features.AuthenticationFeatures.Commands.RegisterDoctor;
using MABS.Application.Features.AuthenticationFeatures.Queries.Login;
using MASB.API.Requests.AuthenticationRequests;
using MASB.API.Requests.AuthenticationResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MABS.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthenticationController(ILogger<AuthenticationController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login(LoginRequest request)
        {
            _logger.LogInformation($"Logging {request.Email}.");

            var query = new LoginQuery(request.Email, request.Password);
            var response = await _mediator.Send(query);

            _logger.LogInformation($"{request.Email} logged with token {response}.");

            return Ok(_mapper.Map<AuthenticationResponse>(response));
        }

        [HttpPost("register/doctor")]
        public async Task<ActionResult<AuthenticationResponse>> RegisterDoctor(RegisterDoctorProfileRequest request)
        {
            _logger.LogInformation($"Registering new doctor profile {request.Email}.");

            var command = new RegisterDoctorCommand(
                request.Email,
                request.Password,
                request.PhoneNumber,
                _mapper.Map<CreateDoctorCommand>(request.Doctor)
            );
            var response = await _mediator.Send(command);

            _logger.LogInformation($"Registered new doctor profile for {request.Email} ({response.Profile.Id}).");

            return Created(Request.Path, _mapper.Map<AuthenticationResponse>(response));
        }
    }
}
