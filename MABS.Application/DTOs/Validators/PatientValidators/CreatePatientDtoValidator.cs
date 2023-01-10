using MABS.Application.DTOs.PatientDtos;

namespace MABS.Application.DTOs.Validators.PatientValidators
{
    public class CreatePatientDtoValidator : UpsertPatientDtoValidator<CreatePatientDto>
    {
        public CreatePatientDtoValidator() { }
    }
}
