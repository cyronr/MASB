using MediatR;

namespace MABS.Application.Features.ProfileFeatures.Commands.DeleteProfile;

public record DeleteProfileCommand(
    Guid ProfileId
) : IRequest;
