using MABS.Domain.Models.ProfileModels;

namespace MABS.Application.DataAccess.Repositories
{
    public interface IProfileRepository
    {
        Task<Profile> GetByUUID(Guid uuid);
        Task<Profile> GetByEmail(string email);
        void Create(Profile profile);
        void CreateEvent(ProfileEvent profileEvent);


        Task<List<ProfileType>> GetAllTypes();
    }
}
