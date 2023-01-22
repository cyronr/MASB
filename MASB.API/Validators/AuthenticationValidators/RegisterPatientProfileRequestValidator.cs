using MABS.API.Requests.AuthenticationRequests;

namespace MABS.API.Validators.AuthenticationValidators
{
    public class RegisterPatientProfileRequestValidator : RegisterProfileRequestValidator<RegisterPatientProfileRequest>
    {
        public RegisterPatientProfileRequestValidator()
        {
            //RuleFor(obj => obj.Patient).SetValidator(new CreatePatientDtoValidator());
        }
    }
}
