using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ConsultantReviewConfiguration : IEntityTypeConfiguration<ConsultantReview>
{
    public void Configure(EntityTypeBuilder<ConsultantReview> entity)
    {
        entity.HasKey(e => e.Id).HasName("consultant_reviews_pkey");

        entity.ToTable("consultant_reviews");

        entity.HasIndex(e => e.ConsultationId, "consultant_reviews_consultation_id_key").IsUnique();

        entity.HasIndex(e => new { e.ConsultantId, e.Status }, "ix_consultant_reviews_consultant");

        entity.HasIndex(e => e.FarmerProfileId, "ix_consultant_reviews_farmer");

        entity.HasIndex(e => e.ModeratedBy, "ix_consultant_reviews_moderated_by_1ed47c");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Comment).HasColumnName("comment");
        entity.Property(e => e.ConsultantId).HasColumnName("consultant_id");
        entity.Property(e => e.ConsultantReply).HasColumnName("consultant_reply");
        entity.Property(e => e.ConsultationId).HasColumnName("consultation_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.ModeratedAt).HasColumnName("moderated_at");
        entity.Property(e => e.ModeratedBy).HasColumnName("moderated_by");
        entity.Property(e => e.Rating).HasColumnName("rating");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'pending'::text")
            .HasColumnName("status");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Consultant).WithMany(p => p.ConsultantReviews)
            .HasForeignKey(d => d.ConsultantId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("consultant_reviews_consultant_id_fkey");

        entity.HasOne(d => d.Consultation).WithOne(p => p.ConsultantReview)
            .HasForeignKey<ConsultantReview>(d => d.ConsultationId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("consultant_reviews_consultation_id_fkey");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.ConsultantReviews)
            .HasForeignKey(d => d.FarmerProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("consultant_reviews_farmer_profile_id_fkey");

        entity.HasOne(d => d.ModeratedByNavigation).WithMany(p => p.ConsultantReviews)
            .HasForeignKey(d => d.ModeratedBy)
            .HasConstraintName("consultant_reviews_moderated_by_fkey");
    }
}
