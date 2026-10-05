using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class BrandProfileConfiguration : IEntityTypeConfiguration<BrandProfile>
{
    public void Configure(EntityTypeBuilder<BrandProfile> entity)
    {
        entity.HasKey(e => e.Id).HasName("brand_profiles_pkey");

        entity.ToTable("brand_profiles");

        entity.HasIndex(e => e.LogoFileId, "ix_brand_profiles_logo_file_id_bed351");

        entity.HasIndex(e => e.UserId, "ix_brand_profiles_user");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.BrandName).HasColumnName("brand_name");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.LegalName).HasColumnName("legal_name");
        entity.Property(e => e.LogoFileId).HasColumnName("logo_file_id");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.UserId).HasColumnName("user_id");
        entity.Property(e => e.VerificationStatus)
            .HasDefaultValueSql("'unverified'::text")
            .HasColumnName("verification_status");
        entity.Property(e => e.Website).HasColumnName("website");

        entity.HasOne(d => d.LogoFile).WithMany(p => p.BrandProfiles)
            .HasForeignKey(d => d.LogoFileId)
            .HasConstraintName("brand_profiles_logo_file_id_fkey");

        entity.HasOne(d => d.User).WithMany(p => p.BrandProfiles)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("brand_profiles_user_id_fkey");
    }
}
