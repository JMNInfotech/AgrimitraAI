using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class CropCycleStageHistoryConfiguration : IEntityTypeConfiguration<CropCycleStageHistory>
{
    public void Configure(EntityTypeBuilder<CropCycleStageHistory> entity)
    {
        entity.HasKey(e => e.Id).HasName("crop_cycle_stage_history_pkey");

        entity.ToTable("crop_cycle_stage_history");

        entity.HasIndex(e => e.ChangedBy, "ix_crop_cycle_stage_history_changed_by_4ab250");

        entity.HasIndex(e => new { e.CropCycleId, e.StartedAt }, "ix_stage_history_cycle").IsDescending(false, true);

        entity.HasIndex(e => e.StageId, "ix_stage_history_stage");

        entity.HasIndex(e => e.CropCycleId, "ux_stage_history_open")
            .IsUnique()
            .HasFilter("(ended_at IS NULL)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ChangedBy).HasColumnName("changed_by");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CropCycleId).HasColumnName("crop_cycle_id");
        entity.Property(e => e.EndedAt).HasColumnName("ended_at");
        entity.Property(e => e.Notes).HasColumnName("notes");
        entity.Property(e => e.Source)
            .HasDefaultValueSql("'farmer'::text")
            .HasColumnName("source");
        entity.Property(e => e.StageId).HasColumnName("stage_id");
        entity.Property(e => e.StartedAt).HasColumnName("started_at");

        entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.CropCycleStageHistories)
            .HasForeignKey(d => d.ChangedBy)
            .HasConstraintName("crop_cycle_stage_history_changed_by_fkey");

        entity.HasOne(d => d.CropCycle).WithOne(p => p.CropCycleStageHistory)
            .HasForeignKey<CropCycleStageHistory>(d => d.CropCycleId)
            .HasConstraintName("crop_cycle_stage_history_crop_cycle_id_fkey");

        entity.HasOne(d => d.Stage).WithMany(p => p.CropCycleStageHistories)
            .HasForeignKey(d => d.StageId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("crop_cycle_stage_history_stage_id_fkey");
    }
}
