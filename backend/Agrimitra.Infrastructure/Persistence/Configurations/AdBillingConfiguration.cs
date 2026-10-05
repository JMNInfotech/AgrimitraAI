using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class AdBillingConfiguration : IEntityTypeConfiguration<AdBilling>
{
    public void Configure(EntityTypeBuilder<AdBilling> entity)
    {
        entity.HasKey(e => e.Id).HasName("ad_billing_pkey");

        entity.ToTable("ad_billing");

        entity.HasIndex(e => e.InvoiceNumber, "ad_billing_invoice_number_key").IsUnique();

        entity.HasIndex(e => new { e.AdvertiserId, e.CreatedAt }, "ix_ad_billing_advertiser").IsDescending(false, true);

        entity.HasIndex(e => e.CampaignId, "ix_ad_billing_campaign");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AdvertiserId).HasColumnName("advertiser_id");
        entity.Property(e => e.Amount)
            .HasPrecision(14, 2)
            .HasColumnName("amount");
        entity.Property(e => e.BillingType).HasColumnName("billing_type");
        entity.Property(e => e.CampaignId).HasColumnName("campaign_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.Currency)
            .HasMaxLength(3)
            .HasDefaultValueSql("'INR'::bpchar")
            .IsFixedLength()
            .HasColumnName("currency");
        entity.Property(e => e.InvoiceNumber).HasColumnName("invoice_number");
        entity.Property(e => e.PaidAt).HasColumnName("paid_at");
        entity.Property(e => e.PeriodEnd).HasColumnName("period_end");
        entity.Property(e => e.PeriodStart).HasColumnName("period_start");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'pending'::text")
            .HasColumnName("status");
        entity.Property(e => e.TaxAmount)
            .HasPrecision(14, 2)
            .HasColumnName("tax_amount");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Advertiser).WithMany(p => p.AdBillings)
            .HasForeignKey(d => d.AdvertiserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("ad_billing_advertiser_id_fkey");

        entity.HasOne(d => d.Campaign).WithMany(p => p.AdBillings)
            .HasForeignKey(d => d.CampaignId)
            .HasConstraintName("ad_billing_campaign_id_fkey");
    }
}
