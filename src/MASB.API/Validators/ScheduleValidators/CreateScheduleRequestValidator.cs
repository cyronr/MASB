﻿using FluentValidation;
using MABS.API.Requests.ScheduleRequests;

namespace MABS.API.Validators.ScheduleValidators
{
    public class CreateScheduleRequestValidator: AbstractValidator<CreateScheduleRequest>
    {
        public CreateScheduleRequestValidator()
        {
            RuleFor(p => p.DoctorId)
                .NotEmpty()
                .WithMessage("Brak identyfikatora lekarza.");

            RuleFor(p => p.AddressId)
                .NotEmpty()
                .WithMessage("Brak identyfikatora adresu.");
        }
    }
}
