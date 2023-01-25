using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.DoctorModels;

namespace MABS.Application.ModelsExtensions.DoctorModelsExtensions
{
    public static class TitleExtensions
    {
        public static async Task<Title> GetByIdAsync(this Title title, IDoctorRepository doctorRepository, int id)
        {
            title = await doctorRepository.GetTitleByIdAsync(id);
            if (title is null)
                throw new DictionaryValueNotExistsException("Wrong TitleId.");

            return title;
        }
    }
}
