using MABS.Domain.Models.PatientModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MABS.Infrastructure.Data.Configuration.PatientConfigurations
{
    public class PatientEventTypeConfiguration : IEntityTypeConfiguration<PatientEventType>
    {
        public void Configure(EntityTypeBuilder<PatientEventType> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(50);

            builder
                .HasMany(s => s.Events)
                .WithOne(o => o.Type)
                .HasForeignKey(s => s.TypeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasData(
                    Enum.GetValues(typeof(PatientEventType.Type))
                        .Cast<PatientEventType.Type>()
                        .Select(e => new PatientEventType()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
            );
        }
    }
}
