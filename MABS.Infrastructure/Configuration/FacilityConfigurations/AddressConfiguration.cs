using MABS.Domain.Models.FacilityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MABS.Infrastructure.Data.Configuration.FacilityConfigurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(255);
            builder.Property(p => p.StreetName).HasMaxLength(255);
            builder.Property(p => p.City).HasMaxLength(255);
            builder.Property(p => p.PostalCode).HasMaxLength(6);
        }
    }
}
