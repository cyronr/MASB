using MABS.Domain.Models.AppointmentModels;
using MABS.Domain.Models.DictionaryModels;
using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.PatientModels;
using MABS.Domain.Models.ProfileModels;
using MABS.Domain.Models.ScheduleModels;
using MABS.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MABS.Infrastructure.Data
{
    public class DataContext : DbContext
    {
        private readonly ConnectionStrings _connectionStrings;

        public DataContext(DbContextOptions<DataContext> options, IOptions<ConnectionStrings> connectionOptions) : base(options)
        {
            _connectionStrings = connectionOptions.Value;
        }

        //Dictionaries
        public DbSet<City> Cities { get; set; }

        //Doctors
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<DoctorStatus> DoctorStatus { get; set; }
        public DbSet<DoctorEvent> DoctorEvents { get; set; }
        public DbSet<DoctorEventType> DoctorEventType { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<Specialty> Specialties { get; set; }

        //Facilites
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<FacilityStatus> FacilityStatus { get; set; }
        public DbSet<FacilityEvent> FacilityEvents { get; set; }
        public DbSet<FacilityEventType> FacilityEventType { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressStatus> AddressStatus { get; set; }
        public DbSet<AddressStreetType> AddressStreetType { get; set; }
        public DbSet<Country> Countries { get; set; }

        //Patients
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientStatus> PatientStatus { get; set; }
        public DbSet<PatientEvent> PatientEvents { get; set; }
        public DbSet<PatientEventType> PatientEventType { get; set; }

        //Profiles
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProfileStatus> ProfileStatus { get; set; }
        public DbSet<ProfileType> ProfileType { get; set; }
        public DbSet<ProfileEvent> ProfileEvents { get; set; }
        public DbSet<ProfileEventType> ProfileEventType { get; set; }

        //Schedules
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleStatus> ScheduleStatus { get; set; }
        public DbSet<ScheduleEvent> ScheduleEvents { get; set; }
        public DbSet<ScheduleEventType> ScheduleEventType { get; set; }

        //Appointments
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentStatus> AppointmentStatus { get; set; }
        public DbSet<AppointmentEvent> AppointmentEvents { get; set; }
        public DbSet<AppointmentEventType> AppointmentEventType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                _connectionStrings.UseSecondary ? _connectionStrings.SecondaryConnection : _connectionStrings.DefaultConnection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }
    }
}