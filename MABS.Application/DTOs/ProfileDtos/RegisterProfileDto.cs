namespace MABS.Application.DTOs.ProfileDtos
{
    public class RegisterProfileDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatedPassword { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
