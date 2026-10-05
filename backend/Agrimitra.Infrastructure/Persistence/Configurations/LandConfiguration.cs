using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class LandConfiguration : IEntityTypeConfiguration<Land>
{
    public void Configure(EntityTypeBuilder<Land> entity)
    {
        entity.HasKey(e => e.Id).HasName("lands_pkey");

        entity.ToTable("lands");

        entity.HasIndex(e => e.AddressId, "ix_lands_address");

        entity.HasIndex(e => e.FarmId, "ix_lands_farm").HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => new { e.FarmerProfileId, e.Status }, "ix_lands_farmer").HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.IrrigationTypeId, "ix_lands_irrigation");

        entity.HasIndex(e => e.Location, "ix_lands_location").HasMethod("gist");

        entity.HasIndex(e => e.OrganizationId, "ix_lands_organization_id_39e682");

        entity.HasIndex(e => e.SoilTypeId, "ix_lands_soil");

        entity.HasIndex(e => e.WaterSourceId, "ix_lands_water");

        entity.HasIndex(e => new { e.FarmId, e.SurveyNumber }, "ux_lands_farm_survey")
            .IsUnique()
            .HasFilter("((survey_number IS NOT NULL) AND (NOT is_deleted))");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AddressId).HasColumnName("address_id");
        entity.Property(e => e.AreaSqMeters)
            .HasPrecision(16, 2)
            .HasComputedColumnSql("(area_value *\nCASE area_unit\n    WHEN 'acre'::text THEN 4046.8564224\n    WHEN 'hectare'::text THEN (10000)::numeric\n    WHEN 'guntha'::text THEN 101.17141056\n    ELSE (1)::numeric\nEND)", true)
            .HasColumnName("area_sq_meters");
        entity.Property(e => e.AreaUnit).HasColumnName("area_unit");
        entity.Property(e => e.AreaValue)
            .HasPrecision(14, 4)
            .HasColumnName("area_value");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.FarmId).HasColumnName("farm_id");
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.IrrigationTypeId).HasColumnName("irrigation_type_id");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.Location)
            .HasColumnType("geography(Point,4326)")
            .HasColumnName("location");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.Notes).HasColumnName("notes");
        entity.Property(e => e.OrganizationId).HasColumnName("organization_id");
        entity.Property(e => e.OwnershipType)
            .HasDefaultValueSql("'owned'::text")
            .HasColumnName("ownership_type");
        entity.Property(e => e.SoilTypeId).HasColumnName("soil_type_id");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'active'::text")
            .HasColumnName("status");
        entity.Property(e => e.SurveyNumber).HasColumnName("survey_number");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.WaterSourceId).HasColumnName("water_source_id");

        entity.HasOne(d => d.Address).WithMany(p => p.Lands)
            .HasForeignKey(d => d.AddressId)
            .HasConstraintName("lands_address_id_fkey");

        entity.HasOne(d => d.Farm).WithMany(p => p.Lands)
            .HasForeignKey(d => d.FarmId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("lands_farm_id_fkey");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.Lands)
            .HasForeignKey(d => d.FarmerProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("lands_farmer_profile_id_fkey");

        entity.HasOne(d => d.IrrigationType).WithMany(p => p.Lands)
            .HasForeignKey(d => d.IrrigationTypeId)
            .HasConstraintName("lands_irrigation_type_id_fkey");

        entity.HasOne(d => d.Organization).WithMany(p => p.Lands)
            .HasForeignKey(d => d.OrganizationId)
            .HasConstraintName("lands_organization_id_fkey");

        entity.HasOne(d => d.SoilType).WithMany(p => p.Lands)
            .HasForeignKey(d => d.SoilTypeId)
            .HasConstraintName("lands_soil_type_id_fkey");

        entity.HasOne(d => d.WaterSource).WithMany(p => p.Lands)
            .HasForeignKey(d => d.WaterSourceId)
            .HasConstraintName("lands_water_source_id_fkey");
    }
}
