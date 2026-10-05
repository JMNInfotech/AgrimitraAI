using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class AdCreativeConfiguration : IEntityTypeConfiguration<AdCreative>
{
    public void Configure(EntityTypeBuilder<AdCreative> entity)
    {
        entity.HasKey(e => e.Id).HasName("ad_creatives_pkey");

        entity.ToTable("ad_creatives");

        entity.HasIndex(e => e.CampaignId, "ix_ad_creatives_campaign").HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.CategoryId, "ix_ad_creatives_category");

        entity.HasIndex(e => e.ImageFileId, "ix_ad_creatives_image_file_id_9b2fa6");

        entity.HasIndex(e => e.LanguageCode, "ix_ad_creatives_language_code_6bc415");

        entity.HasIndex(e => e.ProductId, "ix_ad_creatives_product");

        entity.HasIndex(e => e.ReviewedBy, "ix_ad_creatives_reviewed_by_b3cae2");

        entity.HasIndex(e => e.VideoFileId, "ix_ad_creatives_video_file_id_b3e30b");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AdType).HasColumnName("ad_type");
        entity.Property(e => e.CampaignId).HasColumnName("campaign_id");
        entity.Property(e => e.CategoryId).HasColumnName("category_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CtaLabel).HasColumnName("cta_label");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.ImageFileId).HasColumnName("image_file_id");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.LandingUrl).HasColumnName("landing_url");
        entity.Property(e => e.LanguageCode).HasColumnName("language_code");
        entity.Property(e => e.ProductId).HasColumnName("product_id");
        entity.Property(e => e.ReviewedAt).HasColumnName("reviewed_at");
        entity.Property(e => e.ReviewedBy).HasColumnName("reviewed_by");
        entity.Property(e => e.SponsoredLabel)
            .HasDefaultValueSql("'Sponsored'::text")
            .HasColumnName("sponsored_label");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'draft'::text")
            .HasColumnName("status");
        entity.Property(e => e.Title).HasColumnName("title");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.VideoFileId).HasColumnName("video_file_id");

        entity.HasOne(d => d.Campaign).WithMany(p => p.AdCreatives)
            .HasForeignKey(d => d.CampaignId)
            .HasConstraintName("ad_creatives_campaign_id_fkey");

        entity.HasOne(d => d.Category).WithMany(p => p.AdCreatives)
            .HasForeignKey(d => d.CategoryId)
            .HasConstraintName("ad_creatives_category_id_fkey");

        entity.HasOne(d => d.ImageFile).WithMany(p => p.AdCreativeImageFiles)
            .HasForeignKey(d => d.ImageFileId)
            .HasConstraintName("ad_creatives_image_file_id_fkey");

        entity.HasOne(d => d.LanguageCodeNavigation).WithMany(p => p.AdCreatives)
            .HasForeignKey(d => d.LanguageCode)
            .HasConstraintName("ad_creatives_language_code_fkey");

        entity.HasOne(d => d.Product).WithMany(p => p.AdCreatives)
            .HasForeignKey(d => d.ProductId)
            .HasConstraintName("ad_creatives_product_id_fkey");

        entity.HasOne(d => d.ReviewedByNavigation).WithMany(p => p.AdCreatives)
            .HasForeignKey(d => d.ReviewedBy)
            .HasConstraintName("ad_creatives_reviewed_by_fkey");

        entity.HasOne(d => d.VideoFile).WithMany(p => p.AdCreativeVideoFiles)
            .HasForeignKey(d => d.VideoFileId)
            .HasConstraintName("ad_creatives_video_file_id_fkey");
    }
}
