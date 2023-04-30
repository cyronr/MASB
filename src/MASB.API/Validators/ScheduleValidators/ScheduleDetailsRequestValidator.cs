using FluentValidation;
using MABS.API.Requests.ScheduleDetails;

namespace MABS.API.Validators.ScheduleValidators
{
    public class ScheduleDetailsRequestValidator<T> : AbstractValidator<ScheduleDetailsRequest>
    {
        public ScheduleDetailsRequestValidator()
        {
            RuleFor(p => p.DayOfWeek)
                .NotEmpty()
                .WithMessage("Brak wybranego dnia tygodnia.");

            RuleFor(p => p.FacilityId)
                .NotEmpty()
                .WithMessage("Brak identyfikatora placówki.");
        }
    }
}
