using FluentValidation;
using MABS.Application.DTOs.ProfileDtos;
using MABS.Application.DTOs.Validators.DoctorValidators;

namespace MABS.Application.DTOs.Validators.ProfileValidators
{
    public class RegisterDoctorProfileDtoValidator : RegisterProfileDtoValidator<RegisterDoctorProfileDto>
    {
        public RegisterDoctorProfileDtoValidator()
        {
            RuleFor(obj => obj.Doctor).SetValidator(new CreateDoctorDtoValidator());
        }
    }
}
