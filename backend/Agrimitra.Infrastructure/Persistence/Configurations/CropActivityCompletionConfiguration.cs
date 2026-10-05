using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class CropActivityCompletionConfiguration : IEntityTypeConfiguration<CropActivityCompletion>
{
    public void Configure(EntityTypeBuilder<CropActivityCompletion> entity)
    {
        entity.HasKey(e => e.Id).HasName("crop_activity_completions_pkey");

        entity.ToTable("crop_activity_completions");

        entity.HasIndex(e => e.ClientMutationId, "crop_activity_completions_client_mutation_id_key").IsUnique();

        entity.HasIndex(e => new { e.CompletedBy, e.CompletedAt }, "ix_completions_user").IsDescending(false, true);

        entity.HasIndex(e => e.OccurrenceId, "ux_completions_occurrence")
            .IsUnique()
            .HasFilter("(outcome = 'completed'::text)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ClientMutationId).HasColumnName("client_mutation_id");
        entity.Property(e => e.ClientTimestamp).HasColumnName("client_timestamp");
        entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
        entity.Property(e => e.CompletedBy).HasColumnName("completed_by");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.DeviceId).HasColumnName("device_id");
        entity.Property(e => e.InstructionsSnapshot).HasColumnName("instructions_snapshot");
        entity.Property(e => e.Notes).HasColumnName("notes");
        entity.Property(e => e.OccurrenceId).HasColumnName("occurrence_id");
        entity.Property(e => e.Outcome).HasColumnName("outcome");
        entity.Property(e => e.ScheduledAt).HasColumnName("scheduled_at");
        entity.Property(e => e.Source)
            .HasDefaultValueSql("'online'::text")
            .HasColumnName("source");

        entity.HasOne(d => d.CompletedByNavigation).WithMany(p => p.CropActivityCompletions)
            .HasForeignKey(d => d.CompletedBy)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("crop_activity_completions_completed_by_fkey");

        entity.HasOne(d => d.Occurrence).WithOne(p => p.CropActivityCompletion)
            .HasForeignKey<CropActivityCompletion>(d => d.OccurrenceId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("crop_activity_completions_occurrence_id_fkey");
    }
}
