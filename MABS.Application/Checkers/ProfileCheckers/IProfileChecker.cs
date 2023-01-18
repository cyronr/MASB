using MABS.Domain.Models.ProfileModels;

namespace MABS.Application.Checkers.ProfileCheckers
{
    public interface IProfileChecker
    {
        Task CheckProfileAlreadyExistsAsync(Profile profile);
    }
}
