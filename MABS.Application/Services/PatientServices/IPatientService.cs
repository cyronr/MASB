using MABS.Application.DTOs.PatientDtos;

namespace MABS.Application.Services.PatientServices
{
    public interface IPatientService
    {
        Task<PatientDto> GetById(Guid id);
        Task<PatientDto> Create(CreatePatientDto request);
        Task<PatientDto> Update(UpdatePatientDto request);
        Task Delete(Guid id);
    }
}
