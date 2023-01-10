using MABS.Domain.Models.FacilityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MABS.Infrastructure.Data.Configuration.FacilityConfigurations
{
    public class AddressStatusConfiguration : IEntityTypeConfiguration<AddressStatus>
    {
        public void Configure(EntityTypeBuilder<AddressStatus> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(50);

            builder
                .HasMany(s => s.Addresses)
                .WithOne(o => o.Status)
                .HasForeignKey(s => s.StatusId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasData(
                    Enum.GetValues(typeof(AddressStatus.Status))
                        .Cast<AddressStatus.Status>()
                        .Select(e => new AddressStatus()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
            );
        }
    }
}
