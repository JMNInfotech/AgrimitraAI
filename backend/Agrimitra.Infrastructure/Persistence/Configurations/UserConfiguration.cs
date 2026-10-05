using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.HasKey(e => e.Id).HasName("users_pkey");

        entity.ToTable("users");

        entity.HasIndex(e => e.OrganizationId, "ix_users_organization").HasFilter("(organization_id IS NOT NULL)");

        entity.HasIndex(e => e.PreferredLanguage, "ix_users_preferred_language_8b3990");

        entity.HasIndex(e => e.Status, "ix_users_status").HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.Email, "ux_users_email")
            .IsUnique()
            .HasFilter("((email IS NOT NULL) AND (NOT is_deleted))");

        entity.HasIndex(e => e.MobileNumber, "ux_users_mobile")
            .IsUnique()
            .HasFilter("((mobile_number IS NOT NULL) AND (NOT is_deleted))");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.Email)
            .HasColumnType("citext")
            .HasColumnName("email");
        entity.Property(e => e.EmailVerifiedAt).HasColumnName("email_verified_at");
        entity.Property(e => e.FailedLoginCount).HasColumnName("failed_login_count");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.LastLoginAt).HasColumnName("last_login_at");
        entity.Property(e => e.LockoutEndAt).HasColumnName("lockout_end_at");
        entity.Property(e => e.MfaEnabled).HasColumnName("mfa_enabled");
        entity.Property(e => e.MfaSecretEncrypted).HasColumnName("mfa_secret_encrypted");
        entity.Property(e => e.MobileNumber).HasColumnName("mobile_number");
        entity.Property(e => e.MobileVerifiedAt).HasColumnName("mobile_verified_at");
        entity.Property(e => e.OrganizationId).HasColumnName("organization_id");
        entity.Property(e => e.PasswordAlgorithm).HasColumnName("password_algorithm");
        entity.Property(e => e.PasswordChangedAt).HasColumnName("password_changed_at");
        entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
        entity.Property(e => e.PreferredLanguage)
            .HasDefaultValueSql("'en'::text")
            .HasColumnName("preferred_language");
        entity.Property(e => e.SecurityStamp)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("security_stamp");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'pending_verification'::text")
            .HasColumnName("status");
        entity.Property(e => e.TimeZoneId)
            .HasDefaultValueSql("'Asia/Kolkata'::text")
            .HasColumnName("time_zone_id");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Organization).WithMany(p => p.Users)
            .HasForeignKey(d => d.OrganizationId)
            .HasConstraintName("users_organization_id_fkey");

        entity.HasOne(d => d.PreferredLanguageNavigation).WithMany(p => p.Users)
            .HasForeignKey(d => d.PreferredLanguage)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("users_preferred_language_fkey");
    }
}
