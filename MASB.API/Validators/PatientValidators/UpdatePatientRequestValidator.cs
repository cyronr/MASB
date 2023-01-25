using FluentValidation;
using MABS.API.Requests.PatientRequests;

namespace MABS.API.Validators.PatientValidators
{
    public class UpdatePatientDtoValidator : UpsertPatientRequestValidator<UpdatePatientRequest>
    {
        public UpdatePatientDtoValidator()
        {
            RuleFor(d => d.Id)
               .NotEmpty()
               .WithMessage("Id must have value");
        }
    }
}
