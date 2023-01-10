using FluentValidation;
using MABS.Application.DTOs.ProfileDtos;

namespace MABS.Application.DTOs.Validators.ProfileValidators
{
    public class LoginProfileDtoValidator : AbstractValidator<LoginProfileDto>
    {
        public LoginProfileDtoValidator()
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
