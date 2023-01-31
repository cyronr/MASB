using System.Text.Json;

namespace MABS.Domain.Models.DoctorModels
{
    public class Title
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public List<Doctor> Doctors { get; set; }

        public override string ToString()
        {
            var title = new
            {
                Id = Id,
                ShortName = ShortName,
                Name = Name
            };

            return JsonSerializer.Serialize(title);
        }
    }
}