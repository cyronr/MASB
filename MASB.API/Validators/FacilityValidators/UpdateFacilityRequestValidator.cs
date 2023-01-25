using FluentValidation;
using MABS.API.Requests.FacilityRequests;

namespace MABS.API.Validators.FacilityValidators
{
    public class UpdateFacilityRequestValidator : UpsertFacilityRequestValidator<UpdateFacilityRequest>
    {
        public UpdateFacilityRequestValidator()
        {
            RuleFor(d => d.Id)
                .NotEmpty()
                .WithMessage("Id must have value");
        }
    }
}