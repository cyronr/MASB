using FluentValidation;
using MABS.Application.DTOs.DoctorDtos;

namespace MABS.Application.DTOs.Validators.DoctorValidators
{
    public class UpdateDoctorDtoValidator : UpsertDoctorDtoValidator<UpdateDoctorDto>
    {
        public UpdateDoctorDtoValidator()
        {
            RuleFor(d => d.Id)
                .NotEmpty()
                .WithMessage("Id must have value");
        }
    }
}