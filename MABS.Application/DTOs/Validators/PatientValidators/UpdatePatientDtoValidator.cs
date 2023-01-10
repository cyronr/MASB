using FluentValidation;
using MABS.Application.DTOs.PatientDtos;

namespace MABS.Application.DTOs.Validators.PatientValidators
{
    public class UpdatePatientDtoValidator : UpsertPatientDtoValidator<UpdatePatientDto>
    {
        public UpdatePatientDtoValidator()
        {
            RuleFor(d => d.Id)
               .NotEmpty()
               .WithMessage("Id must have value");
        }
    }
}
