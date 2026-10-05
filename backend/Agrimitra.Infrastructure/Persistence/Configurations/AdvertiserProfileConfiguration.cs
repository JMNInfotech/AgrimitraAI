using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class AdvertiserProfileConfiguration : IEntityTypeConfiguration<AdvertiserProfile>
{
    public void Configure(EntityTypeBuilder<AdvertiserProfile> entity)
    {
        entity.HasKey(e => e.Id).HasName("advertiser_profiles_pkey");

        entity.ToTable("advertiser_profiles");

        entity.HasIndex(e => e.BillingAddressId, "ix_advertiser_profiles_billing_address_id_8ba0f8");

        entity.HasIndex(e => e.BrandProfileId, "ix_advertiser_profiles_brand");

        entity.HasIndex(e => e.UserId, "ix_advertiser_profiles_user");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.BillingAddressId).HasColumnName("billing_address_id");
        entity.Property(e => e.BrandProfileId).HasColumnName("brand_profile_id");
        entity.Property(e => e.CompanyName).HasColumnName("company_name");
        entity.Property(e => e.ContactEmail)
            .HasColumnType("citext")
            .HasColumnName("contact_email");
        entity.Property(e => e.ContactMobile).HasColumnName("contact_mobile");
        entity.Property(e => e.ContactName).HasColumnName("contact_name");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.Gstin).HasColumnName("gstin");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasColumnName("is_active");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.UserId).HasColumnName("user_id");
        entity.Property(e => e.VerificationStatus)
            .HasDefaultValueSql("'unverified'::text")
            .HasColumnName("verification_status");

        entity.HasOne(d => d.BillingAddress).WithMany(p => p.AdvertiserProfiles)
            .HasForeignKey(d => d.BillingAddressId)
            .HasConstraintName("advertiser_profiles_billing_address_id_fkey");

        entity.HasOne(d => d.BrandProfile).WithMany(p => p.AdvertiserProfiles)
            .HasForeignKey(d => d.BrandProfileId)
            .HasConstraintName("advertiser_profiles_brand_profile_id_fkey");

        entity.HasOne(d => d.User).WithMany(p => p.AdvertiserProfiles)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("advertiser_profiles_user_id_fkey");
    }
}
