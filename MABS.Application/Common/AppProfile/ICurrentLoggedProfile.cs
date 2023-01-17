using MABS.Domain.Models.ProfileModels;

namespace MABS.Application.Common.AppProfile
{
    public interface ICurrentLoggedProfile
    {
        Profile? GetCurrentLoggedProfile();
    }
}
