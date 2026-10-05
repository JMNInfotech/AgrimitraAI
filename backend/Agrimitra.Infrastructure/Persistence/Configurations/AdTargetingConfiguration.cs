using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class AdTargetingConfiguration : IEntityTypeConfiguration<AdTargeting>
{
    public void Configure(EntityTypeBuilder<AdTargeting> entity)
    {
        entity.HasKey(e => e.Id).HasName("ad_targeting_pkey");

        entity.ToTable("ad_targeting");

        entity.HasIndex(e => e.CampaignId, "ad_targeting_campaign_id_key").IsUnique();

        entity.HasIndex(e => e.CenterLocation, "ix_ad_targeting_center").HasMethod("gist");

        entity.HasIndex(e => e.CropIds, "ix_ad_targeting_crops").HasMethod("gin");

        entity.HasIndex(e => e.DistrictIds, "ix_ad_targeting_districts").HasMethod("gin");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CampaignId).HasColumnName("campaign_id");
        entity.Property(e => e.CenterLocation)
            .HasColumnType("geography(Point,4326)")
            .HasColumnName("center_location");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropIds)
            .HasDefaultValueSql("'{}'::uuid[]")
            .HasColumnName("crop_ids");
        entity.Property(e => e.CropStageCodes)
            .HasDefaultValueSql("'{}'::text[]")
            .HasColumnName("crop_stage_codes");
        entity.Property(e => e.DistrictIds)
            .HasDefaultValueSql("'{}'::uuid[]")
            .HasColumnName("district_ids");
        entity.Property(e => e.FarmSizeMaxSqM)
            .HasPrecision(14, 2)
            .HasColumnName("farm_size_max_sq_m");
        entity.Property(e => e.FarmSizeMinSqM)
            .HasPrecision(14, 2)
            .HasColumnName("farm_size_min_sq_m");
        entity.Property(e => e.FarmerSegments)
            .HasDefaultValueSql("'{}'::text[]")
            .HasColumnName("farmer_segments");
        entity.Property(e => e.InterestCategoryIds)
            .HasDefaultValueSql("'{}'::uuid[]")
            .HasColumnName("interest_category_ids");
        entity.Property(e => e.LanguageCodes)
            .HasDefaultValueSql("'{}'::text[]")
            .HasColumnName("language_codes");
        entity.Property(e => e.RadiusKm)
            .HasPrecision(8, 2)
            .HasColumnName("radius_km");
        entity.Property(e => e.Seasons)
            .HasDefaultValueSql("'{}'::text[]")
            .HasColumnName("seasons");
        entity.Property(e => e.StateIds)
            .HasDefaultValueSql("'{}'::uuid[]")
            .HasColumnName("state_ids");
        entity.Property(e => e.TalukaIds)
            .HasDefaultValueSql("'{}'::uuid[]")
            .HasColumnName("taluka_ids");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Campaign).WithOne(p => p.AdTargeting)
            .HasForeignKey<AdTargeting>(d => d.CampaignId)
            .HasConstraintName("ad_targeting_campaign_id_fkey");
    }
}
