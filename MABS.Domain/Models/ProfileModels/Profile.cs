using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.PatientModels;
using System.Text.Json;

namespace MABS.Domain.Models.ProfileModels
{
    public class Profile
    {
        public int Id { get; set; }
        public Guid UUID { get; set; }
        public ProfileStatus.Status StatusId { get; set; }
        public ProfileStatus Status { get; set; }
        public ProfileType.Type TypeId { get; set; }
        public ProfileType Type { get; set; }
        public string Email { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public string? PhoneNumber { get; set; }
        public List<ProfileEvent> Events { get; set; }
        public Doctor Doctor { get; set; }
        public Facility Facility { get; set; }
        public Patient Patient { get; set; }
        public List<DoctorEvent> DoctorEvents { get; set; }
        public List<FacilityEvent> FacilityEvents { get; set; }
        public List<PatientEvent> PatientEvents { get; set; }
        public List<ProfileEvent> CallerProfileEvents { get; set; }


        public override string ToString()
        {
            var profile = new
            {
                Id = UUID,
                Type = TypeId,
                PhoneNumber = PhoneNumber
            };

            return JsonSerializer.Serialize(profile);
        }

    }
}