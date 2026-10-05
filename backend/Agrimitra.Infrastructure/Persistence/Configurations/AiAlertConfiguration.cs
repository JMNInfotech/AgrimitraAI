using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class AiAlertConfiguration : IEntityTypeConfiguration<AiAlert>
{
    public void Configure(EntityTypeBuilder<AiAlert> entity)
    {
        entity.HasKey(e => e.Id).HasName("ai_alerts_pkey");

        entity.ToTable("ai_alerts");

        entity.HasIndex(e => e.CropCycleId, "ix_ai_alerts_cycle");

        entity.HasIndex(e => new { e.FarmerProfileId, e.CreatedAt }, "ix_ai_alerts_farmer").IsDescending(false, true);

        entity.HasIndex(e => e.LandId, "ix_ai_alerts_land");

        entity.HasIndex(e => e.OccurrenceId, "ix_ai_alerts_occurrence");

        entity.HasIndex(e => e.WeatherAlertId, "ix_ai_alerts_weather_alert_id_afa548");

        entity.HasIndex(e => new { e.FarmerProfileId, e.DedupeKey }, "ux_ai_alerts_dedupe")
            .IsUnique()
            .HasFilter("((dedupe_key IS NOT NULL) AND (status = ANY (ARRAY['new'::text, 'delivered'::text])))");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AlertType).HasColumnName("alert_type");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropCycleId).HasColumnName("crop_cycle_id");
        entity.Property(e => e.DedupeKey).HasColumnName("dedupe_key");
        entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.LandId).HasColumnName("land_id");
        entity.Property(e => e.Message).HasColumnName("message");
        entity.Property(e => e.OccurrenceId).HasColumnName("occurrence_id");
        entity.Property(e => e.RuleCode).HasColumnName("rule_code");
        entity.Property(e => e.Severity)
            .HasDefaultValueSql("'info'::text")
            .HasColumnName("severity");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'new'::text")
            .HasColumnName("status");
        entity.Property(e => e.Title).HasColumnName("title");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.WeatherAlertId).HasColumnName("weather_alert_id");

        entity.HasOne(d => d.CropCycle).WithMany(p => p.AiAlerts)
            .HasForeignKey(d => d.CropCycleId)
            .HasConstraintName("ai_alerts_crop_cycle_id_fkey");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.AiAlerts)
            .HasForeignKey(d => d.FarmerProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("ai_alerts_farmer_profile_id_fkey");

        entity.HasOne(d => d.Land).WithMany(p => p.AiAlerts)
            .HasForeignKey(d => d.LandId)
            .HasConstraintName("ai_alerts_land_id_fkey");

        entity.HasOne(d => d.Occurrence).WithMany(p => p.AiAlerts)
            .HasForeignKey(d => d.OccurrenceId)
            .HasConstraintName("ai_alerts_occurrence_id_fkey");

        entity.HasOne(d => d.WeatherAlert).WithMany(p => p.AiAlerts)
            .HasForeignKey(d => d.WeatherAlertId)
            .HasConstraintName("ai_alerts_weather_alert_id_fkey");
    }
}
