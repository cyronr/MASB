namespace MABS.Application.DTOs.DoctorDtos
{
    public class UpsertDoctorDto
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int TitleId { get; set; }
        public List<int> Specialties { get; set; }
    }
}
