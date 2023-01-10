using MABS.Application.Services.Helpers.DoctorHelpers;
using MABS.Application.Services.Helpers.FacilityHelpers;
using MABS.Application.Services.Helpers.PatientHelpers;
using MABS.Application.Services.Helpers.ProfileHelpers;

namespace MABS.Application.Services.Helpers
{
    public interface IHelpers
    {
        public IDoctorHelper Doctor { get; }
        public IFacilityHelper Facility { get; }
        public IPatientHelper Patient { get; }
        public IProfileHelper Profile { get; }
    }

    public class Helpers : IHelpers
    {
        public IDoctorHelper Doctor { get; }
        public IFacilityHelper Facility { get; }
        public IPatientHelper Patient { get; }
        public IProfileHelper Profile { get; }

        public Helpers(
            IDoctorHelper Doctor,
            IFacilityHelper Facility,
            IPatientHelper Patient,
            IProfileHelper Profile
            )
        {
            this.Doctor = Doctor;
            this.Facility = Facility;
            this.Patient = Patient;
            this.Profile = Profile;
        }
    }
}
