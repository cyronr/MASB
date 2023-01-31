using MABS.Application.Services.AuthenticationServices.Common;

namespace MASB.API.Requests.AuthenticationResponses
{
    public record AuthenticationResponse
    {
        public ProfileDto Profile { get; set; }
        public string Token { get; set; }
    }
}
