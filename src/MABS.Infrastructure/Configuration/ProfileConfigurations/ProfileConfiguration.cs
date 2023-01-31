using MABS.Domain.Models.ProfileModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace MABS.Infrastructure.Data.Configuration.ProfileConfigurations
{
    public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.Property(p => p.Email).HasMaxLength(150);
            builder.Property(p => p.PhoneNumber).HasMaxLength(20);

            builder
                .HasMany(s => s.CallerProfileEvents)
                .WithOne(t => t.CallerProfile)
                .HasForeignKey(t => t.CallerProfileId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
