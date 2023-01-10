using MABS.Domain.Models.PatientModels;
using MABS.Domain.Models.ProfileModels;

namespace MABS.Application.Repositories
{
    public interface IPatientRepository
    {
        Task<Patient> GetByUUID(Guid uuid);
        Task<Patient> GetByProfile(Profile profile);
        void Create(Patient patient);
        void CreateEvent(PatientEvent patientEvent);
    }
}
