using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class VerificationDocumentConfiguration : IEntityTypeConfiguration<VerificationDocument>
{
    public void Configure(EntityTypeBuilder<VerificationDocument> entity)
    {
        entity.HasKey(e => e.Id).HasName("verification_documents_pkey");

        entity.ToTable("verification_documents");

        entity.HasIndex(e => e.FileObjectId, "ix_verification_documents_file_object_id_3f6d00");

        entity.HasIndex(e => e.VerificationRecordId, "ix_verification_documents_record");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DocumentNumberEncrypted).HasColumnName("document_number_encrypted");
        entity.Property(e => e.DocumentType).HasColumnName("document_type");
        entity.Property(e => e.ExpiresOn).HasColumnName("expires_on");
        entity.Property(e => e.FileObjectId).HasColumnName("file_object_id");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.VerificationRecordId).HasColumnName("verification_record_id");

        entity.HasOne(d => d.FileObject).WithMany(p => p.VerificationDocuments)
            .HasForeignKey(d => d.FileObjectId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("verification_documents_file_object_id_fkey");

        entity.HasOne(d => d.VerificationRecord).WithMany(p => p.VerificationDocuments)
            .HasForeignKey(d => d.VerificationRecordId)
            .HasConstraintName("verification_documents_verification_record_id_fkey");
    }
}
