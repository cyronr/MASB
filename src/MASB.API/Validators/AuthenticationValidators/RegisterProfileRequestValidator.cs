﻿using System.Text.RegularExpressions;
using FluentValidation;
using MABS.API.Requests.AuthenticationRequests;
using Microsoft.IdentityModel.Tokens;

namespace MABS.API.Validators.AuthenticationValidators
{
    public abstract class RegisterProfileRequestValidator<T> : AbstractValidator<T> where T : RegisterProfileRequest
    {
        public RegisterProfileRequestValidator()
        {
            RuleFor(obj => obj.Email)
                .NotEmpty()
                .WithMessage("{PropertyName} must have value")
                .NotEqual("string")
                .WithMessage("{PropertyName} must have value")
                .EmailAddress()
                .WithMessage("{PropertyName} is not valid email");

            RuleFor(obj => obj.Password)
                .NotEmpty()
                .WithMessage("{PropertyName} must have value")
                .NotEqual("string")
                .WithMessage("{PropertyName} must have value")
                .Must(IsPasswordValid)
                .WithMessage("{PropertyName} is not valid password");

            When(obj => !obj.PhoneNumber.IsNullOrEmpty(), () =>
            {
                RuleFor(obj => obj.PhoneNumber)
                    .MinimumLength(9)
                    .WithMessage("{PropertyName} must have at least 9 digits.")
                    .Must(IsPhoneNumberValid)
                    .WithMessage("{PropertyName} is not valid phone number.");

            });
        }

        private bool IsPasswordValid(string password)
        {
            if (password is null || password.Equals(String.Empty))
                return false;
            
            ///At least 8 characters
            ///At least one uppercase
            ///At least one lowercase
            ///At least one digit
            ///At least one special character
            var passwordRules = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
            if (passwordRules.IsMatch(password))
                return true;

            return false;
        }

        private bool IsPhoneNumberValid(string phoneNumber)
        {
            if (phoneNumber is null || phoneNumber.Equals(String.Empty))
                return false;

            var phoneNumberRules = new Regex("[0-9]$");
            if (phoneNumberRules.IsMatch(phoneNumber))
                return true;

            return false;
        }
    }
}
