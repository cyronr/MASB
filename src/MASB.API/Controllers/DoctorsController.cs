using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MASB.API.Requests.DoctorRequests;
using MASB.API.Responses.DoctorResponses;
using MABS.Application.Common.Pagination;
using MABS.Application.Features.DoctorFeatures.Commands.DeleteDoctor;
using MABS.Application.Features.DoctorFeatures.Queries.GetDoctorById;
using MABS.Application.Features.DoctorFeatures.Queries.GetDoctorsBySpecialties;
using MABS.Application.Features.DoctorFeatures.Queries.GetAllSpecialties;
using MABS.Application.Features.DoctorFeatures.Queries.GetAllTitles;
using MABS.Application.Features.DoctorFeatures.Queries.SearchAllDoctorsByText;
using MABS.Application.Features.DoctorFeatures.Commands.CreateDoctor;
using MABS.Application.Features.DoctorFeatures.Commands.UpdateDoctor;

namespace MABS.API.Controllers
{
    [ApiController]
    [Route("api/doctors")]
    public class DoctorsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DoctorsController> _logger;
        private readonly IMapper _mapper;

        public DoctorsController(ILogger<DoctorsController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<DoctorResponse>>> SearchAll([FromQuery] PagingParameters pagingParameters, string searchText)
        {
            _logger.LogInformation("Fetching all doctors.");

            var query = new SearchAllDoctorsByTextQuery(pagingParameters, searchText);
            var response = await _mediator.Send(query);

            _logger.LogInformation($"Returning {response.Count} doctors.");

            Response.Headers.Add("X-Pagination", response.GetMetadata());
            return Ok(response.Select(d => _mapper.Map<DoctorResponse>(d)).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorResponse>> GetById(Guid id)
        {
            _logger.LogInformation($"Fetching doctor of Id = {id}.");

            var query = new GetDoctorByIdQuery(id);
            var response = await _mediator.Send(query);

            _logger.LogInformation($"Returning doctor of Id = {id}.");

            return Ok(_mapper.Map<DoctorResponse>(response));
        }

        [HttpGet("bySpecialties")]
        public async Task<ActionResult<PagedList<DoctorResponse>>> GetBySpecialtyIds([FromQuery]List<int> ids, [FromQuery] PagingParameters pagingParameters)
        {
            _logger.LogInformation($"Fetching doctors for specalties Ids = {String.Join(", ", ids.ToArray())}.");

            var query = new GetDoctorsBySpecialtiesQuery(pagingParameters, ids);
            var response = await _mediator.Send(query);

            _logger.LogInformation($"Returning {response.Count} doctors.");

            Response.Headers.Add("X-Pagination", response.GetMetadata());
            return Ok(response.Select(d => _mapper.Map<DoctorResponse>(d)).ToList());
        }

        //[Authorize]
        [HttpPost]
        public async Task<ActionResult<DoctorResponse>> Create(CreateDoctorRequest request)
        {
            _logger.LogInformation($"Creating doctor with data = {request.ToString()}.");

            var command = _mapper.Map<CreateDoctorCommand>(request);
            var response = await _mediator.Send(command);

            _logger.LogInformation($"Created doctor with Id = {response.Id}.");

            return base.Created(Request.Path, _mapper.Map<DoctorResponse>(response));
        }

        [HttpPut]
        public async Task<ActionResult<DoctorResponse>> Update(UpdateDoctorRequest request)
        {
            _logger.LogInformation($"Updating doctor with data = {request.ToString()}.");

            var command = _mapper.Map<UpdateDoctorCommand>(request);
            var response = await _mediator.Send(command);

            _logger.LogInformation($"Updated doctor of Id = {response.Id}.");

            return base.Ok(_mapper.Map<DoctorResponse>(response));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"Deleting doctor with id = {id}.");

            var command = new DeleteDoctorCommand(id);
            await _mediator.Send(command);

            _logger.LogInformation($"Deleted doctor with id = {id}.");

            return NoContent();
        }

        [HttpGet("dictonaries/specialties")]
        public async Task<ActionResult<List<SpecialtyResponse>>> GetSpecialities()
        {
            _logger.LogInformation("Fetching all specialities.");

            var command = new GetAllSpecialtiesQuery();
            var response = await _mediator.Send(command);

            _logger.LogInformation($"Returning {response.Count} specialities.");

            return Ok(response.Select(s => _mapper.Map<SpecialtyResponse>(s)).ToList());
        }

        [HttpGet("dictonaries/titles")]
        public async Task<ActionResult<List<TitleResponse>>> GetTitles()
        {
            _logger.LogInformation("Fetching all titles.");

            var command = new GetAllTitlesQuery();
            var response = await _mediator.Send(command);

            _logger.LogInformation($"Returning {response.Count} titles.");

            return Ok(response.Select(t => _mapper.Map<TitleResponse>(t)).ToList());
        }

    }
}