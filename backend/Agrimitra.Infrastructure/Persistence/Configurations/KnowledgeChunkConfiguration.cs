using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class KnowledgeChunkConfiguration : IEntityTypeConfiguration<KnowledgeChunk>
{
    public void Configure(EntityTypeBuilder<KnowledgeChunk> entity)
    {
        entity.HasKey(e => e.Id).HasName("knowledge_chunks_pkey");

        entity.ToTable("knowledge_chunks");

        entity.HasIndex(e => e.LanguageCode, "ix_knowledge_chunks_language_code_715c52");

        entity.HasIndex(e => e.SearchVector, "ix_knowledge_chunks_search").HasMethod("gin");

        entity.HasIndex(e => new { e.DocumentId, e.ChunkIndex }, "knowledge_chunks_document_id_chunk_index_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ChunkIndex).HasColumnName("chunk_index");
        entity.Property(e => e.Content).HasColumnName("content");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DocumentId).HasColumnName("document_id");
        entity.Property(e => e.Heading).HasColumnName("heading");
        entity.Property(e => e.LanguageCode).HasColumnName("language_code");
        entity.Property(e => e.PageNumber).HasColumnName("page_number");
        entity.Property(e => e.SearchVector)
            .HasComputedColumnSql("to_tsvector('simple'::regconfig, content)", true)
            .HasColumnName("search_vector");
        entity.Property(e => e.TokenCount).HasColumnName("token_count");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Document).WithMany(p => p.KnowledgeChunks)
            .HasForeignKey(d => d.DocumentId)
            .HasConstraintName("knowledge_chunks_document_id_fkey");

        entity.HasOne(d => d.LanguageCodeNavigation).WithMany(p => p.KnowledgeChunks)
            .HasForeignKey(d => d.LanguageCode)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("knowledge_chunks_language_code_fkey");
    }
}
