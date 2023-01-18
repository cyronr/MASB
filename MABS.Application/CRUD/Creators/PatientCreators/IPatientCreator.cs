using MABS.Domain.Models.PatientModels;

namespace MABS.Application.CRUD.Creators.PatientCreators
{
    public interface IPatientCreator : ICreator<Patient, PatientEventType.Type>
    {
    }
}
