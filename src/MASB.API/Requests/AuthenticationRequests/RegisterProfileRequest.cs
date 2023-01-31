namespace MABS.API.Requests.AuthenticationRequests
{
    public record RegisterProfileRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatedPassword { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
