using FluentValidation;
using MABS.Application.DTOs.FacilityDtos;
using MABS.Domain.Extensions;

namespace MABS.Application.DTOs.Validators.FacilityValidators
{
    public class UpsertFacilityDtoValidator<T> : AbstractValidator<T> where T : UpsertFacilityDto
    {
        public UpsertFacilityDtoValidator()
        {
            RuleFor(obj => obj.ShortName)
                .NotEmpty()
                .WithMessage("{PropertyName} must have value")
                .NotEqual("string")
                .WithMessage("{PropertyName} must have value");

            RuleFor(obj => obj.Name)
                .NotEmpty()
                .WithMessage("{PropertyName} must have value")
                .NotEqual("string")
                .WithMessage("{PropertyName} must have value");

            RuleFor(obj => obj.TaxIdentificationNumber)
                .NotEmpty()
                .WithMessage("{PropertyName} must have value.")
                .NotEqual("string")
                .WithMessage("{PropertyName} must have value")
                .Must(value => value.All(char.IsDigit))
                .WithMessage("{PropertyName} must contain only digits.")
                .Length(10)
                .WithMessage("{PropertyName} must have 10 digits.")
                .Must(IsNIPValid)
                .WithMessage("{PropertyName} is not valid NIP.");
        }

        private bool IsNIPValid(string nip)
        {
            if (nip.Length != 10)
                return false;

            int sum = 0;
            sum += nip[0].ToInt32() * 6;
            sum += nip[1].ToInt32() * 5;
            sum += nip[2].ToInt32() * 7;
            sum += nip[3].ToInt32() * 2;
            sum += nip[4].ToInt32() * 3;
            sum += nip[5].ToInt32() * 4;
            sum += nip[6].ToInt32() * 5;
            sum += nip[7].ToInt32() * 6;
            sum += nip[8].ToInt32() * 7;

            if (sum % 11 == nip[9].ToInt32())
                return true;

            return false;
        }
    }
}