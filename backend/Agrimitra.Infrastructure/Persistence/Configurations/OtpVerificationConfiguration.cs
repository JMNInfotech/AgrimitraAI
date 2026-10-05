using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class OtpVerificationConfiguration : IEntityTypeConfiguration<OtpVerification>
{
    public void Configure(EntityTypeBuilder<OtpVerification> entity)
    {
        entity.HasKey(e => e.Id).HasName("otp_verifications_pkey");

        entity.ToTable("otp_verifications");

        entity.HasIndex(e => new { e.Target, e.Purpose, e.CreatedAt }, "ix_otp_target_purpose").IsDescending(false, false, true);

        entity.HasIndex(e => e.UserId, "ix_otp_user").HasFilter("(user_id IS NOT NULL)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Attempts).HasColumnName("attempts");
        entity.Property(e => e.Channel).HasColumnName("channel");
        entity.Property(e => e.CodeHash).HasColumnName("code_hash");
        entity.Property(e => e.ConsumedAt).HasColumnName("consumed_at");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
        entity.Property(e => e.IpAddress).HasColumnName("ip_address");
        entity.Property(e => e.MaxAttempts)
            .HasDefaultValue(5)
            .HasColumnName("max_attempts");
        entity.Property(e => e.Purpose).HasColumnName("purpose");
        entity.Property(e => e.Target).HasColumnName("target");
        entity.Property(e => e.UserId).HasColumnName("user_id");
        entity.Property(e => e.VerifiedAt).HasColumnName("verified_at");

        entity.HasOne(d => d.User).WithMany(p => p.OtpVerifications)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("otp_verifications_user_id_fkey");
    }
}
