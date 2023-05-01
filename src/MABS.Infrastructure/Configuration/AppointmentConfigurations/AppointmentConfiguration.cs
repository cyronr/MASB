using MABS.Domain.Models.AppointmentModels;
using MABS.Infrastructure.Configuration.ConfigurationUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MABS.Infrastructure.Configuration.AppointmentConfigurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.Property(x => x.Date)
            .HasConversion<DateOnlyConverter, DateOnlyComparer>();

        builder.Property(x => x.Time)
            .HasConversion<TimeOnlyConverter, TimeOnlyComparer>();
    }
}
