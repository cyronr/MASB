using MABS.Application.DTOs.PatientDtos;

namespace MABS.Application.DTOs.ProfileDtos
{
    public class RegisterPatientProfileDto : RegisterProfileDto
    {
        public CreatePatientDto Patient { get; set; }
    }
}
