using MABS.Application.DTOs.ProfileDtos;

namespace MABS.Application.Services.ProfileServices
{
    public interface IProfileService
    {
        Task<ProfileDto> RegisterPatientProfile(RegisterPatientProfileDto request);
        Task<string> Login(LoginProfileDto request);
    }
}
