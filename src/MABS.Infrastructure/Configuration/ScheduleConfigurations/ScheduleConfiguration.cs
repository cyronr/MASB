using MABS.Domain.Models.ScheduleModels;
using MABS.Infrastructure.Configuration.ConfigurationUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MABS.Infrastructure.Configuration.ScheduleConfigurations;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.Property(x => x.ValidDateFrom)
            .HasConversion<DateOnlyConverter, DateOnlyComparer>();

        builder.Property(x => x.ValidDateTo)
            .HasConversion<DateOnlyConverter, DateOnlyComparer>();

        builder.Property(x => x.StartTime)
            .HasConversion<TimeOnlyConverter, TimeOnlyComparer>();

        builder.Property(x => x.EndTime)
            .HasConversion<TimeOnlyConverter, TimeOnlyComparer>();
    }
}
