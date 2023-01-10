using MABS.Domain.Models.PatientModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MABS.Infrastructure.Data.Configuration.PatientConfigurations
{
    public class PatientStatusConfiguration : IEntityTypeConfiguration<PatientStatus>
    {
        public void Configure(EntityTypeBuilder<PatientStatus> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(50);

            builder
                .HasMany(s => s.Patients)
                .WithOne(o => o.Status)
                .HasForeignKey(s => s.StatusId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasData(
                    Enum.GetValues(typeof(PatientStatus.Status))
                        .Cast<PatientStatus.Status>()
                        .Select(e => new PatientStatus()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
            );
        }
    }
}
