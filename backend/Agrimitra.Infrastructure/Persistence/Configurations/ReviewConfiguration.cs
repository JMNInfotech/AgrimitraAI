using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> entity)
    {
        entity.HasKey(e => e.Id).HasName("reviews_pkey");

        entity.ToTable("reviews");

        entity.HasIndex(e => new { e.ConsultantId, e.Status }, "ix_reviews_consultant");

        entity.HasIndex(e => e.ConsultationId, "ix_reviews_consultation_id_065aa5");

        entity.HasIndex(e => e.LabBookingId, "ix_reviews_lab_booking_id_6f0be3");

        entity.HasIndex(e => new { e.LaboratoryId, e.Status }, "ix_reviews_laboratory");

        entity.HasIndex(e => e.ModeratedBy, "ix_reviews_moderated_by_390726");

        entity.HasIndex(e => e.CreatedAt, "ix_reviews_moderation").HasFilter("(status = 'pending'::text)");

        entity.HasIndex(e => new { e.NurseryId, e.Status }, "ix_reviews_nursery");

        entity.HasIndex(e => e.OrderItemId, "ix_reviews_order_item");

        entity.HasIndex(e => new { e.ProductId, e.Status }, "ix_reviews_product");

        entity.HasIndex(e => e.ReviewerUserId, "ix_reviews_reviewer");

        entity.HasIndex(e => new { e.ShopId, e.Status }, "ix_reviews_shop");

        entity.HasIndex(e => new { e.ReviewerUserId, e.ConsultationId }, "ux_reviews_consultation")
            .IsUnique()
            .HasFilter("((consultation_id IS NOT NULL) AND (NOT is_deleted))");

        entity.HasIndex(e => new { e.ReviewerUserId, e.LabBookingId }, "ux_reviews_lab_booking")
            .IsUnique()
            .HasFilter("((lab_booking_id IS NOT NULL) AND (NOT is_deleted))");

        entity.HasIndex(e => new { e.ReviewerUserId, e.OrderItemId }, "ux_reviews_order_item")
            .IsUnique()
            .HasFilter("((order_item_id IS NOT NULL) AND (NOT is_deleted))");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Comment).HasColumnName("comment");
        entity.Property(e => e.ConsultantId).HasColumnName("consultant_id");
        entity.Property(e => e.ConsultationId).HasColumnName("consultation_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.LabBookingId).HasColumnName("lab_booking_id");
        entity.Property(e => e.LaboratoryId).HasColumnName("laboratory_id");
        entity.Property(e => e.ModeratedAt).HasColumnName("moderated_at");
        entity.Property(e => e.ModeratedBy).HasColumnName("moderated_by");
        entity.Property(e => e.ModerationReason).HasColumnName("moderation_reason");
        entity.Property(e => e.NurseryId).HasColumnName("nursery_id");
        entity.Property(e => e.OrderItemId).HasColumnName("order_item_id");
        entity.Property(e => e.ProductId).HasColumnName("product_id");
        entity.Property(e => e.Rating).HasColumnName("rating");
        entity.Property(e => e.ReviewerUserId).HasColumnName("reviewer_user_id");
        entity.Property(e => e.SellerReply).HasColumnName("seller_reply");
        entity.Property(e => e.ShopId).HasColumnName("shop_id");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'pending'::text")
            .HasColumnName("status");
        entity.Property(e => e.TargetType).HasColumnName("target_type");
        entity.Property(e => e.Title).HasColumnName("title");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Consultant).WithMany(p => p.Reviews)
            .HasForeignKey(d => d.ConsultantId)
            .HasConstraintName("reviews_consultant_id_fkey");

        entity.HasOne(d => d.Consultation).WithMany(p => p.Reviews)
            .HasForeignKey(d => d.ConsultationId)
            .HasConstraintName("reviews_consultation_id_fkey");

        entity.HasOne(d => d.LabBooking).WithMany(p => p.Reviews)
            .HasForeignKey(d => d.LabBookingId)
            .HasConstraintName("reviews_lab_booking_id_fkey");

        entity.HasOne(d => d.Laboratory).WithMany(p => p.Reviews)
            .HasForeignKey(d => d.LaboratoryId)
            .HasConstraintName("reviews_laboratory_id_fkey");

        entity.HasOne(d => d.ModeratedByNavigation).WithMany(p => p.ReviewModeratedByNavigations)
            .HasForeignKey(d => d.ModeratedBy)
            .HasConstraintName("reviews_moderated_by_fkey");

        entity.HasOne(d => d.Nursery).WithMany(p => p.Reviews)
            .HasForeignKey(d => d.NurseryId)
            .HasConstraintName("reviews_nursery_id_fkey");

        entity.HasOne(d => d.OrderItem).WithMany(p => p.Reviews)
            .HasForeignKey(d => d.OrderItemId)
            .HasConstraintName("fk_reviews_order_item");

        entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
            .HasForeignKey(d => d.ProductId)
            .HasConstraintName("reviews_product_id_fkey");

        entity.HasOne(d => d.ReviewerUser).WithMany(p => p.ReviewReviewerUsers)
            .HasForeignKey(d => d.ReviewerUserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("reviews_reviewer_user_id_fkey");

        entity.HasOne(d => d.Shop).WithMany(p => p.Reviews)
            .HasForeignKey(d => d.ShopId)
            .HasConstraintName("reviews_shop_id_fkey");
    }
}
