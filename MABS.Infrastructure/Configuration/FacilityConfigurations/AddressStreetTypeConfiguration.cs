using MABS.Domain.Models.FacilityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MABS.Infrastructure.Data.Configuration.FacilityConfigurations
{
    public class AddressStreetTypeConfiguration : IEntityTypeConfiguration<AddressStreetType>
    {
        public void Configure(EntityTypeBuilder<AddressStreetType> builder)
        {
            builder.Property(p => p.ShortName).HasMaxLength(10);
            builder.Property(p => p.Name).HasMaxLength(50);

            builder
                .HasMany(s => s.Addresses)
                .WithOne(o => o.StreetType)
                .HasForeignKey(s => s.StreetTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasData(
                    Enum.GetValues(typeof(AddressStreetType.StreetType))
                        .Cast<AddressStreetType.StreetType>()
                        .Select(e => new AddressStreetType()
                        {
                            Id = e,
                            ShortName = e.ToString()
                        })
            );
        }
    }
}
