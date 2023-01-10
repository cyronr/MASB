using MABS.Application.DTOs.FacilityDtos;

namespace MABS.Application.DTOs.Validators.FacilityValidators
{
    public class CreateFacilityDtoValidator : UpsertFacilityDtoValidator<CreateFacilityDto>
    {
        public CreateFacilityDtoValidator()
        {
            RuleFor(obj => obj.Address).SetValidator(new CreateAddressDtoValidator());
        }
    }
}