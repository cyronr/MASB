using System;
using System.Text.Json;

namespace MABS.Domain.Models.DoctorModels
{
    public class Specialty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Doctor> Doctors { get; set; }

        public override string ToString()
        {
            var speciality = new
            {
                Id = Id,
                Name = Name
            };

            return JsonSerializer.Serialize(speciality);
        }
    }
}