using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class AiRecommendationConfiguration : IEntityTypeConfiguration<AiRecommendation>
{
    public void Configure(EntityTypeBuilder<AiRecommendation> entity)
    {
        entity.HasKey(e => e.Id).HasName("ai_recommendations_pkey");

        entity.ToTable("ai_recommendations");

        entity.HasIndex(e => e.EscalatedConsultationId, "ix_ai_recommendations_escalated_consultation_id_e25446");

        entity.HasIndex(e => e.LanguageCode, "ix_ai_recommendations_language_code_5f647a");

        entity.HasIndex(e => e.CropCycleId, "ix_recommendations_cycle");

        entity.HasIndex(e => e.CreatedAt, "ix_recommendations_escalation").HasFilter("escalation_required");

        entity.HasIndex(e => new { e.FarmerProfileId, e.CreatedAt }, "ix_recommendations_farmer").IsDescending(false, true);

        entity.HasIndex(e => e.LandId, "ix_recommendations_land");

        entity.HasIndex(e => e.ModelVersionId, "ix_recommendations_model");

        entity.HasIndex(e => e.DiseaseResultId, "ix_recommendations_result");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Confidence)
            .HasPrecision(6, 5)
            .HasColumnName("confidence");
        entity.Property(e => e.Content).HasColumnName("content");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropCycleId).HasColumnName("crop_cycle_id");
        entity.Property(e => e.Disclaimer).HasColumnName("disclaimer");
        entity.Property(e => e.DiseaseResultId).HasColumnName("disease_result_id");
        entity.Property(e => e.EscalatedConsultationId).HasColumnName("escalated_consultation_id");
        entity.Property(e => e.EscalationRequired).HasColumnName("escalation_required");
        entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.Kind).HasColumnName("kind");
        entity.Property(e => e.LandId).HasColumnName("land_id");
        entity.Property(e => e.LanguageCode).HasColumnName("language_code");
        entity.Property(e => e.ModelVersionId).HasColumnName("model_version_id");
        entity.Property(e => e.RiskLevel)
            .HasDefaultValueSql("'low'::text")
            .HasColumnName("risk_level");
        entity.Property(e => e.Sources)
            .HasDefaultValueSql("'[]'::jsonb")
            .HasColumnType("jsonb")
            .HasColumnName("sources");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'shown'::text")
            .HasColumnName("status");
        entity.Property(e => e.Title).HasColumnName("title");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.CropCycle).WithMany(p => p.AiRecommendations)
            .HasForeignKey(d => d.CropCycleId)
            .HasConstraintName("ai_recommendations_crop_cycle_id_fkey");

        entity.HasOne(d => d.DiseaseResult).WithMany(p => p.AiRecommendations)
            .HasForeignKey(d => d.DiseaseResultId)
            .HasConstraintName("ai_recommendations_disease_result_id_fkey");

        entity.HasOne(d => d.EscalatedConsultation).WithMany(p => p.AiRecommendations)
            .HasForeignKey(d => d.EscalatedConsultationId)
            .HasConstraintName("ai_recommendations_escalated_consultation_id_fkey");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.AiRecommendations)
            .HasForeignKey(d => d.FarmerProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("ai_recommendations_farmer_profile_id_fkey");

        entity.HasOne(d => d.Land).WithMany(p => p.AiRecommendations)
            .HasForeignKey(d => d.LandId)
            .HasConstraintName("ai_recommendations_land_id_fkey");

        entity.HasOne(d => d.LanguageCodeNavigation).WithMany(p => p.AiRecommendations)
            .HasForeignKey(d => d.LanguageCode)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("ai_recommendations_language_code_fkey");

        entity.HasOne(d => d.ModelVersion).WithMany(p => p.AiRecommendations)
            .HasForeignKey(d => d.ModelVersionId)
            .HasConstraintName("ai_recommendations_model_version_id_fkey");
    }
}
