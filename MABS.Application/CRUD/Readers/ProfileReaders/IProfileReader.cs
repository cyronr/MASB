using MABS.Domain.Models.ProfileModels;

namespace MABS.Application.CRUD.Readers.ProfileReaders
{
    public interface IProfileReader : IReader<Profile>
    {
        Task<Profile> GetByEmailAsync(string email);
    }
}
