using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class MachineryServiceConfiguration : IEntityTypeConfiguration<MachineryService>
{
    public void Configure(EntityTypeBuilder<MachineryService> entity)
    {
        entity.HasKey(e => e.Id).HasName("machinery_services_pkey");

        entity.ToTable("machinery_services");

        entity.HasIndex(e => e.CalendarActivityId, "ix_machinery_services_activity");

        entity.HasIndex(e => new { e.MachineryId, e.ServiceDate }, "ix_machinery_services_machine").IsDescending(false, true);

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CalendarActivityId).HasColumnName("calendar_activity_id");
        entity.Property(e => e.Cost)
            .HasPrecision(12, 2)
            .HasColumnName("cost");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.MachineryId).HasColumnName("machinery_id");
        entity.Property(e => e.NextServiceDate).HasColumnName("next_service_date");
        entity.Property(e => e.Notes).HasColumnName("notes");
        entity.Property(e => e.OdometerOrHours)
            .HasPrecision(10, 1)
            .HasColumnName("odometer_or_hours");
        entity.Property(e => e.ServiceDate).HasColumnName("service_date");
        entity.Property(e => e.ServiceProvider).HasColumnName("service_provider");
        entity.Property(e => e.ServiceType).HasColumnName("service_type");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.CalendarActivity).WithMany(p => p.MachineryServices)
            .HasForeignKey(d => d.CalendarActivityId)
            .HasConstraintName("machinery_services_calendar_activity_id_fkey");

        entity.HasOne(d => d.Machinery).WithMany(p => p.MachineryServices)
            .HasForeignKey(d => d.MachineryId)
            .HasConstraintName("machinery_services_machinery_id_fkey");
    }
}
