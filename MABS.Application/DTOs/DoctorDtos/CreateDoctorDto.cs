using System.Text.Json;

namespace MABS.Application.DTOs.DoctorDtos
{
    public class CreateDoctorDto : UpsertDoctorDto
    {
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}