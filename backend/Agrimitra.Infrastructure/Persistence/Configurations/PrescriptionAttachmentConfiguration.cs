using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class PrescriptionAttachmentConfiguration : IEntityTypeConfiguration<PrescriptionAttachment>
{
    public void Configure(EntityTypeBuilder<PrescriptionAttachment> entity)
    {
        entity.HasKey(e => e.Id).HasName("prescription_attachments_pkey");

        entity.ToTable("prescription_attachments");

        entity.HasIndex(e => e.FileObjectId, "ix_prescription_attachments_file_object_id_d717d9");

        entity.HasIndex(e => e.PrescriptionId, "ix_prescription_attachments_rx");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Caption).HasColumnName("caption");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.FileObjectId).HasColumnName("file_object_id");
        entity.Property(e => e.PrescriptionId).HasColumnName("prescription_id");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.FileObject).WithMany(p => p.PrescriptionAttachments)
            .HasForeignKey(d => d.FileObjectId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("prescription_attachments_file_object_id_fkey");

        entity.HasOne(d => d.Prescription).WithMany(p => p.PrescriptionAttachments)
            .HasForeignKey(d => d.PrescriptionId)
            .HasConstraintName("prescription_attachments_prescription_id_fkey");
    }
}
