using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class AdBudgetLedgerConfiguration : IEntityTypeConfiguration<AdBudgetLedger>
{
    public void Configure(EntityTypeBuilder<AdBudgetLedger> entity)
    {
        entity.HasKey(e => e.Id).HasName("ad_budget_ledger_pkey");

        entity.ToTable("ad_budget_ledger");

        entity.HasIndex(e => new { e.AdvertiserId, e.CreatedAt }, "ix_ad_ledger_advertiser").IsDescending(false, true);

        entity.HasIndex(e => new { e.CampaignId, e.CreatedAt }, "ix_ad_ledger_campaign").IsDescending(false, true);

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AdvertiserId).HasColumnName("advertiser_id");
        entity.Property(e => e.Amount)
            .HasPrecision(14, 4)
            .HasColumnName("amount");
        entity.Property(e => e.BalanceAfter)
            .HasPrecision(14, 4)
            .HasColumnName("balance_after");
        entity.Property(e => e.CampaignId).HasColumnName("campaign_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.EntryType).HasColumnName("entry_type");
        entity.Property(e => e.ReferenceId).HasColumnName("reference_id");

        entity.HasOne(d => d.Advertiser).WithMany(p => p.AdBudgetLedgers)
            .HasForeignKey(d => d.AdvertiserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("ad_budget_ledger_advertiser_id_fkey");

        entity.HasOne(d => d.Campaign).WithMany(p => p.AdBudgetLedgers)
            .HasForeignKey(d => d.CampaignId)
            .HasConstraintName("ad_budget_ledger_campaign_id_fkey");
    }
}
