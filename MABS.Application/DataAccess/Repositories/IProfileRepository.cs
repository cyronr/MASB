using MABS.Domain.Models.ProfileModels;

namespace MABS.Application.DataAccess.Repositories
{
    public interface IProfileRepository
    {
        Profile? GetByUUID(Guid uuid);
        Task<Profile?> GetByUUIDAsync(Guid uuid);
        Task<Profile?> GetByEmailAsync(string email);
        void Create(Profile profile);
        void CreateEvent(ProfileEvent profileEvent);


        Task<List<ProfileType>> GetAllTypesAsync();
    }
}
