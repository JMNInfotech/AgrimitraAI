using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> entity)
    {
        entity.HasKey(e => e.Id).HasName("orders_pkey");

        entity.ToTable("orders");

        entity.HasIndex(e => new { e.BuyerUserId, e.PlacedAt }, "ix_orders_buyer")
            .IsDescending(false, true)
            .HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.DeliveryAddressId, "ix_orders_delivery_address");

        entity.HasIndex(e => e.FarmerProfileId, "ix_orders_farmer");

        entity.HasIndex(e => new { e.NurseryId, e.Status, e.PlacedAt }, "ix_orders_nursery_status").IsDescending(false, false, true);

        entity.HasIndex(e => e.OrganizationId, "ix_orders_organization_id_69f37d");

        entity.HasIndex(e => new { e.ShopId, e.Status, e.PlacedAt }, "ix_orders_shop_status").IsDescending(false, false, true);

        entity.HasIndex(e => new { e.Status, e.PlacedAt }, "ix_orders_status").IsDescending(false, true);

        entity.HasIndex(e => e.IdempotencyKey, "orders_idempotency_key_key").IsUnique();

        entity.HasIndex(e => e.OrderNumber, "orders_order_number_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.BuyerUserId).HasColumnName("buyer_user_id");
        entity.Property(e => e.CancellationReason).HasColumnName("cancellation_reason");
        entity.Property(e => e.CancelledAt).HasColumnName("cancelled_at");
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
        entity.Property(e => e.DeliveryAddressId).HasColumnName("delivery_address_id");
        entity.Property(e => e.DeliveryAddressSnapshot)
            .HasColumnType("jsonb")
            .HasColumnName("delivery_address_snapshot");
        entity.Property(e => e.DiscountAmount)
            .HasPrecision(14, 2)
            .HasColumnName("discount_amount");
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.Fulfillment)
            .HasDefaultValueSql("'delivery'::text")
            .HasColumnName("fulfillment");
        entity.Property(e => e.IdempotencyKey).HasColumnName("idempotency_key");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.Notes).HasColumnName("notes");
        entity.Property(e => e.NurseryId).HasColumnName("nursery_id");
        entity.Property(e => e.OrderNumber).HasColumnName("order_number");
        entity.Property(e => e.OrganizationId).HasColumnName("organization_id");
        entity.Property(e => e.PaymentStatus)
            .HasDefaultValueSql("'PENDING'::text")
            .HasColumnName("payment_status");
        entity.Property(e => e.PlacedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("placed_at");
        entity.Property(e => e.SellerType).HasColumnName("seller_type");
        entity.Property(e => e.ShippingAmount)
            .HasPrecision(14, 2)
            .HasColumnName("shipping_amount");
        entity.Property(e => e.ShopId).HasColumnName("shop_id");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'pending'::text")
            .HasColumnName("status");
        entity.Property(e => e.Subtotal)
            .HasPrecision(14, 2)
            .HasColumnName("subtotal");
        entity.Property(e => e.TaxAmount)
            .HasPrecision(14, 2)
            .HasColumnName("tax_amount");
        entity.Property(e => e.TotalAmount)
            .HasPrecision(14, 2)
            .HasComputedColumnSql("(((subtotal + tax_amount) + shipping_amount) - discount_amount)", true)
            .HasColumnName("total_amount");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.BuyerUser).WithMany(p => p.Orders)
            .HasForeignKey(d => d.BuyerUserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("orders_buyer_user_id_fkey");

        entity.HasOne(d => d.DeliveryAddress).WithMany(p => p.Orders)
            .HasForeignKey(d => d.DeliveryAddressId)
            .HasConstraintName("orders_delivery_address_id_fkey");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.Orders)
            .HasForeignKey(d => d.FarmerProfileId)
            .HasConstraintName("orders_farmer_profile_id_fkey");

        entity.HasOne(d => d.Nursery).WithMany(p => p.Orders)
            .HasForeignKey(d => d.NurseryId)
            .HasConstraintName("orders_nursery_id_fkey");

        entity.HasOne(d => d.Organization).WithMany(p => p.Orders)
            .HasForeignKey(d => d.OrganizationId)
            .HasConstraintName("orders_organization_id_fkey");

        entity.HasOne(d => d.Shop).WithMany(p => p.Orders)
            .HasForeignKey(d => d.ShopId)
            .HasConstraintName("orders_shop_id_fkey");
    }
}
