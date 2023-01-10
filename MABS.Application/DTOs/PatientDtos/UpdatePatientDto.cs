namespace MABS.Application.DTOs.PatientDtos
{
    public class UpdatePatientDto : UpsertPatientDto
    {
        public Guid Id { get; set; }
    }
}
