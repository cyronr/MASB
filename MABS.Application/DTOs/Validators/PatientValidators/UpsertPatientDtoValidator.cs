using FluentValidation;
using MABS.Application.DTOs.PatientDtos;

namespace MABS.Application.DTOs.Validators.PatientValidators
{
    public class UpsertPatientDtoValidator<T> : AbstractValidator<T> where T : UpsertPatientDto
    {
        private const int FIRSTNAME_MIN_LENGTH = 2;
        private const int LASTNAME_MIN_LENGTH = 2;

        public UpsertPatientDtoValidator()
        {
            RuleFor(p => p.Firstname)
            .NotEmpty()
            .WithMessage("Firstname must have value")
            .NotEqual("string")
            .WithMessage("Firstname must have value")
            .MinimumLength(FIRSTNAME_MIN_LENGTH)
            .WithMessage($"Firstname minimanl length must be {FIRSTNAME_MIN_LENGTH}")
            .Must(value => !value.Any(char.IsDigit))
            .WithMessage("Firstname cannot contain numbers.");

            When(p => p.Firstname != string.Empty, () =>
            {
                RuleFor(p => p.Firstname)
                    .Must(value => char.IsUpper(value[0]))
                    .WithMessage("Firstname must start with capital letter.");

            });

            RuleFor(p => p.Lastname)
                .NotEmpty()
                .WithMessage("Lastname must have value.")
                .NotEqual("string")
                .WithMessage("Lastname must have value.")
                .MinimumLength(LASTNAME_MIN_LENGTH)
                .WithMessage($"Lastname minimanl length must be {LASTNAME_MIN_LENGTH}.")
                .Must(value => !value.Any(char.IsDigit))
                .WithMessage("Lastname cannot contain numbers.");

            When(p => p.Lastname != string.Empty, () =>
            {
                RuleFor(p => p.Lastname)
                    .Must(value => char.IsUpper(value[0]))
                    .WithMessage("Lastname must start with capital letter.");

            });
        }
    }
}
