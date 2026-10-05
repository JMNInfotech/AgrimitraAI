using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> entity)
    {
        entity.HasKey(e => e.Id).HasName("user_sessions_pkey");

        entity.ToTable("user_sessions");

        entity.HasIndex(e => e.ActiveRoleId, "ix_user_sessions_active_role_id_2afa35");

        entity.HasIndex(e => e.DeviceId, "ix_user_sessions_device_id_d812cf");

        entity.HasIndex(e => new { e.UserId, e.ExpiresAt }, "ix_user_sessions_user_active").HasFilter("(revoked_at IS NULL)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ActiveRoleId).HasColumnName("active_role_id");
        entity.Property(e => e.DeviceId).HasColumnName("device_id");
        entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
        entity.Property(e => e.IpAddress).HasColumnName("ip_address");
        entity.Property(e => e.LastSeenAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("last_seen_at");
        entity.Property(e => e.RevokedAt).HasColumnName("revoked_at");
        entity.Property(e => e.RevokedReason).HasColumnName("revoked_reason");
        entity.Property(e => e.StartedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("started_at");
        entity.Property(e => e.UserAgent).HasColumnName("user_agent");
        entity.Property(e => e.UserId).HasColumnName("user_id");

        entity.HasOne(d => d.ActiveRole).WithMany(p => p.UserSessions)
            .HasForeignKey(d => d.ActiveRoleId)
            .HasConstraintName("user_sessions_active_role_id_fkey");

        entity.HasOne(d => d.Device).WithMany(p => p.UserSessions)
            .HasForeignKey(d => d.DeviceId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("user_sessions_device_id_fkey");

        entity.HasOne(d => d.User).WithMany(p => p.UserSessions)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("user_sessions_user_id_fkey");
    }
}
