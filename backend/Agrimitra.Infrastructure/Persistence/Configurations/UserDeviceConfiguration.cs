using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class UserDeviceConfiguration : IEntityTypeConfiguration<UserDevice>
{
    public void Configure(EntityTypeBuilder<UserDevice> entity)
    {
        entity.HasKey(e => e.Id).HasName("user_devices_pkey");

        entity.ToTable("user_devices");

        entity.HasIndex(e => e.PushToken, "ix_user_devices_push").HasFilter("(push_token IS NOT NULL)");

        entity.HasIndex(e => new { e.UserId, e.DeviceIdentifier }, "user_devices_user_id_device_identifier_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AppVersion).HasColumnName("app_version");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeviceIdentifier).HasColumnName("device_identifier");
        entity.Property(e => e.DeviceName).HasColumnName("device_name");
        entity.Property(e => e.IsTrusted).HasColumnName("is_trusted");
        entity.Property(e => e.LastSeenAt).HasColumnName("last_seen_at");
        entity.Property(e => e.Platform).HasColumnName("platform");
        entity.Property(e => e.PushToken).HasColumnName("push_token");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.UserId).HasColumnName("user_id");

        entity.HasOne(d => d.User).WithMany(p => p.UserDevices)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("user_devices_user_id_fkey");
    }
}
