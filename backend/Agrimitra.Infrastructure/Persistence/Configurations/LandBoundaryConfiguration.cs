using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class LandBoundaryConfiguration : IEntityTypeConfiguration<LandBoundary>
{
    public void Configure(EntityTypeBuilder<LandBoundary> entity)
    {
        entity.HasKey(e => e.Id).HasName("land_boundaries_pkey");

        entity.ToTable("land_boundaries");

        entity.HasIndex(e => e.Boundary, "ix_land_boundaries_gist").HasMethod("gist");

        entity.HasIndex(e => new { e.LandId, e.Version }, "land_boundaries_land_id_version_key").IsUnique();

        entity.HasIndex(e => e.LandId, "ux_land_boundaries_current")
            .IsUnique()
            .HasFilter("is_current");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AreaSqMeters)
            .HasPrecision(16, 2)
            .HasComputedColumnSql("round((st_area(boundary))::numeric, 2)", true)
            .HasColumnName("area_sq_meters");
        entity.Property(e => e.Boundary)
            .HasColumnType("geography(Polygon,4326)")
            .HasColumnName("boundary");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.IsCurrent)
            .HasDefaultValue(true)
            .HasColumnName("is_current");
        entity.Property(e => e.LandId).HasColumnName("land_id");
        entity.Property(e => e.Source)
            .HasDefaultValueSql("'drawn'::text")
            .HasColumnName("source");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.Version)
            .HasDefaultValue(1)
            .HasColumnName("version");

        entity.HasOne(d => d.Land).WithOne(p => p.LandBoundary)
            .HasForeignKey<LandBoundary>(d => d.LandId)
            .HasConstraintName("land_boundaries_land_id_fkey");
    }
}
