using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class NotificationDeliveryLogConfiguration : IEntityTypeConfiguration<NotificationDeliveryLog>
{
    public void Configure(EntityTypeBuilder<NotificationDeliveryLog> entity)
    {
        entity.HasKey(e => e.Id).HasName("notification_delivery_logs_pkey");

        entity.ToTable("notification_delivery_logs");

        entity.HasIndex(e => e.CreatedAt, "ix_delivery_logs_failed").HasFilter("(status = 'failed'::text)");

        entity.HasIndex(e => e.NotificationId, "ix_delivery_logs_notification");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Attempt)
            .HasDefaultValue(1)
            .HasColumnName("attempt");
        entity.Property(e => e.Channel).HasColumnName("channel");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.ErrorCode).HasColumnName("error_code");
        entity.Property(e => e.ErrorMessage).HasColumnName("error_message");
        entity.Property(e => e.NotificationId).HasColumnName("notification_id");
        entity.Property(e => e.Provider).HasColumnName("provider");
        entity.Property(e => e.ProviderMessageId).HasColumnName("provider_message_id");
        entity.Property(e => e.SentAt).HasColumnName("sent_at");
        entity.Property(e => e.Status).HasColumnName("status");

        entity.HasOne(d => d.Notification).WithMany(p => p.NotificationDeliveryLogs)
            .HasForeignKey(d => d.NotificationId)
            .HasConstraintName("notification_delivery_logs_notification_id_fkey");
    }
}
