namespace MABS.API.Requests.AuthenticationRequests
{
    public record RegisterPatientProfileRequest : RegisterProfileRequest
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }

    }
}
