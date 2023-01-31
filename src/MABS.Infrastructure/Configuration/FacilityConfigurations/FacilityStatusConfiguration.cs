using MABS.Domain.Models.FacilityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MABS.Infrastructure.Data.Configuration.FacilityConfigurations
{
    public class FacilityStatusConfiguration : IEntityTypeConfiguration<FacilityStatus>
    {
        public void Configure(EntityTypeBuilder<FacilityStatus> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(50);

            builder
                .HasMany(s => s.Facilities)
                .WithOne(o => o.Status)
                .HasForeignKey(s => s.StatusId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasData(
                    Enum.GetValues(typeof(FacilityStatus.Status))
                        .Cast<FacilityStatus.Status>()
                        .Select(e => new FacilityStatus()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
            );
        }
    }
}
