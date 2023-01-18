using MABS.Domain.Models.PatientModels;
using MABS.Domain.Models.ProfileModels;

namespace MABS.Application.CRUD.Readers.PatientReaders
{
    public interface IPatientReader : IReader<Patient>
    {
        Task<Patient> GetByProfileAsync(Profile profile);
    }
}
