using FluentValidation;
using MABS.API.Requests.FacilityRequests;

namespace MABS.API.Validators.FacilityValidators
{
    public class UpdateAddressRequestValidator : UpsertAddressRequestValidator<UpdateAddressRequest>
    {
        public UpdateAddressRequestValidator()
        {
            RuleFor(d => d.Id)
                .NotEmpty()
                .WithMessage("Id must have value");
        }
    }
}