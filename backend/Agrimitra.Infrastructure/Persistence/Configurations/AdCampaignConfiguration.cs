using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class AdCampaignConfiguration : IEntityTypeConfiguration<AdCampaign>
{
    public void Configure(EntityTypeBuilder<AdCampaign> entity)
    {
        entity.HasKey(e => e.Id).HasName("ad_campaigns_pkey");

        entity.ToTable("ad_campaigns");

        entity.HasIndex(e => new { e.StartAt, e.EndAt }, "ix_ad_campaigns_active_window").HasFilter("(status = ANY (ARRAY['scheduled'::text, 'active'::text]))");

        entity.HasIndex(e => new { e.AdvertiserId, e.Status }, "ix_ad_campaigns_advertiser").HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.SubmittedAt, "ix_ad_campaigns_approval").HasFilter("(status = 'pending_approval'::text)");

        entity.HasIndex(e => e.ApprovedBy, "ix_ad_campaigns_approved_by_7cc55e");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AdvertiserId).HasColumnName("advertiser_id");
        entity.Property(e => e.ApprovedAt).HasColumnName("approved_at");
        entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
        entity.Property(e => e.BidAmount)
            .HasPrecision(12, 4)
            .HasColumnName("bid_amount");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.Currency)
            .HasMaxLength(3)
            .HasDefaultValueSql("'INR'::bpchar")
            .IsFixedLength()
            .HasColumnName("currency");
        entity.Property(e => e.DailyBudget)
            .HasPrecision(14, 2)
            .HasColumnName("daily_budget");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.EndAt).HasColumnName("end_at");
        entity.Property(e => e.FrequencyCapPerDay).HasColumnName("frequency_cap_per_day");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.Objective).HasColumnName("objective");
        entity.Property(e => e.PricingModel).HasColumnName("pricing_model");
        entity.Property(e => e.RejectionReason).HasColumnName("rejection_reason");
        entity.Property(e => e.SpentAmount)
            .HasPrecision(14, 2)
            .HasColumnName("spent_amount");
        entity.Property(e => e.StartAt).HasColumnName("start_at");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'draft'::text")
            .HasColumnName("status");
        entity.Property(e => e.SubmittedAt).HasColumnName("submitted_at");
        entity.Property(e => e.TotalBudget)
            .HasPrecision(14, 2)
            .HasColumnName("total_budget");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Advertiser).WithMany(p => p.AdCampaigns)
            .HasForeignKey(d => d.AdvertiserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("ad_campaigns_advertiser_id_fkey");

        entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.AdCampaigns)
            .HasForeignKey(d => d.ApprovedBy)
            .HasConstraintName("ad_campaigns_approved_by_fkey");
    }
}
