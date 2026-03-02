using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Infrastructure.Persistence.Configurations;

internal class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events", t =>
        {
            t.HasCheckConstraint("CK_Event_Name_NotEmpty", "TRIM(Name) <> ''");
            t.HasCheckConstraint("CK_Event_Scope_Range", "Scope IN (0,1,2)");
        });

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
               .HasMaxLength(200)
               .IsRequired(true);

        builder.Property(e => e.Description)
               .HasMaxLength(500)
               .IsRequired(false);

        builder.Property(e => e.Scope)
               .HasConversion<byte>()
               .IsRequired(true);

        builder.HasIndex(e => e.Name)
       .IsUnique()
       .HasDatabaseName("UX_Event_Name");
    }
}
