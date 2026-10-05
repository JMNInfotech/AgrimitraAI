using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class LaboratoryProfileConfiguration : IEntityTypeConfiguration<LaboratoryProfile>
{
    public void Configure(EntityTypeBuilder<LaboratoryProfile> entity)
    {
        entity.HasKey(e => e.Id).HasName("laboratory_profiles_pkey");

        entity.ToTable("laboratory_profiles");

        entity.HasIndex(e => e.AddressId, "ix_laboratory_profiles_address_id_62dc8c");

        entity.HasIndex(e => e.Location, "ix_laboratory_profiles_location").HasMethod("gist");

        entity.HasIndex(e => e.Name, "ix_laboratory_profiles_name_trgm")
            .HasMethod("gin")
            .HasOperators(new[] { "gin_trgm_ops" });

        entity.HasIndex(e => e.ProfileImageId, "ix_laboratory_profiles_profile_image_id_57a3f9");

        entity.HasIndex(e => e.UserId, "ix_laboratory_profiles_user");

        entity.HasIndex(e => e.VerificationStatus, "ix_laboratory_profiles_verified").HasFilter("(NOT is_deleted)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AccreditationNumber).HasColumnName("accreditation_number");
        entity.Property(e => e.AddressId).HasColumnName("address_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.Email)
            .HasColumnType("citext")
            .HasColumnName("email");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasColumnName("is_active");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.Location)
            .HasColumnType("geography(Point,4326)")
            .HasColumnName("location");
        entity.Property(e => e.MobileNumber).HasColumnName("mobile_number");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.ProfileImageId).HasColumnName("profile_image_id");
        entity.Property(e => e.RatingAverage)
            .HasPrecision(3, 2)
            .HasColumnName("rating_average");
        entity.Property(e => e.RatingCount).HasColumnName("rating_count");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.UserId).HasColumnName("user_id");
        entity.Property(e => e.VerificationStatus)
            .HasDefaultValueSql("'unverified'::text")
            .HasColumnName("verification_status");
        entity.Property(e => e.VerifiedAt).HasColumnName("verified_at");

        entity.HasOne(d => d.Address).WithMany(p => p.LaboratoryProfiles)
            .HasForeignKey(d => d.AddressId)
            .HasConstraintName("laboratory_profiles_address_id_fkey");

        entity.HasOne(d => d.ProfileImage).WithMany(p => p.LaboratoryProfiles)
            .HasForeignKey(d => d.ProfileImageId)
            .HasConstraintName("laboratory_profiles_profile_image_id_fkey");

        entity.HasOne(d => d.User).WithMany(p => p.LaboratoryProfiles)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("laboratory_profiles_user_id_fkey");
    }
}
