using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.ProfileModels;
using MABS.Domain.Models.ScheduleModels;
using System.Text.Json;

namespace MABS.Domain.Models.DoctorModels
{
    public class Doctor
    {
        public long Id { get; set; }
        public Guid UUID { get; set; }
        public DoctorStatus.Status StatusId { get; set; }
        public DoctorStatus Status { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Title Title { get; set; } = new Title();
        public List<Specialty> Specialties { get; set; } = new List<Specialty>();
        public List<DoctorEvent> Events { get; set; }
        public List<Facility> Facilities { get; set; }
        public Profile Profile { get; set; }
        public int? ProfileId { get; set; }
        public List<Schedule> Schedules { get; set; }

        public override string ToString()
        {
            var specalties = new List<string>();
            foreach (var specialty in Specialties) 
            {
                specalties.Add(specialty.ToString());
            }

            var doctor = new
            {
                Id = UUID,
                Firstname = Firstname,
                Lastname = Lastname,
                Title = Title.ToString(),
                Specialties = JsonSerializer.Serialize(specalties)
            };

            return JsonSerializer.Serialize(doctor);
        }
        
    }
}