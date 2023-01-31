using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MABS.Infrastructure.Data.Configuration.DoctorConfigurations
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.Property(p => p.Firstname).HasMaxLength(100);
            builder.Property(p => p.Lastname).HasMaxLength(100);

            ///fix naming of Doctor-Specialty 
            builder
                .HasMany(p => p.Specialties)
                .WithMany(p => p.Doctors)
                .UsingEntity<Dictionary<string, object>>(
                    "DoctorsSpecialties",
                    j => j
                        .HasOne<Specialty>()
                        .WithMany()
                        .HasForeignKey("SpecialtyId")
                        ,
                    j => j
                        .HasOne<Doctor>()
                        .WithMany()
                        .HasForeignKey("DoctorId"));

            ///fix naming of Doctor-Facility 
            builder
                .HasMany(p => p.Facilities)
                .WithMany(p => p.Doctors)
                .UsingEntity<Dictionary<string, object>>(
                    "DoctorsFacilities",
                    j => j
                        .HasOne<Facility>()
                        .WithMany()
                        .HasForeignKey("FacilityId")
                        ,
                    j => j
                        .HasOne<Doctor>()
                        .WithMany()
                        .HasForeignKey("DoctorId"));
        }
    }
}
