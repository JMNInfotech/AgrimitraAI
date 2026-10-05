using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class AdDailyRollupConfiguration : IEntityTypeConfiguration<AdDailyRollup>
{
    public void Configure(EntityTypeBuilder<AdDailyRollup> entity)
    {
        entity.HasKey(e => new { e.CampaignId, e.CreativeId, e.PlacementId, e.Day }).HasName("ad_daily_rollups_pkey");

        entity.ToTable("ad_daily_rollups");

        entity.HasIndex(e => e.CreativeId, "ix_ad_rollups_creative");

        entity.HasIndex(e => e.Day, "ix_ad_rollups_day");

        entity.HasIndex(e => e.PlacementId, "ix_ad_rollups_placement");

        entity.Property(e => e.CampaignId).HasColumnName("campaign_id");
        entity.Property(e => e.CreativeId).HasColumnName("creative_id");
        entity.Property(e => e.PlacementId).HasColumnName("placement_id");
        entity.Property(e => e.Day).HasColumnName("day");
        entity.Property(e => e.AttributedRevenue)
            .HasPrecision(14, 2)
            .HasColumnName("attributed_revenue");
        entity.Property(e => e.Clicks).HasColumnName("clicks");
        entity.Property(e => e.Conversions).HasColumnName("conversions");
        entity.Property(e => e.Impressions).HasColumnName("impressions");
        entity.Property(e => e.Spend)
            .HasPrecision(14, 4)
            .HasColumnName("spend");
        entity.Property(e => e.UniqueImpressions).HasColumnName("unique_impressions");

        entity.HasOne(d => d.Campaign).WithMany(p => p.AdDailyRollups)
            .HasForeignKey(d => d.CampaignId)
            .HasConstraintName("ad_daily_rollups_campaign_id_fkey");

        entity.HasOne(d => d.Creative).WithMany(p => p.AdDailyRollups)
            .HasForeignKey(d => d.CreativeId)
            .HasConstraintName("ad_daily_rollups_creative_id_fkey");

        entity.HasOne(d => d.Placement).WithMany(p => p.AdDailyRollups)
            .HasForeignKey(d => d.PlacementId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("ad_daily_rollups_placement_id_fkey");
    }
}
