using MABS.Domain.Models.DoctorModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MABS.Infrastructure.Data.Configuration.DoctorConfigurations
{
    public class DoctorStatusConfiguration : IEntityTypeConfiguration<DoctorStatus>
    {
        public void Configure(EntityTypeBuilder<DoctorStatus> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(50);

            builder
                .HasMany(s => s.Doctors)
                .WithOne(o => o.Status)
                .HasForeignKey(s => s.StatusId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasData(
                    Enum.GetValues(typeof(DoctorStatus.Status))
                        .Cast<DoctorStatus.Status>()
                        .Select(e => new DoctorStatus()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
            );
        }
    }
}
