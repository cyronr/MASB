using MABS.Application.DTOs.ProfileDtos;
using MABS.Application.DTOs.Validators.FacilityValidators;

namespace MABS.Application.DTOs.Validators.ProfileValidators
{
    public class RegisterFacilityProfileDtoValidator : RegisterProfileDtoValidator<RegisterFacilityProfileDto>
    {
        public RegisterFacilityProfileDtoValidator()
        {
            RuleFor(obj => obj.Facility).SetValidator(new CreateFacilityDtoValidator());
        }
    }
}
