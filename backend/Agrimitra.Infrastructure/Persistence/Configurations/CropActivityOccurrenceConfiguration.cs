using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class CropActivityOccurrenceConfiguration : IEntityTypeConfiguration<CropActivityOccurrence>
{
    public void Configure(EntityTypeBuilder<CropActivityOccurrence> entity)
    {
        entity.HasKey(e => e.Id).HasName("crop_activity_occurrences_pkey");

        entity.ToTable("crop_activity_occurrences");

        entity.HasIndex(e => new { e.ActivityId, e.SeqNo, e.Revision }, "crop_activity_occurrences_activity_id_seq_no_revision_key").IsUnique();

        entity.HasIndex(e => new { e.ActivityId, e.ScheduledStartAt }, "ix_occurrences_activity");

        entity.HasIndex(e => e.RescheduledToId, "ix_occurrences_rescheduled_to");

        entity.HasIndex(e => e.ScheduledStartAt, "ix_occurrences_start").HasFilter("(status = ANY (ARRAY['scheduled'::text, 'upcoming'::text, 'in_progress'::text]))");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ActivityId).HasColumnName("activity_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.OverrideInstructions).HasColumnName("override_instructions");
        entity.Property(e => e.RescheduledToId).HasColumnName("rescheduled_to_id");
        entity.Property(e => e.Revision).HasColumnName("revision");
        entity.Property(e => e.ScheduledEndAt).HasColumnName("scheduled_end_at");
        entity.Property(e => e.ScheduledStartAt).HasColumnName("scheduled_start_at");
        entity.Property(e => e.SeqNo).HasColumnName("seq_no");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'scheduled'::text")
            .HasColumnName("status");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Activity).WithMany(p => p.CropActivityOccurrences)
            .HasForeignKey(d => d.ActivityId)
            .HasConstraintName("crop_activity_occurrences_activity_id_fkey");

        entity.HasOne(d => d.RescheduledTo).WithMany(p => p.InverseRescheduledTo)
            .HasForeignKey(d => d.RescheduledToId)
            .HasConstraintName("crop_activity_occurrences_rescheduled_to_id_fkey");
    }
}
