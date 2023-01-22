using FluentValidation;
using MASB.API.Requests.DoctorRequests;

namespace MABS.API.Validators.DoctorValidators
{
    public class UpdateDoctorRequestValidator : UpsertDoctorRequestValidator<UpdateDoctorRequest>
    {
        public UpdateDoctorRequestValidator()
        {
            RuleFor(d => d.Id)
                .NotEmpty()
                .WithMessage("Id must have value");
        }
    }
}