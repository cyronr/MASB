namespace MABS.Application.Services.AuthenticationServices.Common
{
    public record AuthenticationResultDto
    (
        ProfileDto Profile,
        string Token
    );
}
