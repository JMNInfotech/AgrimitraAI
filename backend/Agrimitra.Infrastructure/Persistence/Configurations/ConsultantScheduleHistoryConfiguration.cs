using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ConsultantScheduleHistoryConfiguration : IEntityTypeConfiguration<ConsultantScheduleHistory>
{
    public void Configure(EntityTypeBuilder<ConsultantScheduleHistory> entity)
    {
        entity.HasKey(e => e.Id).HasName("consultant_schedule_history_pkey");

        entity.ToTable("consultant_schedule_history");

        entity.HasIndex(e => new { e.ScheduleActivityId, e.CreatedAt }, "ix_schedule_history_activity").IsDescending(false, true);

        entity.HasIndex(e => e.ActorUserId, "ix_schedule_history_actor");

        entity.HasIndex(e => new { e.CarePlanId, e.CreatedAt }, "ix_schedule_history_plan").IsDescending(false, true);

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ActorRole).HasColumnName("actor_role");
        entity.Property(e => e.ActorUserId).HasColumnName("actor_user_id");
        entity.Property(e => e.CarePlanId).HasColumnName("care_plan_id");
        entity.Property(e => e.ChangeType).HasColumnName("change_type");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.NewValue)
            .HasColumnType("jsonb")
            .HasColumnName("new_value");
        entity.Property(e => e.OldValue)
            .HasColumnType("jsonb")
            .HasColumnName("old_value");
        entity.Property(e => e.PlanVersion).HasColumnName("plan_version");
        entity.Property(e => e.Reason).HasColumnName("reason");
        entity.Property(e => e.ScheduleActivityId).HasColumnName("schedule_activity_id");

        entity.HasOne(d => d.ActorUser).WithMany(p => p.ConsultantScheduleHistories)
            .HasForeignKey(d => d.ActorUserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("consultant_schedule_history_actor_user_id_fkey");

        entity.HasOne(d => d.CarePlan).WithMany(p => p.ConsultantScheduleHistories)
            .HasForeignKey(d => d.CarePlanId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("consultant_schedule_history_care_plan_id_fkey");

        entity.HasOne(d => d.ScheduleActivity).WithMany(p => p.ConsultantScheduleHistories)
            .HasForeignKey(d => d.ScheduleActivityId)
            .HasConstraintName("consultant_schedule_history_schedule_activity_id_fkey");
    }
}
