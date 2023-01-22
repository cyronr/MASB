using FluentValidation;
using MASB.API.Requests.AuthenticationRequests;

namespace MABS.API.Validators.AuthenticationValidators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(obj => obj.Email)
                .NotEmpty()
                .WithMessage("{PropertyName} must have value")
                .NotEqual("string")
                .WithMessage("{PropertyName} must have value");

            RuleFor(obj => obj.Password)
                .NotEmpty()
                .WithMessage("{PropertyName} must have value")
                .NotEqual("string")
                .WithMessage("{PropertyName} must have value");
        }
    }
}
