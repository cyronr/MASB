using MABS.Domain.Models.PatientModels;
using MABS.Domain.Models.ProfileModels;

namespace MABS.Application.Services.Helpers.PatientHelpers
{
    public interface IPatientHelper
    {
        Task<Patient> GetPatientByUUID(Guid uuid);
        Task<Patient> GetPatientByProfile(Profile profile);
    }
}
