using MABS.API.Requests.AuthenticationRequests;
using MABS.API.Validators.FacilityValidators;

namespace MABS.API.Validators.AuthenticationValidators
{
    public class RegisterFacilityProfileRequestValidator : RegisterProfileRequestValidator<RegisterFacilityProfileRequest>
    {
        public RegisterFacilityProfileRequestValidator()
        {
            RuleFor(obj => obj.Facility).SetValidator(new CreateFacilityRequestValidator());
        }
    }
}
