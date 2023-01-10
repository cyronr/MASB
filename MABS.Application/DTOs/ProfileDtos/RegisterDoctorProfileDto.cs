using MABS.Application.DTOs.DoctorDtos;

namespace MABS.Application.DTOs.ProfileDtos
{
    public class RegisterDoctorProfileDto : RegisterProfileDto
    {
        public CreateDoctorDto Doctor { get; set; }
    }
}
