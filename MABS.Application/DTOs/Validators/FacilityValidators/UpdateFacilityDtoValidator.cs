using FluentValidation;
using MABS.Application.DTOs.FacilityDtos;

namespace MABS.Application.DTOs.Validators.FacilityValidators
{
    public class UpdateFacilityDtoValidator : UpsertFacilityDtoValidator<UpdateFacilityDto>
    {
        public UpdateFacilityDtoValidator()
        {
            RuleFor(d => d.Id)
                .NotEmpty()
                .WithMessage("Id must have value");
        }
    }
}