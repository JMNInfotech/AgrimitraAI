using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class FileObjectConfiguration : IEntityTypeConfiguration<FileObject>
{
    public void Configure(EntityTypeBuilder<FileObject> entity)
    {
        entity.HasKey(e => e.Id).HasName("file_objects_pkey");

        entity.ToTable("file_objects");

        entity.HasIndex(e => new { e.Bucket, e.ObjectKey }, "file_objects_bucket_object_key_key").IsUnique();

        entity.HasIndex(e => e.OwnerUserId, "ix_file_objects_owner");

        entity.HasIndex(e => e.ScanStatus, "ix_file_objects_scan").HasFilter("(scan_status = 'pending'::text)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Bucket).HasColumnName("bucket");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.DurationSeconds)
            .HasPrecision(10, 2)
            .HasColumnName("duration_seconds");
        entity.Property(e => e.Height).HasColumnName("height");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.MimeType).HasColumnName("mime_type");
        entity.Property(e => e.ObjectKey).HasColumnName("object_key");
        entity.Property(e => e.OriginalName).HasColumnName("original_name");
        entity.Property(e => e.OwnerUserId).HasColumnName("owner_user_id");
        entity.Property(e => e.Purpose)
            .HasDefaultValueSql("'general'::text")
            .HasColumnName("purpose");
        entity.Property(e => e.ScanStatus)
            .HasDefaultValueSql("'pending'::text")
            .HasColumnName("scan_status");
        entity.Property(e => e.Sha256).HasColumnName("sha256");
        entity.Property(e => e.SizeBytes).HasColumnName("size_bytes");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'uploaded'::text")
            .HasColumnName("status");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.Width).HasColumnName("width");

        entity.HasOne(d => d.OwnerUser).WithMany(p => p.FileObjects)
            .HasForeignKey(d => d.OwnerUserId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("file_objects_owner_user_id_fkey");
    }
}
