using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class CropActivityHistoryConfiguration : IEntityTypeConfiguration<CropActivityHistory>
{
    public void Configure(EntityTypeBuilder<CropActivityHistory> entity)
    {
        entity.HasKey(e => e.Id).HasName("crop_activity_history_pkey");

        entity.ToTable("crop_activity_history");

        entity.HasIndex(e => new { e.ActivityId, e.CreatedAt }, "ix_activity_history_activity").IsDescending(false, true);

        entity.HasIndex(e => new { e.ActorUserId, e.CreatedAt }, "ix_activity_history_actor").IsDescending(false, true);

        entity.HasIndex(e => e.OccurrenceId, "ix_activity_history_occurrence");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ActivityId).HasColumnName("activity_id");
        entity.Property(e => e.ActorRole).HasColumnName("actor_role");
        entity.Property(e => e.ActorUserId).HasColumnName("actor_user_id");
        entity.Property(e => e.AfterValue)
            .HasColumnType("jsonb")
            .HasColumnName("after_value");
        entity.Property(e => e.BeforeValue)
            .HasColumnType("jsonb")
            .HasColumnName("before_value");
        entity.Property(e => e.ChangeType).HasColumnName("change_type");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.OccurrenceId).HasColumnName("occurrence_id");
        entity.Property(e => e.Reason).HasColumnName("reason");
        entity.Property(e => e.Via)
            .HasDefaultValueSql("'direct'::text")
            .HasColumnName("via");

        entity.HasOne(d => d.Activity).WithMany(p => p.CropActivityHistories)
            .HasForeignKey(d => d.ActivityId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("crop_activity_history_activity_id_fkey");

        entity.HasOne(d => d.ActorUser).WithMany(p => p.CropActivityHistories)
            .HasForeignKey(d => d.ActorUserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("crop_activity_history_actor_user_id_fkey");

        entity.HasOne(d => d.Occurrence).WithMany(p => p.CropActivityHistories)
            .HasForeignKey(d => d.OccurrenceId)
            .HasConstraintName("crop_activity_history_occurrence_id_fkey");
    }
}
