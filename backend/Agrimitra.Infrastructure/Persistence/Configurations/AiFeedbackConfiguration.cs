using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class AiFeedbackConfiguration : IEntityTypeConfiguration<AiFeedback>
{
    public void Configure(EntityTypeBuilder<AiFeedback> entity)
    {
        entity.HasKey(e => e.Id).HasName("ai_feedback_pkey");

        entity.ToTable("ai_feedback");

        entity.HasIndex(e => e.CandidateReviewedBy, "ix_ai_feedback_candidate_reviewed_by_e3d806");

        entity.HasIndex(e => e.CorrectedCropDiseaseId, "ix_ai_feedback_corrected_crop_disease_id_06cc8a");

        entity.HasIndex(e => e.CreatedAt, "ix_feedback_candidates").HasFilter("(candidate_status = 'proposed'::text)");

        entity.HasIndex(e => e.RecommendationId, "ix_feedback_recommendation");

        entity.HasIndex(e => e.DiseaseResultId, "ix_feedback_result");

        entity.HasIndex(e => e.UserId, "ix_feedback_user");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CandidateReviewedAt).HasColumnName("candidate_reviewed_at");
        entity.Property(e => e.CandidateReviewedBy).HasColumnName("candidate_reviewed_by");
        entity.Property(e => e.CandidateStatus)
            .HasDefaultValueSql("'none'::text")
            .HasColumnName("candidate_status");
        entity.Property(e => e.Comment).HasColumnName("comment");
        entity.Property(e => e.CorrectedCropDiseaseId).HasColumnName("corrected_crop_disease_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DiseaseResultId).HasColumnName("disease_result_id");
        entity.Property(e => e.FeedbackType).HasColumnName("feedback_type");
        entity.Property(e => e.InferenceLogId).HasColumnName("inference_log_id");
        entity.Property(e => e.IsHelpful).HasColumnName("is_helpful");
        entity.Property(e => e.RecommendationId).HasColumnName("recommendation_id");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.UserId).HasColumnName("user_id");

        entity.HasOne(d => d.CandidateReviewedByNavigation).WithMany(p => p.AiFeedbackCandidateReviewedByNavigations)
            .HasForeignKey(d => d.CandidateReviewedBy)
            .HasConstraintName("ai_feedback_candidate_reviewed_by_fkey");

        entity.HasOne(d => d.CorrectedCropDisease).WithMany(p => p.AiFeedbacks)
            .HasForeignKey(d => d.CorrectedCropDiseaseId)
            .HasConstraintName("ai_feedback_corrected_crop_disease_id_fkey");

        entity.HasOne(d => d.DiseaseResult).WithMany(p => p.AiFeedbacks)
            .HasForeignKey(d => d.DiseaseResultId)
            .HasConstraintName("ai_feedback_disease_result_id_fkey");

        entity.HasOne(d => d.Recommendation).WithMany(p => p.AiFeedbacks)
            .HasForeignKey(d => d.RecommendationId)
            .HasConstraintName("ai_feedback_recommendation_id_fkey");

        entity.HasOne(d => d.User).WithMany(p => p.AiFeedbackUsers)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("ai_feedback_user_id_fkey");
    }
}
