using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class LandImageConfiguration : IEntityTypeConfiguration<LandImage>
{
    public void Configure(EntityTypeBuilder<LandImage> entity)
    {
        entity.HasKey(e => e.Id).HasName("land_images_pkey");

        entity.ToTable("land_images");

        entity.HasIndex(e => e.FileObjectId, "ix_land_images_file_object_id_8ad6b2");

        entity.HasIndex(e => e.LandId, "ix_land_images_land").HasFilter("(NOT is_deleted)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Caption).HasColumnName("caption");
        entity.Property(e => e.CapturedAt).HasColumnName("captured_at");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.FileObjectId).HasColumnName("file_object_id");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.LandId).HasColumnName("land_id");
        entity.Property(e => e.SortOrder).HasColumnName("sort_order");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.FileObject).WithMany(p => p.LandImages)
            .HasForeignKey(d => d.FileObjectId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("land_images_file_object_id_fkey");

        entity.HasOne(d => d.Land).WithMany(p => p.LandImages)
            .HasForeignKey(d => d.LandId)
            .HasConstraintName("land_images_land_id_fkey");
    }
}
