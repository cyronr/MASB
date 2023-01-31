using MABS.API.Requests.AuthenticationRequests;
using MABS.API.Validators.DoctorValidators;

namespace MABS.API.Validators.AuthenticationValidators
{
    public class RegisterDoctorProfileRequestValidator : RegisterProfileRequestValidator<RegisterDoctorProfileRequest>
    {
        public RegisterDoctorProfileRequestValidator()
        {
            RuleFor(obj => obj.Doctor).SetValidator(new CreateDoctorRequestValidator());
        }
    }
}
