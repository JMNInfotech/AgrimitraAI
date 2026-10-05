using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class NotificationPreferenceConfiguration : IEntityTypeConfiguration<NotificationPreference>
{
    public void Configure(EntityTypeBuilder<NotificationPreference> entity)
    {
        entity.HasKey(e => e.Id).HasName("notification_preferences_pkey");

        entity.ToTable("notification_preferences");

        entity.HasIndex(e => new { e.UserId, e.EventType, e.Channel }, "notification_preferences_user_id_event_type_channel_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Channel).HasColumnName("channel");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.EventType).HasColumnName("event_type");
        entity.Property(e => e.IsEnabled)
            .HasDefaultValue(true)
            .HasColumnName("is_enabled");
        entity.Property(e => e.QuietHoursEnd).HasColumnName("quiet_hours_end");
        entity.Property(e => e.QuietHoursStart).HasColumnName("quiet_hours_start");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.UserId).HasColumnName("user_id");

        entity.HasOne(d => d.User).WithMany(p => p.NotificationPreferences)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("notification_preferences_user_id_fkey");
    }
}
