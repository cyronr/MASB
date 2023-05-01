using MABS.Domain.Models.AppointmentModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MABS.Infrastructure.Data.Configuration.AppointmentConfigurations
{
    public class AppointmentStatusConfiguration : IEntityTypeConfiguration<AppointmentStatus>
    {
        public void Configure(EntityTypeBuilder<AppointmentStatus> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(50);

            builder
                .HasMany(s => s.Appointments)
                .WithOne(o => o.Status)
                .HasForeignKey(s => s.StatusId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasData(
                    Enum.GetValues(typeof(AppointmentStatus.Status))
                        .Cast<AppointmentStatus.Status>()
                        .Select(e => new AppointmentStatus()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
            );
        }
    }
}
