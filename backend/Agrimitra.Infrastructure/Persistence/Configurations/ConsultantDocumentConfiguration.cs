using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ConsultantDocumentConfiguration : IEntityTypeConfiguration<ConsultantDocument>
{
    public void Configure(EntityTypeBuilder<ConsultantDocument> entity)
    {
        entity.HasKey(e => e.Id).HasName("consultant_documents_pkey");

        entity.ToTable("consultant_documents");

        entity.HasIndex(e => e.ConsultantId, "ix_consultant_documents_consultant");

        entity.HasIndex(e => e.FileObjectId, "ix_consultant_documents_file_object_id_46e240");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ConsultantId).HasColumnName("consultant_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DocumentType).HasColumnName("document_type");
        entity.Property(e => e.FileObjectId).HasColumnName("file_object_id");
        entity.Property(e => e.Title).HasColumnName("title");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Consultant).WithMany(p => p.ConsultantDocuments)
            .HasForeignKey(d => d.ConsultantId)
            .HasConstraintName("consultant_documents_consultant_id_fkey");

        entity.HasOne(d => d.FileObject).WithMany(p => p.ConsultantDocuments)
            .HasForeignKey(d => d.FileObjectId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("consultant_documents_file_object_id_fkey");
    }
}
