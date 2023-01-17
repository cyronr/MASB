using MABS.Application.DTOs.ProfileDtos;

namespace MABS.Application.Services.AuthenticationServices
{
    public interface IAuthenticationService
    {
        Task<ProfileDto> RegisterPatientProfile(RegisterPatientProfileDto request);
        Task<string> Login(LoginProfileDto request);
    }
}
