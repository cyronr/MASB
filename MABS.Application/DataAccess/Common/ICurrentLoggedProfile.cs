using MABS.Domain.Models.ProfileModels;

namespace MABS.Application.DataAccess.Common
{
    public interface ICurrentLoggedProfile
    {
        Profile GetByUUID(Guid uuid);
    }
}
