using MABS.Domain.Models.PatientModels;
using MABS.Domain.Models.ProfileModels;

namespace MABS.Application.DataAccess.Repositories
{
    public interface IPatientRepository
    {
        Task<Patient?> GetByUUIDAsync(Guid uuid);
        Task<Patient?> GetByProfileAsync(Profile profile);
        void Create(Patient patient);
        void CreateEvent(PatientEvent patientEvent);
    }
}
