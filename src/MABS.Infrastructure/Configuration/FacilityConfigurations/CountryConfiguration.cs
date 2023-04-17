using MABS.Domain.Models.DictionaryModels;
using MABS.Domain.Models.FacilityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MABS.Infrastructure.Data.Configuration.FacilityConfigurations
{
    public class CityConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.Property(p => p.Id)
                .HasColumnType("char")
                .HasMaxLength(2);
            builder.Property(p => p.Name).HasMaxLength(100);
        }
    }
}
