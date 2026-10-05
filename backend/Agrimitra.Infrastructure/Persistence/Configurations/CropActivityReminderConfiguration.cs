using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class CropActivityReminderConfiguration : IEntityTypeConfiguration<CropActivityReminder>
{
    public void Configure(EntityTypeBuilder<CropActivityReminder> entity)
    {
        entity.HasKey(e => e.Id).HasName("crop_activity_reminders_pkey");

        entity.ToTable("crop_activity_reminders");

        entity.HasIndex(e => new { e.OccurrenceId, e.OffsetMinutes }, "crop_activity_reminders_occurrence_id_offset_minutes_key").IsUnique();

        entity.HasIndex(e => e.FireAt, "ix_reminders_due").HasFilter("(status = ANY (ARRAY['pending'::text, 'processing'::text]))");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Attempts).HasColumnName("attempts");
        entity.Property(e => e.Channels)
            .HasDefaultValueSql("'{in_app,push}'::text[]")
            .HasColumnName("channels");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.FireAt).HasColumnName("fire_at");
        entity.Property(e => e.LastError).HasColumnName("last_error");
        entity.Property(e => e.LockedUntil).HasColumnName("locked_until");
        entity.Property(e => e.OccurrenceId).HasColumnName("occurrence_id");
        entity.Property(e => e.OffsetMinutes).HasColumnName("offset_minutes");
        entity.Property(e => e.SentAt).HasColumnName("sent_at");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'pending'::text")
            .HasColumnName("status");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Occurrence).WithMany(p => p.CropActivityReminders)
            .HasForeignKey(d => d.OccurrenceId)
            .HasConstraintName("crop_activity_reminders_occurrence_id_fkey");
    }
}
