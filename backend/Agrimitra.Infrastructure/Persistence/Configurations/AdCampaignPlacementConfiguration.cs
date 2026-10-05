using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class AdCampaignPlacementConfiguration : IEntityTypeConfiguration<AdCampaignPlacement>
{
    public void Configure(EntityTypeBuilder<AdCampaignPlacement> entity)
    {
        entity.HasKey(e => new { e.CampaignId, e.PlacementId }).HasName("ad_campaign_placements_pkey");

        entity.ToTable("ad_campaign_placements");

        entity.HasIndex(e => e.PlacementId, "ix_ad_campaign_placements_placement");

        entity.Property(e => e.CampaignId).HasColumnName("campaign_id");
        entity.Property(e => e.PlacementId).HasColumnName("placement_id");
        entity.Property(e => e.Priority).HasColumnName("priority");

        entity.HasOne(d => d.Campaign).WithMany(p => p.AdCampaignPlacements)
            .HasForeignKey(d => d.CampaignId)
            .HasConstraintName("ad_campaign_placements_campaign_id_fkey");

        entity.HasOne(d => d.Placement).WithMany(p => p.AdCampaignPlacements)
            .HasForeignKey(d => d.PlacementId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("ad_campaign_placements_placement_id_fkey");
    }
}
