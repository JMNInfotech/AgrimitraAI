using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class LandDocumentConfiguration : IEntityTypeConfiguration<LandDocument>
{
    public void Configure(EntityTypeBuilder<LandDocument> entity)
    {
        entity.HasKey(e => e.Id).HasName("land_documents_pkey");

        entity.ToTable("land_documents");

        entity.HasIndex(e => e.FileObjectId, "ix_land_documents_file_object_id_f6673a");

        entity.HasIndex(e => e.LandId, "ix_land_documents_land").HasFilter("(NOT is_deleted)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.DocumentType).HasColumnName("document_type");
        entity.Property(e => e.FileObjectId).HasColumnName("file_object_id");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.LandId).HasColumnName("land_id");
        entity.Property(e => e.Title).HasColumnName("title");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.FileObject).WithMany(p => p.LandDocuments)
            .HasForeignKey(d => d.FileObjectId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("land_documents_file_object_id_fkey");

        entity.HasOne(d => d.Land).WithMany(p => p.LandDocuments)
            .HasForeignKey(d => d.LandId)
            .HasConstraintName("land_documents_land_id_fkey");
    }
}
