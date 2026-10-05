using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class NurseryBatchConfiguration : IEntityTypeConfiguration<NurseryBatch>
{
    public void Configure(EntityTypeBuilder<NurseryBatch> entity)
    {
        entity.HasKey(e => e.Id).HasName("nursery_batches_pkey");

        entity.ToTable("nursery_batches");

        entity.HasIndex(e => new { e.NurseryProductId, e.Status }, "ix_nursery_batches_product").HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.ReadyOn, "ix_nursery_batches_ready").HasFilter("(status = ANY (ARRAY['growing'::text, 'ready'::text]))");

        entity.HasIndex(e => e.ProductVarietyId, "ix_nursery_batches_variety");

        entity.HasIndex(e => new { e.NurseryId, e.BatchCode }, "nursery_batches_nursery_id_batch_code_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.BatchCode).HasColumnName("batch_code");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.Currency)
            .HasMaxLength(3)
            .HasDefaultValueSql("'INR'::bpchar")
            .IsFixedLength()
            .HasColumnName("currency");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.NurseryId).HasColumnName("nursery_id");
        entity.Property(e => e.NurseryProductId).HasColumnName("nursery_product_id");
        entity.Property(e => e.PlantAgeDays).HasColumnName("plant_age_days");
        entity.Property(e => e.ProductVarietyId).HasColumnName("product_variety_id");
        entity.Property(e => e.QuantityAvailable).HasColumnName("quantity_available");
        entity.Property(e => e.QuantityTotal).HasColumnName("quantity_total");
        entity.Property(e => e.ReadyOn).HasColumnName("ready_on");
        entity.Property(e => e.SownOn).HasColumnName("sown_on");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'growing'::text")
            .HasColumnName("status");
        entity.Property(e => e.UnitPrice)
            .HasPrecision(12, 2)
            .HasColumnName("unit_price");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Nursery).WithMany(p => p.NurseryBatches)
            .HasForeignKey(d => d.NurseryId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("nursery_batches_nursery_id_fkey");

        entity.HasOne(d => d.NurseryProduct).WithMany(p => p.NurseryBatches)
            .HasForeignKey(d => d.NurseryProductId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("nursery_batches_nursery_product_id_fkey");

        entity.HasOne(d => d.ProductVariety).WithMany(p => p.NurseryBatches)
            .HasForeignKey(d => d.ProductVarietyId)
            .HasConstraintName("nursery_batches_product_variety_id_fkey");
    }
}
