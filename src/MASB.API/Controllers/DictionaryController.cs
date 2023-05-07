using AutoMapper;
using MABS.Application.Features.DictionaryFeatures.Queries.GetAllCities;
using MASB.API.Responses.DictionaryResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MASB.API.Controllers
{
    [ApiController]
    [Route("api/dict")]
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
    public class DictionaryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DictionaryController> _logger;
        private readonly IMapper _mapper;

        public DictionaryController(IMediator mediator, ILogger<DictionaryController> logger, IMapper mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
        }


        [HttpGet("cities")]
        [SwaggerOperation(
            Summary = "Pobierz listę miast",
            Description = "Zwraca listę wszystkich miast dostępnych w systemie."
        )]
        public async Task<ActionResult<List<CityResponse>>> GetCities()
        {
            _logger.LogInformation("Fetching all cities.");

            var command = new GetAllCitiesQuery();
            var response = await _mediator.Send(command);

            _logger.LogInformation($"Returning {response.Count} cities.");

            return Ok(response.Select(t => _mapper.Map<CityResponse>(t)).ToList());
        }
    }
}
