using MABS.Domain.Models.DictionaryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MABS.Infrastructure.Data.Configuration.DictionaryConfigurations
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(255);
        }
    }
}
