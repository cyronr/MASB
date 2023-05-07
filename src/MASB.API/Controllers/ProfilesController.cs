using MABS.Application.Features.ProfileFeatures.Commands.DeleteProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MASB.API.Controllers;

[ApiController]
[Route("api/profiles")]
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
    [SwaggerOperation(
        Summary = "Usuń profil",
        Description = "Usuwa profil użytkownika na podstawie podanego Id (UUID). Wymaga autoryzacji."
    )]
    public async Task<ActionResult> DeleteProfile(Guid id)
    {
        _logger.LogInformation($"Deleting profile with id = {id}.");

        var command = new DeleteProfileCommand(id);
        await _mediator.Send(command);

        _logger.LogInformation($"Deleted profile with Id = {id}.");

        return NoContent();
    }
}
