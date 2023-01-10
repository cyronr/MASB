using MABS.Application.DTOs.ProfileDtos;
using MABS.Application.DTOs.Validators.PatientValidators;

namespace MABS.Application.DTOs.Validators.ProfileValidators
{
    public class RegisterPatientProfileDtoValidator : RegisterProfileDtoValidator<RegisterPatientProfileDto>
    {
        public RegisterPatientProfileDtoValidator()
        {
            RuleFor(obj => obj.Patient).SetValidator(new CreatePatientDtoValidator());
        }
    }
}
