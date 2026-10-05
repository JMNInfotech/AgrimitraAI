using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ShopProfileConfiguration : IEntityTypeConfiguration<ShopProfile>
{
    public void Configure(EntityTypeBuilder<ShopProfile> entity)
    {
        entity.HasKey(e => e.Id).HasName("shop_profiles_pkey");

        entity.ToTable("shop_profiles");

        entity.HasIndex(e => e.AddressId, "ix_shop_profiles_address_id_9f6cb3");

        entity.HasIndex(e => e.Location, "ix_shop_profiles_location").HasMethod("gist");

        entity.HasIndex(e => e.ShopName, "ix_shop_profiles_name_trgm")
            .HasMethod("gin")
            .HasOperators(new[] { "gin_trgm_ops" });

        entity.HasIndex(e => e.ShopImageId, "ix_shop_profiles_shop_image_id_5a5ec4");

        entity.HasIndex(e => e.UserId, "ix_shop_profiles_user");

        entity.HasIndex(e => e.VerificationStatus, "ix_shop_profiles_verified").HasFilter("(NOT is_deleted)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AddressId).HasColumnName("address_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.Email)
            .HasColumnType("citext")
            .HasColumnName("email");
        entity.Property(e => e.Gstin).HasColumnName("gstin");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasColumnName("is_active");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.LicenseNumber).HasColumnName("license_number");
        entity.Property(e => e.LicenseType).HasColumnName("license_type");
        entity.Property(e => e.LicenseValidUntil).HasColumnName("license_valid_until");
        entity.Property(e => e.Location)
            .HasColumnType("geography(Point,4326)")
            .HasColumnName("location");
        entity.Property(e => e.MobileNumber).HasColumnName("mobile_number");
        entity.Property(e => e.OwnerName).HasColumnName("owner_name");
        entity.Property(e => e.RatingAverage)
            .HasPrecision(3, 2)
            .HasColumnName("rating_average");
        entity.Property(e => e.RatingCount).HasColumnName("rating_count");
        entity.Property(e => e.ShopImageId).HasColumnName("shop_image_id");
        entity.Property(e => e.ShopName).HasColumnName("shop_name");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.UserId).HasColumnName("user_id");
        entity.Property(e => e.VerificationStatus)
            .HasDefaultValueSql("'unverified'::text")
            .HasColumnName("verification_status");
        entity.Property(e => e.VerifiedAt).HasColumnName("verified_at");

        entity.HasOne(d => d.Address).WithMany(p => p.ShopProfiles)
            .HasForeignKey(d => d.AddressId)
            .HasConstraintName("shop_profiles_address_id_fkey");

        entity.HasOne(d => d.ShopImage).WithMany(p => p.ShopProfiles)
            .HasForeignKey(d => d.ShopImageId)
            .HasConstraintName("shop_profiles_shop_image_id_fkey");

        entity.HasOne(d => d.User).WithMany(p => p.ShopProfiles)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("shop_profiles_user_id_fkey");
    }
}
