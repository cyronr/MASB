using FluentValidation;
using MABS.Application.DTOs.FacilityDtos;

namespace MABS.Application.DTOs.Validators.FacilityValidators
{
    public class UpdateAddressDtoValidator : UpsertAddressDtoValidator<UpdateAddressDto>
    {
        public UpdateAddressDtoValidator()
        {
            RuleFor(d => d.Id)
                .NotEmpty()
                .WithMessage("Id must have value");
        }
    }
}