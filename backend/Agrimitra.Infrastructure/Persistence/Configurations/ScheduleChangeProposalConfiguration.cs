using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ScheduleChangeProposalConfiguration : IEntityTypeConfiguration<ScheduleChangeProposal>
{
    public void Configure(EntityTypeBuilder<ScheduleChangeProposal> entity)
    {
        entity.HasKey(e => e.Id).HasName("schedule_change_proposals_pkey");

        entity.ToTable("schedule_change_proposals");

        entity.HasIndex(e => e.ActivityId, "ix_proposals_activity");

        entity.HasIndex(e => e.ConflictId, "ix_proposals_conflict");

        entity.HasIndex(e => new { e.DeciderUserId, e.CreatedAt }, "ix_proposals_decider_pending").HasFilter("(status = 'pending'::text)");

        entity.HasIndex(e => e.OccurrenceId, "ix_proposals_occurrence");

        entity.HasIndex(e => e.DecidedBy, "ix_schedule_change_proposals_decided_by_40f184");

        entity.HasIndex(e => e.ModelVersionId, "ix_schedule_change_proposals_model_version_id_e158be");

        entity.HasIndex(e => e.RaisedByUserId, "ix_schedule_change_proposals_raised_by_user_id_b01faa");

        entity.HasIndex(e => e.OccurrenceId, "ux_proposals_one_pending")
            .IsUnique()
            .HasFilter("(status = 'pending'::text)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ActivityId).HasColumnName("activity_id");
        entity.Property(e => e.ConflictId).HasColumnName("conflict_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DecidedAt).HasColumnName("decided_at");
        entity.Property(e => e.DecidedBy).HasColumnName("decided_by");
        entity.Property(e => e.DeciderUserId).HasColumnName("decider_user_id");
        entity.Property(e => e.DecisionNote).HasColumnName("decision_note");
        entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
        entity.Property(e => e.ModelVersionId).HasColumnName("model_version_id");
        entity.Property(e => e.OccurrenceId).HasColumnName("occurrence_id");
        entity.Property(e => e.ProposedStartAt).HasColumnName("proposed_start_at");
        entity.Property(e => e.RaisedBy).HasColumnName("raised_by");
        entity.Property(e => e.RaisedByUserId).HasColumnName("raised_by_user_id");
        entity.Property(e => e.Rationale).HasColumnName("rationale");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'pending'::text")
            .HasColumnName("status");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Activity).WithMany(p => p.ScheduleChangeProposals)
            .HasForeignKey(d => d.ActivityId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("schedule_change_proposals_activity_id_fkey");

        entity.HasOne(d => d.Conflict).WithMany(p => p.ScheduleChangeProposals)
            .HasForeignKey(d => d.ConflictId)
            .HasConstraintName("schedule_change_proposals_conflict_id_fkey");

        entity.HasOne(d => d.DecidedByNavigation).WithMany(p => p.ScheduleChangeProposalDecidedByNavigations)
            .HasForeignKey(d => d.DecidedBy)
            .HasConstraintName("schedule_change_proposals_decided_by_fkey");

        entity.HasOne(d => d.DeciderUser).WithMany(p => p.ScheduleChangeProposalDeciderUsers)
            .HasForeignKey(d => d.DeciderUserId)
            .HasConstraintName("schedule_change_proposals_decider_user_id_fkey");

        entity.HasOne(d => d.ModelVersion).WithMany(p => p.ScheduleChangeProposals)
            .HasForeignKey(d => d.ModelVersionId)
            .HasConstraintName("fk_proposals_model_version");

        entity.HasOne(d => d.Occurrence).WithOne(p => p.ScheduleChangeProposal)
            .HasForeignKey<ScheduleChangeProposal>(d => d.OccurrenceId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("schedule_change_proposals_occurrence_id_fkey");

        entity.HasOne(d => d.RaisedByUser).WithMany(p => p.ScheduleChangeProposalRaisedByUsers)
            .HasForeignKey(d => d.RaisedByUserId)
            .HasConstraintName("schedule_change_proposals_raised_by_user_id_fkey");
    }
}
