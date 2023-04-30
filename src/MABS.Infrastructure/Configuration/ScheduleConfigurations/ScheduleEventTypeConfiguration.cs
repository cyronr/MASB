using MABS.Domain.Models.PatientModels;
using MABS.Domain.Models.ScheduleModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MABS.Infrastructure.Data.Configuration.ScheduleConfigurations
{
    public class ScheduleEventTypeConfiguration : IEntityTypeConfiguration<ScheduleEventType>
    {
        public void Configure(EntityTypeBuilder<ScheduleEventType> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(50);

            builder
                .HasMany(s => s.Events)
                .WithOne(o => o.Type)
                .HasForeignKey(s => s.TypeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasData(
                    Enum.GetValues(typeof(ScheduleEventType.Type))
                        .Cast<ScheduleEventType.Type>()
                        .Select(e => new ScheduleEventType()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
            );
        }
    }
}
