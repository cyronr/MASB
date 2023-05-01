using MABS.Domain.Models.AppointmentModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MABS.Infrastructure.Data.Configuration.AppointmentConfigurations
{
    public class AppointmentEventTypeConfiguration : IEntityTypeConfiguration<AppointmentEventType>
    {
        public void Configure(EntityTypeBuilder<AppointmentEventType> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(50);

            builder
                .HasMany(s => s.Events)
                .WithOne(o => o.Type)
                .HasForeignKey(s => s.TypeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasData(
                    Enum.GetValues(typeof(AppointmentEventType.Type))
                        .Cast<AppointmentEventType.Type>()
                        .Select(e => new AppointmentEventType()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
            );
        }
    }
}
