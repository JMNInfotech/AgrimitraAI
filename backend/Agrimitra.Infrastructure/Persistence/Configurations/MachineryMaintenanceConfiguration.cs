using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class MachineryMaintenanceConfiguration : IEntityTypeConfiguration<MachineryMaintenance>
{
    public void Configure(EntityTypeBuilder<MachineryMaintenance> entity)
    {
        entity.HasKey(e => e.Id).HasName("machinery_maintenance_pkey");

        entity.ToTable("machinery_maintenance");

        entity.HasIndex(e => new { e.MachineryId, e.MaintenanceDate }, "ix_machinery_maintenance_machine").IsDescending(false, true);

        entity.HasIndex(e => e.ReceiptFileId, "ix_machinery_maintenance_receipt_file_id_186861");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Cost)
            .HasPrecision(12, 2)
            .HasColumnName("cost");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.MachineryId).HasColumnName("machinery_id");
        entity.Property(e => e.MaintenanceDate).HasColumnName("maintenance_date");
        entity.Property(e => e.ReceiptFileId).HasColumnName("receipt_file_id");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.Vendor).HasColumnName("vendor");

        entity.HasOne(d => d.Machinery).WithMany(p => p.MachineryMaintenances)
            .HasForeignKey(d => d.MachineryId)
            .HasConstraintName("machinery_maintenance_machinery_id_fkey");

        entity.HasOne(d => d.ReceiptFile).WithMany(p => p.MachineryMaintenances)
            .HasForeignKey(d => d.ReceiptFileId)
            .HasConstraintName("machinery_maintenance_receipt_file_id_fkey");
    }
}
