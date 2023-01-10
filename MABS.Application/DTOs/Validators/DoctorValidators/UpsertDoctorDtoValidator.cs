using FluentValidation;
using MABS.Application.DTOs.DoctorDtos;

namespace MABS.Application.DTOs.Validators.DoctorValidators
{
    public class UpsertDoctorDtoValidator<T> : AbstractValidator<T> where T : UpsertDoctorDto
    {
        private const int FIRSTNAME_MIN_LENGTH = 2;
        private const int LASTNAME_MIN_LENGTH = 2;

        public UpsertDoctorDtoValidator()
        {
            RuleFor(d => d.Firstname)
                .NotEmpty()
                .WithMessage("Firstname must have value")
                .NotEqual("string")
                .WithMessage("Firstname must have value")
                .MinimumLength(FIRSTNAME_MIN_LENGTH)
                .WithMessage($"Firstname minimanl length must be {FIRSTNAME_MIN_LENGTH}")
                .Must(value => value.Any(char.IsDigit) == false)
                .WithMessage("Firstname cannot contain numbers.");

            When(d => d.Firstname != string.Empty, () =>
            {
                RuleFor(d => d.Firstname)
                    .Must(value => char.IsUpper(value[0]) == true)
                    .WithMessage("Firstname must start with capital letter.");

            });

            RuleFor(d => d.Lastname)
                .NotEmpty()
                .WithMessage("Lastname must have value.")
                .NotEqual("string")
                .WithMessage("Lastname must have value.")
                .MinimumLength(LASTNAME_MIN_LENGTH)
                .WithMessage($"Lastname minimanl length must be {LASTNAME_MIN_LENGTH}.")
                .Must(value => value.Any(char.IsDigit) == false)
                .WithMessage("Lastname cannot contain numbers.");

            When(d => d.Lastname != string.Empty, () =>
            {
                RuleFor(d => d.Lastname)
                    .Must(value => char.IsUpper(value[0]) == true)
                    .WithMessage("Lastname must start with capital letter.");

            });

            RuleFor(d => d.TitleId)
                .NotEmpty()
                .WithMessage("TitleId must have value.")
                .NotEqual(0)
                .WithMessage("TitleId must have value greater then 0.");

            RuleForEach(t => t.Specialties)
                .NotEmpty()
                .WithMessage("Specialties must have value .")
                .NotEqual(0)
                .WithMessage("Specialties must have value greater then 0.");
        }
    }
}