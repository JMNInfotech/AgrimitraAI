using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class KnowledgeDocumentConfiguration : IEntityTypeConfiguration<KnowledgeDocument>
{
    public void Configure(EntityTypeBuilder<KnowledgeDocument> entity)
    {
        entity.HasKey(e => e.Id).HasName("knowledge_documents_pkey");

        entity.ToTable("knowledge_documents");

        entity.HasIndex(e => e.ApprovedBy, "ix_knowledge_documents_approved_by_9adade");

        entity.HasIndex(e => e.AuthorUserId, "ix_knowledge_documents_author_user_id_4bf8eb");

        entity.HasIndex(e => e.CategoryId, "ix_knowledge_documents_category");

        entity.HasIndex(e => e.CropId, "ix_knowledge_documents_crop");

        entity.HasIndex(e => e.FileObjectId, "ix_knowledge_documents_file_object_id_f2a9ad");

        entity.HasIndex(e => e.LanguageCode, "ix_knowledge_documents_language_code_0684b6");

        entity.HasIndex(e => new { e.Status, e.IngestionStatus }, "ix_knowledge_documents_status").HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => new { e.ContentHash, e.LanguageCode }, "ux_knowledge_documents_hash")
            .IsUnique()
            .HasFilter("((content_hash IS NOT NULL) AND (NOT is_deleted))");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ApprovedAt).HasColumnName("approved_at");
        entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
        entity.Property(e => e.AuthorUserId).HasColumnName("author_user_id");
        entity.Property(e => e.CategoryId).HasColumnName("category_id");
        entity.Property(e => e.ContentHash).HasColumnName("content_hash");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropId).HasColumnName("crop_id");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.FileObjectId).HasColumnName("file_object_id");
        entity.Property(e => e.IngestionStatus)
            .HasDefaultValueSql("'pending'::text")
            .HasColumnName("ingestion_status");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.LanguageCode).HasColumnName("language_code");
        entity.Property(e => e.SourceReference).HasColumnName("source_reference");
        entity.Property(e => e.SourceType).HasColumnName("source_type");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'draft'::text")
            .HasColumnName("status");
        entity.Property(e => e.Title).HasColumnName("title");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.Version)
            .HasDefaultValue(1)
            .HasColumnName("version");

        entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.KnowledgeDocumentApprovedByNavigations)
            .HasForeignKey(d => d.ApprovedBy)
            .HasConstraintName("knowledge_documents_approved_by_fkey");

        entity.HasOne(d => d.AuthorUser).WithMany(p => p.KnowledgeDocumentAuthorUsers)
            .HasForeignKey(d => d.AuthorUserId)
            .HasConstraintName("knowledge_documents_author_user_id_fkey");

        entity.HasOne(d => d.Category).WithMany(p => p.KnowledgeDocuments)
            .HasForeignKey(d => d.CategoryId)
            .HasConstraintName("knowledge_documents_category_id_fkey");

        entity.HasOne(d => d.Crop).WithMany(p => p.KnowledgeDocuments)
            .HasForeignKey(d => d.CropId)
            .HasConstraintName("knowledge_documents_crop_id_fkey");

        entity.HasOne(d => d.FileObject).WithMany(p => p.KnowledgeDocuments)
            .HasForeignKey(d => d.FileObjectId)
            .HasConstraintName("knowledge_documents_file_object_id_fkey");

        entity.HasOne(d => d.LanguageCodeNavigation).WithMany(p => p.KnowledgeDocuments)
            .HasForeignKey(d => d.LanguageCode)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("knowledge_documents_language_code_fkey");
    }
}
