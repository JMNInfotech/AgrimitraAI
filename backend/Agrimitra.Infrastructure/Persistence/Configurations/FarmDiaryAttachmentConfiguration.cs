using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class FarmDiaryAttachmentConfiguration : IEntityTypeConfiguration<FarmDiaryAttachment>
{
    public void Configure(EntityTypeBuilder<FarmDiaryAttachment> entity)
    {
        entity.HasKey(e => e.Id).HasName("farm_diary_attachments_pkey");

        entity.ToTable("farm_diary_attachments");

        entity.HasIndex(e => e.DiaryId, "ix_diary_attachments_diary");

        entity.HasIndex(e => e.FileObjectId, "ix_farm_diary_attachments_file_object_id_452945");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Caption).HasColumnName("caption");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DiaryId).HasColumnName("diary_id");
        entity.Property(e => e.FileObjectId).HasColumnName("file_object_id");
        entity.Property(e => e.SortOrder).HasColumnName("sort_order");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Diary).WithMany(p => p.FarmDiaryAttachments)
            .HasForeignKey(d => d.DiaryId)
            .HasConstraintName("farm_diary_attachments_diary_id_fkey");

        entity.HasOne(d => d.FileObject).WithMany(p => p.FarmDiaryAttachments)
            .HasForeignKey(d => d.FileObjectId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("farm_diary_attachments_file_object_id_fkey");
    }
}
