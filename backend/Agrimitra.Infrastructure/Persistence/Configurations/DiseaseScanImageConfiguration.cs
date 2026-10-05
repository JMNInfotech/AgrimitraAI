using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class DiseaseScanImageConfiguration : IEntityTypeConfiguration<DiseaseScanImage>
{
    public void Configure(EntityTypeBuilder<DiseaseScanImage> entity)
    {
        entity.HasKey(e => e.Id).HasName("disease_scan_images_pkey");

        entity.ToTable("disease_scan_images");

        entity.HasIndex(e => e.FileObjectId, "ix_disease_scan_images_file_object_id_424e2b");

        entity.HasIndex(e => e.DiseaseScanId, "ix_scan_images_scan");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DiseaseScanId).HasColumnName("disease_scan_id");
        entity.Property(e => e.FileObjectId).HasColumnName("file_object_id");
        entity.Property(e => e.SortOrder).HasColumnName("sort_order");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.DiseaseScan).WithMany(p => p.DiseaseScanImages)
            .HasForeignKey(d => d.DiseaseScanId)
            .HasConstraintName("disease_scan_images_disease_scan_id_fkey");

        entity.HasOne(d => d.FileObject).WithMany(p => p.DiseaseScanImages)
            .HasForeignKey(d => d.FileObjectId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("disease_scan_images_file_object_id_fkey");
    }
}
