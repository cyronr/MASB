using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.DoctorModels;

namespace MABS.Application.ModelsExtensions.DoctorModelsExtensions
{
    public static class SpecialtyExtensions
    {
        public static async Task<Specialty> GetByIdsAsync(this Specialty specialty, IDoctorRepository doctorRepository, int id)
        {
            specialty = await doctorRepository.GetSpecialtyByIdAsync(id);
            if (specialty is null)
                throw new DictionaryValueNotExistsException("Wrong Specailties.");

            return specialty;
        }
    }
}
