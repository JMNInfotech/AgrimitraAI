using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> entity)
    {
        entity.HasKey(e => e.Id).HasName("notifications_pkey");

        entity.ToTable("notifications");

        entity.HasIndex(e => new { e.RelatedEntityType, e.RelatedEntityId }, "ix_notifications_entity");

        entity.HasIndex(e => e.LanguageCode, "ix_notifications_language_code_a4d41d");

        entity.HasIndex(e => new { e.UserId, e.CreatedAt }, "ix_notifications_user_time").IsDescending(false, true);

        entity.HasIndex(e => new { e.UserId, e.CreatedAt }, "ix_notifications_user_unread")
            .IsDescending(false, true)
            .HasFilter("(read_at IS NULL)");

        entity.HasIndex(e => new { e.UserId, e.DedupeKey }, "ux_notifications_dedupe")
            .IsUnique()
            .HasFilter("(dedupe_key IS NOT NULL)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Body).HasColumnName("body");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.Data)
            .HasDefaultValueSql("'{}'::jsonb")
            .HasColumnType("jsonb")
            .HasColumnName("data");
        entity.Property(e => e.DedupeKey).HasColumnName("dedupe_key");
        entity.Property(e => e.EventType).HasColumnName("event_type");
        entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
        entity.Property(e => e.LanguageCode).HasColumnName("language_code");
        entity.Property(e => e.Priority)
            .HasDefaultValueSql("'normal'::text")
            .HasColumnName("priority");
        entity.Property(e => e.ReadAt).HasColumnName("read_at");
        entity.Property(e => e.RelatedEntityId).HasColumnName("related_entity_id");
        entity.Property(e => e.RelatedEntityType).HasColumnName("related_entity_type");
        entity.Property(e => e.TemplateCode).HasColumnName("template_code");
        entity.Property(e => e.Title).HasColumnName("title");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.UserId).HasColumnName("user_id");

        entity.HasOne(d => d.LanguageCodeNavigation).WithMany(p => p.Notifications)
            .HasForeignKey(d => d.LanguageCode)
            .HasConstraintName("notifications_language_code_fkey");

        entity.HasOne(d => d.User).WithMany(p => p.Notifications)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("notifications_user_id_fkey");
    }
}
