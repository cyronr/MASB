using MABS.Domain.Models.ProfileModels;

namespace MABS.Application.Common.Interfaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Profile profile);
    }
}
