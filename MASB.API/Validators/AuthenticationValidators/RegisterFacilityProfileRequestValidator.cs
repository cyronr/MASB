using MABS.API.Requests.AuthenticationRequests;

namespace MABS.API.Validators.AuthenticationValidators
{
    public class RegisterFacilityProfileRequestValidator : RegisterProfileRequestValidator<RegisterFacilityProfileRequest>
    {
        public RegisterFacilityProfileRequestValidator()
        {
            //RuleFor(obj => obj.Facility).SetValidator(new CreateFacilityDtoValidator());
        }
    }
}
