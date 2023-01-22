namespace MABS.Application.Services.AuthenticationServices.Common
{
    public record ProfileDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
