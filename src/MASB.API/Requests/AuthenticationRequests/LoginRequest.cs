namespace MASB.API.Requests.AuthenticationRequests
{
    public record LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
