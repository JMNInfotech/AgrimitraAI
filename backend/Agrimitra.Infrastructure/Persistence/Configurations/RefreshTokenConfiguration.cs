using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> entity)
    {
        entity.HasKey(e => e.Id).HasName("refresh_tokens_pkey");

        entity.ToTable("refresh_tokens");

        entity.HasIndex(e => e.FamilyId, "ix_refresh_tokens_family");

        entity.HasIndex(e => e.ReplacedById, "ix_refresh_tokens_replaced_by_id_bf9435");

        entity.HasIndex(e => e.SessionId, "ix_refresh_tokens_session");

        entity.HasIndex(e => e.UserId, "ix_refresh_tokens_user");

        entity.HasIndex(e => e.TokenHash, "refresh_tokens_token_hash_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedByIp).HasColumnName("created_by_ip");
        entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
        entity.Property(e => e.FamilyId).HasColumnName("family_id");
        entity.Property(e => e.ReplacedById).HasColumnName("replaced_by_id");
        entity.Property(e => e.RevokedAt).HasColumnName("revoked_at");
        entity.Property(e => e.SessionId).HasColumnName("session_id");
        entity.Property(e => e.TokenHash).HasColumnName("token_hash");
        entity.Property(e => e.UsedAt).HasColumnName("used_at");
        entity.Property(e => e.UserId).HasColumnName("user_id");

        entity.HasOne(d => d.ReplacedBy).WithMany(p => p.InverseReplacedBy)
            .HasForeignKey(d => d.ReplacedById)
            .HasConstraintName("refresh_tokens_replaced_by_id_fkey");

        entity.HasOne(d => d.Session).WithMany(p => p.RefreshTokens)
            .HasForeignKey(d => d.SessionId)
            .HasConstraintName("refresh_tokens_session_id_fkey");

        entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("refresh_tokens_user_id_fkey");
    }
}
