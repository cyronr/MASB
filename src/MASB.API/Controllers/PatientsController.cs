using AutoMapper;
using MABS.API.Requests.PatientRequests;
using MABS.Application.Features.PatientFeatures.Commands.UpdatePatient;
using MABS.Application.Features.PatientFeatures.Queries.GetPatientByProfile;
using MASB.API.Responses.PatientResponses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MASB.API.Controllers
{
    [ApiController]
    [Route("api/patients")]
    public class PatientsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PatientsController> _logger;
        private readonly IMapper _mapper;

        public PatientsController(IMediator mediator, ILogger<PatientsController> logger, IMapper mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
        }


        [Authorize]
        [HttpGet("byProfile/{profileId}")]
        public async Task<ActionResult<PatientResponse>> GetByProfileId(Guid profileId)
        {
            _logger.LogInformation($"Fetching patient by profile of Id = {profileId}.");

            var query = new GetPatientByProfileQuery(profileId);
            var response = await _mediator.Send(query);

            _logger.LogInformation($"Returning patient of Id = {response.Id}.");

            return Ok(_mapper.Map<PatientResponse>(response));
        }


        [Authorize]
        [HttpPut]
        public async Task<ActionResult<PatientResponse>> Update(UpdatePatientRequest request)
        {
            _logger.LogInformation($"Updating Patient with data = {request.ToString()}.");

            var command = _mapper.Map<UpdatePatientCommand>(request);
            var response = await _mediator.Send(command);

            _logger.LogInformation($"Updated Patient of Id = {response.Id}.");

            return Ok(_mapper.Map<PatientResponse>(response));
        }
    }
}
