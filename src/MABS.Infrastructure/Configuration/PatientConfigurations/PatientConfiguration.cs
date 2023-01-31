using MABS.Domain.Models.PatientModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MABS.Infrastructure.Data.Configuration.PatientConfigurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.Property(p => p.Firstname).HasMaxLength(100);
            builder.Property(p => p.Lastname).HasMaxLength(100);
        }
    }
}
