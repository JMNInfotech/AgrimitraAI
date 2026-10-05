using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class EmbeddingConfiguration : IEntityTypeConfiguration<Embedding>
{
    public void Configure(EntityTypeBuilder<Embedding> entity)
    {
        entity.HasKey(e => e.Id).HasName("embeddings_pkey");

        entity.ToTable("embeddings");

        entity.HasIndex(e => new { e.ChunkId, e.EmbeddingModel }, "embeddings_chunk_id_embedding_model_key").IsUnique();

        entity.HasIndex(e => e.Embedding1, "ix_embeddings_hnsw")
            .HasMethod("hnsw")
            .HasOperators(new[] { "vector_cosine_ops" })
            .HasAnnotation("Npgsql:StorageParameter:ef_construction", "64")
            .HasAnnotation("Npgsql:StorageParameter:m", "16");

        entity.HasIndex(e => e.ModelVersionId, "ix_embeddings_model_version");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ChunkId).HasColumnName("chunk_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.Embedding1)
            .HasMaxLength(1024)
            .HasColumnName("embedding");
        entity.Property(e => e.EmbeddingModel).HasColumnName("embedding_model");
        entity.Property(e => e.ModelVersionId).HasColumnName("model_version_id");

        entity.HasOne(d => d.Chunk).WithMany(p => p.Embeddings)
            .HasForeignKey(d => d.ChunkId)
            .HasConstraintName("embeddings_chunk_id_fkey");

        entity.HasOne(d => d.ModelVersion).WithMany(p => p.Embeddings)
            .HasForeignKey(d => d.ModelVersionId)
            .HasConstraintName("embeddings_model_version_id_fkey");
    }
}
