using FluentValidation;
using MABS.API.Requests.ScheduleRequests;

namespace MABS.API.Validators.ScheduleValidators
{
    public class UpdateScheduleRequestValidator : AbstractValidator<UpdateScheduleRequest>
    {
        public UpdateScheduleRequestValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty()
                .WithMessage("Brak identyfikatora harmonogramu.");
        }
    }
}
