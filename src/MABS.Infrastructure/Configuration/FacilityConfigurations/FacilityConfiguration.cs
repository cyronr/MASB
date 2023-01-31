using MABS.Domain.Models.FacilityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MABS.Infrastructure.Data.Configuration.FacilityConfigurations
{
    public class FacilityConfiguration : IEntityTypeConfiguration<Facility>
    {
        public void Configure(EntityTypeBuilder<Facility> builder)
        {
            builder.Property(p => p.ShortName).HasMaxLength(100);
            builder.Property(p => p.Name).HasMaxLength(255);
            builder.Property(p => p.TaxIdentificationNumber).HasMaxLength(10);
        }
    }
}
