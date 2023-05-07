using AutoMapper;
using MABS.API.Requests.AuthenticationRequests;
using MABS.Application.Features.AuthenticationFeatures.Queries.Login;
using MASB.API.Requests.AuthenticationRequests;
using MASB.API.Requests.AuthenticationResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MABS.Application.Features.AuthenticationFeatures.Commands.RegisterPatient;
using MABS.Application.Features.PatientFeatures.Commands.CreatePatient;
using MABS.Application.Features.AuthenticationFeatures.Commands.RegisterFacility;
using MABS.Application.Features.FacilityFeatures.Commands.CreateFacility;
using Swashbuckle.AspNetCore.Annotations;

namespace MABS.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerResponse(200, "Sukces. Zwrócono odpowiedź.")]
    [SwaggerResponse(204, "Sukces. Brak odpowiedzi.")]
    [SwaggerResponse(400, "Błąd. Niepoprawny request.")]
    [SwaggerResponse(401, "Błąd. Brak autoryzacji.")]
    [SwaggerResponse(403, "Błąd. Zabroniono.")]
    [SwaggerResponse(404, "Błąd. Nie znaleziono obiektu.")]
    [SwaggerResponse(407, "Błąd. Wystąpił błąd biznesowy.")]
    [SwaggerResponse(500, "Nieoczekiwany błąd.")]
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
        [SwaggerOperation(
            Summary = "Zaloguj",
            Description = "Loguje użytkownika na podstawie przekazanego w JSON obiektu LoginRequest." +
                "Zwraca szczegóły zidentyfikowanego profilu oraz token autoryzacyjny (JWT)."
        )]
        public async Task<ActionResult<AuthenticationResponse>> Login(LoginRequest request)
        {
            _logger.LogInformation($"Logging {request.Email}.");

            var query = new LoginQuery(request.Email, request.Password);
            var response = await _mediator.Send(query);

            _logger.LogInformation($"{request.Email} logged with token {response}.");

            return Ok(_mapper.Map<AuthenticationResponse>(response));
        }

        [HttpPost("register/patient")]
        [SwaggerOperation(
            Summary = "Zarejestruj pacjenta",
            Description = "Rejestruje nowy profil pacjenta na podstawie przekazanego w JSON obiektu RegisterPatientProfileRequest." +
                "Zwraca szczegóły zidentyfikowanego profilu oraz token autoryzacyjny (JWT)."
        )]
        public async Task<ActionResult<AuthenticationResponse>> RegisterPatient(RegisterPatientProfileRequest request)
        {
            _logger.LogInformation($"Registering new patient profile {request.Email}.");

            var command = new RegisterPatientCommand(
                request.Email,
                request.Password,
                request.PhoneNumber,
                new CreatePatientCommand
                {
                    Firstname = request.Firstname,
                    Lastname = request.Lastname
                }
            );
            var response = await _mediator.Send(command);

            _logger.LogInformation($"Registered new patient profile for {request.Email} ({response.Profile.Id}).");

            return Created(Request.Path, _mapper.Map<AuthenticationResponse>(response));
        }

        [HttpPost("register/facility")]
        [SwaggerOperation(
            Summary = "Zarejestruj placówkę medyczną",
            Description = "Rejestruje nowy profil placówki medycznej na podstawie przekazanego w JSON obiektu RegisterFacilityProfileRequest." +
                "Zwraca szczegóły zidentyfikowanego profilu oraz token autoryzacyjny (JWT)."
        )]
        public async Task<ActionResult<AuthenticationResponse>> RegisterFacility(RegisterFacilityProfileRequest request)
        {
            _logger.LogInformation($"Registering new facility profile {request.Email}.");

            var command = new RegisterFacilityCommand(
                request.Email,
                request.Password,
                request.PhoneNumber,
                _mapper.Map<CreateFacilityCommand>(request.Facility)
            );
            var response = await _mediator.Send(command);

            _logger.LogInformation($"Registered new facility profile for {request.Email} ({response.Profile.Id}).");

            return Created(Request.Path, _mapper.Map<AuthenticationResponse>(response));
        }
    }
}
