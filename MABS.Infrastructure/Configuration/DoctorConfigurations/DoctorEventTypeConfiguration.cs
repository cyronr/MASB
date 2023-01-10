using MABS.Domain.Models.DoctorModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MABS.Infrastructure.Data.Configuration.DoctorConfigurations
{
    public class DoctorEventTypeConfiguration : IEntityTypeConfiguration<DoctorEventType>
    {
        public void Configure(EntityTypeBuilder<DoctorEventType> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(50);

            builder
                .HasMany(s => s.Events)
                .WithOne(o => o.Type)
                .HasForeignKey(s => s.TypeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasData(
                    Enum.GetValues(typeof(DoctorEventType.Type))
                        .Cast<DoctorEventType.Type>()
                        .Select(e => new DoctorEventType()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
            );
        }
    }
}
