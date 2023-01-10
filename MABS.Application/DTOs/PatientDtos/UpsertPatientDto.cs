using System.Text.Json;

namespace MABS.Application.DTOs.PatientDtos
{
    public class UpsertPatientDto
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
