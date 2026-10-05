using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class LandWeatherLocationConfiguration : IEntityTypeConfiguration<LandWeatherLocation>
{
    public void Configure(EntityTypeBuilder<LandWeatherLocation> entity)
    {
        entity.HasKey(e => e.LandId).HasName("land_weather_locations_pkey");

        entity.ToTable("land_weather_locations");

        entity.HasIndex(e => e.WeatherLocationId, "ix_land_weather_location");

        entity.Property(e => e.LandId)
            .ValueGeneratedNever()
            .HasColumnName("land_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DistanceMeters)
            .HasPrecision(10, 1)
            .HasColumnName("distance_meters");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.WeatherLocationId).HasColumnName("weather_location_id");

        entity.HasOne(d => d.Land).WithOne(p => p.LandWeatherLocation)
            .HasForeignKey<LandWeatherLocation>(d => d.LandId)
            .HasConstraintName("land_weather_locations_land_id_fkey");

        entity.HasOne(d => d.WeatherLocation).WithMany(p => p.LandWeatherLocations)
            .HasForeignKey(d => d.WeatherLocationId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("land_weather_locations_weather_location_id_fkey");
    }
}
