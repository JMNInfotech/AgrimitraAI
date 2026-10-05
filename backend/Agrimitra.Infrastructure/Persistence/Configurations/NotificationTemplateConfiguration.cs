using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class NotificationTemplateConfiguration : IEntityTypeConfiguration<NotificationTemplate>
{
    public void Configure(EntityTypeBuilder<NotificationTemplate> entity)
    {
        entity.HasKey(e => e.Id).HasName("notification_templates_pkey");

        entity.ToTable("notification_templates");

        entity.HasIndex(e => e.LanguageCode, "ix_notification_templates_language_code_79aee0");

        entity.HasIndex(e => new { e.Code, e.Channel, e.LanguageCode }, "notification_templates_code_channel_language_code_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Body).HasColumnName("body");
        entity.Property(e => e.Channel).HasColumnName("channel");
        entity.Property(e => e.Code).HasColumnName("code");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasColumnName("is_active");
        entity.Property(e => e.LanguageCode).HasColumnName("language_code");
        entity.Property(e => e.ProviderTemplateId).HasColumnName("provider_template_id");
        entity.Property(e => e.Subject).HasColumnName("subject");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.LanguageCodeNavigation).WithMany(p => p.NotificationTemplates)
            .HasForeignKey(d => d.LanguageCode)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("notification_templates_language_code_fkey");
    }
}
