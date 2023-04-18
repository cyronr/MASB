using AutoMapper;
using MABS.Application.Features.DictionaryFeatures.Queries.GetAllCities;
using MASB.API.Responses.DictionaryResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MASB.API.Controllers
{
    [ApiController]
    [Route("api/dict")]
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
