using MABS.Domain.Models.AppointmentModels;
using MABS.Domain.Models.ProfileModels;
using System.Text.Json;

namespace MABS.Domain.Models.PatientModels
{
    public class Patient
    {
        public int Id { get; set; }
        public Guid UUID { get; set; }
        public PatientStatus.Status StatusId { get; set; }
        public PatientStatus Status { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public List<PatientEvent> Events { get; set; }
        public Profile Profile { get; set; }
        public int ProfileId { get; set; }

        public List<Appointment> Appointments { get; set; }
        public override string ToString()
        {
            var patient = new
            {
                Id = UUID,
                Firstname = Firstname,
                Lastname = Lastname
            };

            return JsonSerializer.Serialize(patient);
        }

    }
}