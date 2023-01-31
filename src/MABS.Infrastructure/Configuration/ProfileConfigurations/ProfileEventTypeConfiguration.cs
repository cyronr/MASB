using MABS.Domain.Models.ProfileModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MABS.Infrastructure.Data.Configuration.ProfileConfigurations
{
    public class ProfileEventTypeConfiguration : IEntityTypeConfiguration<ProfileEventType>
    {
        public void Configure(EntityTypeBuilder<ProfileEventType> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(50);

            builder
                .HasMany(s => s.Events)
                .WithOne(o => o.Type)
                .HasForeignKey(s => s.TypeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasData(
                    Enum.GetValues(typeof(ProfileEventType.Type))
                        .Cast<ProfileEventType.Type>()
                        .Select(e => new ProfileEventType()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
            );
        }
    }
}
