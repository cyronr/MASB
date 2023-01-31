using MABS.Domain.Models.ProfileModels;

namespace MABS.Application.Common.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Profile profile);
    }
}
