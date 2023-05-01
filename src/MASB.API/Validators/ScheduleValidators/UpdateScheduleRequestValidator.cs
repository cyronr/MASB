using FluentValidation;
using MABS.API.Requests.ScheduleDetails;

namespace MABS.API.Validators.ScheduleValidators
{
    public class UpdateScheduleRequestValidator: AbstractValidator<UpdateScheduleRequest>
    {
        public UpdateScheduleRequestValidator()
        {
            RuleFor(p => p.DoctorId)
                .NotEmpty()
                .WithMessage("Brak identyfikatora lekarza.");

            RuleFor(p => p.FacilityId)
                .NotEmpty()
                .WithMessage("Brak identyfikatora placówki.");

            RuleFor(obj => obj.Schedules)
                .NotNull()
                .WithMessage("Brak informacji o harmonogramach.")
                .NotEmpty()
                .WithMessage("Brak informacji o harmonogramach.")
                .ForEach(schedule =>
                {
                    schedule.SetValidator(new ScheduleDetailsRequestValidator());
                });
        }
    }
}
