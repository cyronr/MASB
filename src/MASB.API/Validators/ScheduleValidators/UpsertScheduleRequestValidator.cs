using FluentValidation;
using MABS.API.Requests.ScheduleRequests;

namespace MABS.API.Validators.ScheduleValidators;

public class UpsertScheduleRequestValidator<T> : AbstractValidator<T> where T : UpsertScheduleRequest
{
    public UpsertScheduleRequestValidator()
    {
        RuleFor(p => p.DayOfWeek)
            .NotNull()
            .WithMessage("Nie podano wybranego dnia tygodnia.")
            .IsInEnum()
            .WithMessage("Nieprawidłowy dzień tygodnia.");

        RuleFor(p => p.AppointmentDuration)
            .NotEmpty()
            .WithMessage("Nie podano czasu trwania wizyty.")
            .InclusiveBetween(1, 60)
            .WithMessage("Czas trawnia wizyty nie może być mniejszy niż 0 ani dłuższy niż 60 minut.");

        RuleFor(p => p.ValidDateFrom)
            .NotEmpty()
            .WithMessage("Nie podano daty ważności.")
            .GreaterThanOrEqualTo(new DateOnly(DateTime.Now.Date.Year, DateTime.Now.Date.Month, DateTime.Now.Date.Day))
            .WithMessage("Data ważności nie może być mniejsza niż aktualna data");

        RuleFor(p => p.ValidDateTo)
            .NotEmpty()
            .WithMessage("Nie podano daty ważności.")
            .GreaterThanOrEqualTo(r => r.ValidDateFrom)
            .WithMessage("Data ważności Do musi być większa od daty ważności Od.");

    }
}
