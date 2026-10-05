using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ScheduleConflictConfiguration : IEntityTypeConfiguration<ScheduleConflict>
{
    public void Configure(EntityTypeBuilder<ScheduleConflict> entity)
    {
        entity.HasKey(e => e.Id).HasName("schedule_conflicts_pkey");

        entity.ToTable("schedule_conflicts");

        entity.HasIndex(e => e.OccurrenceId, "ix_conflicts_occurrence");

        entity.HasIndex(e => e.DetectedAt, "ix_conflicts_open").HasFilter("(status = ANY (ARRAY['open'::text, 'proposed'::text]))");

        entity.HasIndex(e => e.WeatherAlertId, "ix_conflicts_weather_alert");

        entity.HasIndex(e => e.WeatherDataId, "ix_conflicts_weather_data");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DetectedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("detected_at");
        entity.Property(e => e.Evidence)
            .HasDefaultValueSql("'{}'::jsonb")
            .HasColumnType("jsonb")
            .HasColumnName("evidence");
        entity.Property(e => e.Kind).HasColumnName("kind");
        entity.Property(e => e.OccurrenceId).HasColumnName("occurrence_id");
        entity.Property(e => e.ResolvedAt).HasColumnName("resolved_at");
        entity.Property(e => e.RuleCode).HasColumnName("rule_code");
        entity.Property(e => e.RuleVersion).HasColumnName("rule_version");
        entity.Property(e => e.Severity).HasColumnName("severity");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'open'::text")
            .HasColumnName("status");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.WeatherAlertId).HasColumnName("weather_alert_id");
        entity.Property(e => e.WeatherDataId).HasColumnName("weather_data_id");

        entity.HasOne(d => d.Occurrence).WithMany(p => p.ScheduleConflicts)
            .HasForeignKey(d => d.OccurrenceId)
            .HasConstraintName("schedule_conflicts_occurrence_id_fkey");

        entity.HasOne(d => d.WeatherAlert).WithMany(p => p.ScheduleConflicts)
            .HasForeignKey(d => d.WeatherAlertId)
            .HasConstraintName("schedule_conflicts_weather_alert_id_fkey");
    }
}
