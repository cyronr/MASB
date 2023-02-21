namespace MABS.Application.Features.AuthenticationFeatures.Common
{
    public record AuthenticationResultDto
    (
        ProfileDto Profile,
        string Token
    );
}
