using MABS.Domain.Models.FacilityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MABS.Infrastructure.Data.Configuration.FacilityConfigurations
{
    public class FacilityEventTypeConfiguration : IEntityTypeConfiguration<FacilityEventType>
    {
        public void Configure(EntityTypeBuilder<FacilityEventType> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(50);

            builder
                .HasMany(s => s.Events)
                .WithOne(o => o.Type)
                .HasForeignKey(s => s.TypeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasData(
                    Enum.GetValues(typeof(FacilityEventType.Type))
                        .Cast<FacilityEventType.Type>()
                        .Select(e => new FacilityEventType()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
            );
        }
    }
}
