using MABS.Domain.Models.ProfileModels;

namespace MABS.Application.Repositories
{
    public interface ICurrentLoggedProfile
    {
        Profile GetByUUID(Guid uuid);
    }
}
