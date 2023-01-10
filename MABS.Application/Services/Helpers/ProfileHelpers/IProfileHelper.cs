using MABS.Domain.Models.ProfileModels;

namespace MABS.Application.Services.Helpers.ProfileHelpers
{
    public interface IProfileHelper
    {
        Task<Profile> TryGetLoggedProfile();
        Task<Profile> GetByUUID(Guid uuid);
        Task<Profile> GetByEmail(string email);
        Task CheckProfileAlreadyExists(Profile profile);
    }
}
