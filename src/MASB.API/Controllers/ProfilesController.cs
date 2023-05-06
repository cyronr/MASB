using MABS.Application.Features.ProfileFeatures.Commands.DeleteProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MASB.API.Controllers;

[ApiController]
[Route("api/profiles")]
[Produces("application/json")]
[Consumes("application/json")]
public class ProfilesController : ControllerBase
{
    private readonly ILogger<ProfilesController> _logger;
    private readonly IMediator _mediator;
    public ProfilesController(ILogger<ProfilesController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProfile(Guid id)
    {
        _logger.LogInformation($"Deleting profile with id = {id}.");

        var command = new DeleteProfileCommand(id);
        await _mediator.Send(command);

        _logger.LogInformation($"Deleted profile with Id = {id}.");

        return NoContent();
    }
}
