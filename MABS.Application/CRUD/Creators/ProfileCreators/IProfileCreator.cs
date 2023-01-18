using MABS.Domain.Models.ProfileModels;

namespace MABS.Application.CRUD.Creators.ProfileCreators
{
    public interface IProfileCreator : ICreator<Profile, ProfileEventType.Type>
    {
    }
}
