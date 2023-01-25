using System.Text.Json;

namespace MABS.API.Requests.PatientRequests
{
    public record UpsertPatientRequest
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
