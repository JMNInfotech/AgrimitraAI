using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class WeatherLocationConfiguration : IEntityTypeConfiguration<WeatherLocation>
{
    public void Configure(EntityTypeBuilder<WeatherLocation> entity)
    {
        entity.HasKey(e => e.Id).HasName("weather_locations_pkey");

        entity.ToTable("weather_locations");

        entity.HasIndex(e => e.Location, "ix_weather_locations_geo").HasMethod("gist");

        entity.HasIndex(e => e.LandId, "ix_weather_locations_land");

        entity.HasIndex(e => e.VillageId, "ix_weather_locations_village");

        entity.HasIndex(e => new { e.Provider, e.GridKey }, "weather_locations_provider_grid_key_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.GridKey).HasColumnName("grid_key");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasColumnName("is_active");
        entity.Property(e => e.LandId).HasColumnName("land_id");
        entity.Property(e => e.LastFetchedAt).HasColumnName("last_fetched_at");
        entity.Property(e => e.Location)
            .HasColumnType("geography(Point,4326)")
            .HasColumnName("location");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.Provider).HasColumnName("provider");
        entity.Property(e => e.ProviderLocationId).HasColumnName("provider_location_id");
        entity.Property(e => e.TimeZoneId)
            .HasDefaultValueSql("'Asia/Kolkata'::text")
            .HasColumnName("time_zone_id");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.VillageId).HasColumnName("village_id");

        entity.HasOne(d => d.Land).WithMany(p => p.WeatherLocations)
            .HasForeignKey(d => d.LandId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("weather_locations_land_id_fkey");

        entity.HasOne(d => d.Village).WithMany(p => p.WeatherLocations)
            .HasForeignKey(d => d.VillageId)
            .HasConstraintName("weather_locations_village_id_fkey");
    }
}
