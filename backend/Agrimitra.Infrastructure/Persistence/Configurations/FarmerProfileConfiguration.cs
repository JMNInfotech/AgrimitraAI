using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class FarmerProfileConfiguration : IEntityTypeConfiguration<FarmerProfile>
{
    public void Configure(EntityTypeBuilder<FarmerProfile> entity)
    {
        entity.HasKey(e => e.Id).HasName("farmer_profiles_pkey");

        entity.ToTable("farmer_profiles");

        entity.HasIndex(e => e.FarmerCode, "farmer_profiles_farmer_code_key").IsUnique();

        entity.HasIndex(e => e.UserId, "farmer_profiles_user_id_key").IsUnique();

        entity.HasIndex(e => e.AddressId, "ix_farmer_profiles_address");

        entity.HasIndex(e => e.HomeLocation, "ix_farmer_profiles_location").HasMethod("gist");

        entity.HasIndex(e => e.FullName, "ix_farmer_profiles_name_trgm")
            .HasMethod("gin")
            .HasOperators(new[] { "gin_trgm_ops" });

        entity.HasIndex(e => e.OrganizationId, "ix_farmer_profiles_org").HasFilter("(organization_id IS NOT NULL)");

        entity.HasIndex(e => e.PhotoFileId, "ix_farmer_profiles_photo_file_id_904e6f");

        entity.HasIndex(e => e.GovIdBlindIndex, "ux_farmer_profiles_govid")
            .IsUnique()
            .HasFilter("((gov_id_blind_index IS NOT NULL) AND (NOT is_deleted))");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AddressId).HasColumnName("address_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.FarmerCode).HasColumnName("farmer_code");
        entity.Property(e => e.FarmerSegment).HasColumnName("farmer_segment");
        entity.Property(e => e.FullName).HasColumnName("full_name");
        entity.Property(e => e.Gender).HasColumnName("gender");
        entity.Property(e => e.GovIdBlindIndex).HasColumnName("gov_id_blind_index");
        entity.Property(e => e.GovIdEncrypted).HasColumnName("gov_id_encrypted");
        entity.Property(e => e.GovIdLast4).HasColumnName("gov_id_last4");
        entity.Property(e => e.GovIdType).HasColumnName("gov_id_type");
        entity.Property(e => e.HomeLocation)
            .HasColumnType("geography(Point,4326)")
            .HasColumnName("home_location");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.OnboardingState)
            .HasDefaultValueSql("'language'::text")
            .HasColumnName("onboarding_state");
        entity.Property(e => e.OrganizationId).HasColumnName("organization_id");
        entity.Property(e => e.PhotoFileId).HasColumnName("photo_file_id");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.UserId).HasColumnName("user_id");

        entity.HasOne(d => d.Address).WithMany(p => p.FarmerProfiles)
            .HasForeignKey(d => d.AddressId)
            .HasConstraintName("farmer_profiles_address_id_fkey");

        entity.HasOne(d => d.Organization).WithMany(p => p.FarmerProfiles)
            .HasForeignKey(d => d.OrganizationId)
            .HasConstraintName("farmer_profiles_organization_id_fkey");

        entity.HasOne(d => d.PhotoFile).WithMany(p => p.FarmerProfiles)
            .HasForeignKey(d => d.PhotoFileId)
            .HasConstraintName("farmer_profiles_photo_file_id_fkey");

        entity.HasOne(d => d.User).WithOne(p => p.FarmerProfile)
            .HasForeignKey<FarmerProfile>(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("farmer_profiles_user_id_fkey");
    }
}
