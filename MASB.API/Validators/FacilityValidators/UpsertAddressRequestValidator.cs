using FluentValidation;
using MABS.API.Requests.FacilityRequests;
using System.Text.RegularExpressions;

namespace MABS.API.Validators.FacilityValidators
{
    public class UpsertAddressRequestValidator<T> : AbstractValidator<T> where T : UpsertAddressRequest
    {
        const int STREETNAME_MIN_LENGTH = 3;

        public UpsertAddressRequestValidator()
        {
            RuleFor(obj => obj.Name)
                .NotEmpty()
                .WithMessage("Name must have value")
                .NotEqual("string")
                .WithMessage("Name must have value");

            RuleFor(obj => obj.StreetName)
                .NotEmpty()
                .WithMessage("StreetName must have value")
                .NotEqual("string")
                .WithMessage("StreetName must have value")
                .MinimumLength(STREETNAME_MIN_LENGTH)
                .WithMessage($"StreetName minimanl length must be {STREETNAME_MIN_LENGTH}");

            When(obj => obj.StreetName != string.Empty, () =>
            {
                RuleFor(obj => obj.StreetName)
                    .Must(value => char.IsUpper(value[0]) == true)
                    .WithMessage("StreetName must start with capital letter.");

            });

            RuleFor(obj => obj.HouseNumber)
                .NotEmpty()
                .WithMessage("HouseNumber must have value.")
                .GreaterThan(0)
                .WithMessage("HouseNumber must be greater than 0.");

            When(obj => obj.FlatNumber is not null, () =>
            {
                RuleFor(obj => obj.FlatNumber)
                    .GreaterThan(0)
                    .WithMessage("FlatNumber must be greater than 0.");

            });

            RuleFor(obj => obj.PostalCode)
                .NotEmpty()
                .WithMessage("PostalCode must have value.")
                .NotEqual("string")
                .WithMessage("PostalCode must have value")
                .Must(IsPostalCodeValid)
                .WithMessage("PostalCode must have pattern XX-XXX where X is number between 0 and 9.");

            RuleFor(obj => obj.CountryId)
                .NotEmpty()
                .WithMessage("CountryId must have value.")
                .NotEqual("string")
                .WithMessage("CountryId must have value")
                .Must(value => value.Any(char.IsDigit) == false)
                .WithMessage("CountryId cannot contain numbers.")
                .Length(2)
                .WithMessage("CountryId must have be 2 letters.");
        }

        private bool IsPostalCodeValid(string postalCode)
        {
            var pattern = new Regex("[0-9]{2}-[0-9]{3}$");
            if (pattern.IsMatch(postalCode))
                return true;

            return false;
        }
    }
}