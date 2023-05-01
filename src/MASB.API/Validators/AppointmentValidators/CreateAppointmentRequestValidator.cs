using FluentValidation;
using MASB.API.Requests.AppointmentRequests;

namespace MASB.API.Validators.AppointmentValidators;

public class CreateAppointmentRequestValidator : AbstractValidator<CreateAppointmentRequest>
{
	public CreateAppointmentRequestValidator()
	{
		RuleFor(p => p.PatientId)
			.NotEmpty()
			.WithMessage("Nie podano identyfikatora pacjneta.");

        RuleFor(p => p.ScheduleId)
            .NotEmpty()
            .WithMessage("Nie podano identyfikatora harmanogoramu.");

        RuleFor(p => p.Date)
            .NotEmpty()
            .WithMessage("Nie podano daty.");

        RuleFor(p => p.Time)
            .NotEmpty()
            .WithMessage("Nie podano czasu.");
    }
}
