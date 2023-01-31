using MABS.Domain.Models.ProfileModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MABS.Infrastructure.Data.Configuration.ProfileConfigurations
{
    public class ProfileTypeConfiguration : IEntityTypeConfiguration<ProfileType>
    {
        public void Configure(EntityTypeBuilder<ProfileType> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(50);

            builder
                .HasMany(s => s.Profiles)
                .WithOne(o => o.Type)
                .HasForeignKey(s => s.TypeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasData(
                    Enum.GetValues(typeof(ProfileType.Type))
                        .Cast<ProfileType.Type>()
                        .Select(e => new ProfileType()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
            );
        }
    }
}
