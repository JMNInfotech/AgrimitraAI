using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> entity)
    {
        entity.HasKey(e => e.Id).HasName("deliveries_pkey");

        entity.ToTable("deliveries");

        entity.HasIndex(e => e.ShipmentId, "deliveries_shipment_id_key").IsUnique();

        entity.HasIndex(e => e.ProofFileId, "ix_deliveries_proof_file_id_1cdada");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeliveredAt).HasColumnName("delivered_at");
        entity.Property(e => e.FailureReason).HasColumnName("failure_reason");
        entity.Property(e => e.ProofFileId).HasColumnName("proof_file_id");
        entity.Property(e => e.ReceivedBy).HasColumnName("received_by");
        entity.Property(e => e.ShipmentId).HasColumnName("shipment_id");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'pending'::text")
            .HasColumnName("status");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.ProofFile).WithMany(p => p.Deliveries)
            .HasForeignKey(d => d.ProofFileId)
            .HasConstraintName("deliveries_proof_file_id_fkey");

        entity.HasOne(d => d.Shipment).WithOne(p => p.Delivery)
            .HasForeignKey<Delivery>(d => d.ShipmentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("deliveries_shipment_id_fkey");
    }
}
