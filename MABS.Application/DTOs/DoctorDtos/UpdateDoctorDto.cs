using System.Text.Json;

namespace MABS.Application.DTOs.DoctorDtos
{
    public class UpdateDoctorDto : UpsertDoctorDto
    {
        public Guid Id { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}