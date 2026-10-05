using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class WeatherAlertConfiguration : IEntityTypeConfiguration<WeatherAlert>
{
    public void Configure(EntityTypeBuilder<WeatherAlert> entity)
    {
        entity.HasKey(e => e.Id).HasName("weather_alerts_pkey");

        entity.ToTable("weather_alerts");

        entity.HasIndex(e => e.StartsAt, "ix_weather_alerts_active").HasFilter("(status = 'active'::text)");

        entity.HasIndex(e => new { e.WeatherLocationId, e.StartsAt }, "ix_weather_alerts_location").IsDescending(false, true);

        entity.HasIndex(e => new { e.Source, e.ExternalId }, "weather_alerts_source_external_id_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AlertType).HasColumnName("alert_type");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.EndsAt).HasColumnName("ends_at");
        entity.Property(e => e.ExternalId).HasColumnName("external_id");
        entity.Property(e => e.Severity).HasColumnName("severity");
        entity.Property(e => e.Source).HasColumnName("source");
        entity.Property(e => e.StartsAt).HasColumnName("starts_at");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'active'::text")
            .HasColumnName("status");
        entity.Property(e => e.Title).HasColumnName("title");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.WeatherLocationId).HasColumnName("weather_location_id");

        entity.HasOne(d => d.WeatherLocation).WithMany(p => p.WeatherAlerts)
            .HasForeignKey(d => d.WeatherLocationId)
            .HasConstraintName("weather_alerts_weather_location_id_fkey");
    }
}
