using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class MessageAttachmentConfiguration : IEntityTypeConfiguration<MessageAttachment>
{
    public void Configure(EntityTypeBuilder<MessageAttachment> entity)
    {
        entity.HasKey(e => e.Id).HasName("message_attachments_pkey");

        entity.ToTable("message_attachments");

        entity.HasIndex(e => e.FileObjectId, "ix_message_attachments_file");

        entity.HasIndex(e => e.MessageId, "ix_message_attachments_message");

        entity.HasIndex(e => e.ThumbnailFileId, "ix_message_attachments_thumbnail_file_id_72761f");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AttachmentKind).HasColumnName("attachment_kind");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DurationSeconds)
            .HasPrecision(10, 2)
            .HasColumnName("duration_seconds");
        entity.Property(e => e.FileObjectId).HasColumnName("file_object_id");
        entity.Property(e => e.MessageId).HasColumnName("message_id");
        entity.Property(e => e.ThumbnailFileId).HasColumnName("thumbnail_file_id");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.Waveform)
            .HasColumnType("jsonb")
            .HasColumnName("waveform");

        entity.HasOne(d => d.FileObject).WithMany(p => p.MessageAttachmentFileObjects)
            .HasForeignKey(d => d.FileObjectId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("message_attachments_file_object_id_fkey");

        entity.HasOne(d => d.Message).WithMany(p => p.MessageAttachments)
            .HasForeignKey(d => d.MessageId)
            .HasConstraintName("message_attachments_message_id_fkey");

        entity.HasOne(d => d.ThumbnailFile).WithMany(p => p.MessageAttachmentThumbnailFiles)
            .HasForeignKey(d => d.ThumbnailFileId)
            .HasConstraintName("message_attachments_thumbnail_file_id_fkey");
    }
}
