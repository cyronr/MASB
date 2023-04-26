using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.PatientModels;

namespace MABS.Application.ModelsExtensions.PatientModelsExtensions
{
    public static class PatientExtensions
    {
        public static async Task<Patient> GetByUUIDAsync(this Patient patient, IPatientRepository patientRepository, Guid uuid)
        {
            patient = await patientRepository.GetByUUIDAsync(uuid);
            if (patient is null)
                throw new NotFoundException($"Patient not found.", $"PatientId = {uuid}");

            return patient;
        }
    }
}
