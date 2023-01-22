using MASB.API.Requests.DoctorRequests;

namespace MABS.API.Requests.AuthenticationRequests
{
    public record RegisterDoctorProfileRequest : RegisterProfileRequest
    {
        public CreateDoctorRequest Doctor { get; set; }
    }
}
