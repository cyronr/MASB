using MABS.Domain.Models.PatientModels;
using MABS.Domain.Models.ScheduleModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MABS.Infrastructure.Data.Configuration.ScheduleConfigurations
{
    public class ScheduleStatusConfiguration : IEntityTypeConfiguration<ScheduleStatus>
    {
        public void Configure(EntityTypeBuilder<ScheduleStatus> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(50);

            builder
                .HasMany(s => s.Schedules)
                .WithOne(o => o.Status)
                .HasForeignKey(s => s.StatusId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasData(
                    Enum.GetValues(typeof(ScheduleStatus.Status))
                        .Cast<ScheduleStatus.Status>()
                        .Select(e => new ScheduleStatus()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
            );
        }
    }
}
