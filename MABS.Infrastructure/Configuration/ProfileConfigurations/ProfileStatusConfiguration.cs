using MABS.Domain.Models.ProfileModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MABS.Infrastructure.Data.Configuration.ProfileConfigurations
{
    public class ProfileStatusConfiguration : IEntityTypeConfiguration<ProfileStatus>
    {
        public void Configure(EntityTypeBuilder<ProfileStatus> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(50);

            builder
                .HasMany(s => s.Profiles)
                .WithOne(o => o.Status)
                .HasForeignKey(s => s.StatusId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasData(
                    Enum.GetValues(typeof(ProfileStatus.Status))
                        .Cast<ProfileStatus.Status>()
                        .Select(e => new ProfileStatus()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
            );
        }
    }
}
