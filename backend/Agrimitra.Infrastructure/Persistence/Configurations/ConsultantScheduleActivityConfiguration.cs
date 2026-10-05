using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ConsultantScheduleActivityConfiguration : IEntityTypeConfiguration<ConsultantScheduleActivity>
{
    public void Configure(EntityTypeBuilder<ConsultantScheduleActivity> entity)
    {
        entity.HasKey(e => e.Id).HasName("consultant_schedule_activities_pkey");

        entity.ToTable("consultant_schedule_activities");

        entity.HasIndex(e => e.PrescriptionItemId, "ix_consultant_schedule_activities_prescription_item_id_0e8efd");

        entity.HasIndex(e => e.CalendarActivityId, "ix_schedule_activities_calendar");

        entity.HasIndex(e => new { e.CarePlanId, e.StartDate }, "ix_schedule_activities_plan").HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => new { e.CalendarActivityId, e.PlanVersion }, "ux_schedule_activities_calendar")
            .IsUnique()
            .HasFilter("(calendar_activity_id IS NOT NULL)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ActivityType).HasColumnName("activity_type");
        entity.Property(e => e.CalendarActivityId).HasColumnName("calendar_activity_id");
        entity.Property(e => e.CarePlanId).HasColumnName("care_plan_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.EndTime).HasColumnName("end_time");
        entity.Property(e => e.Instructions).HasColumnName("instructions");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.IsMandatory).HasColumnName("is_mandatory");
        entity.Property(e => e.PlanVersion).HasColumnName("plan_version");
        entity.Property(e => e.PrescriptionItemId).HasColumnName("prescription_item_id");
        entity.Property(e => e.Priority)
            .HasDefaultValueSql("'normal'::text")
            .HasColumnName("priority");
        entity.Property(e => e.RecurrenceCount).HasColumnName("recurrence_count");
        entity.Property(e => e.RecurrenceFrequency).HasColumnName("recurrence_frequency");
        entity.Property(e => e.RecurrenceInterval).HasColumnName("recurrence_interval");
        entity.Property(e => e.RecurrenceUntil).HasColumnName("recurrence_until");
        entity.Property(e => e.RecurrenceWeekdays).HasColumnName("recurrence_weekdays");
        entity.Property(e => e.ReminderOffsetsMinutes)
            .HasDefaultValueSql("'{1440,60}'::integer[]")
            .HasColumnName("reminder_offsets_minutes");
        entity.Property(e => e.StartDate).HasColumnName("start_date");
        entity.Property(e => e.StartTime).HasColumnName("start_time");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'draft'::text")
            .HasColumnName("status");
        entity.Property(e => e.TimeZoneId)
            .HasDefaultValueSql("'Asia/Kolkata'::text")
            .HasColumnName("time_zone_id");
        entity.Property(e => e.Title).HasColumnName("title");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.CalendarActivity).WithMany(p => p.ConsultantScheduleActivities)
            .HasForeignKey(d => d.CalendarActivityId)
            .HasConstraintName("fk_schedule_activities_calendar");

        entity.HasOne(d => d.CarePlan).WithMany(p => p.ConsultantScheduleActivities)
            .HasForeignKey(d => d.CarePlanId)
            .HasConstraintName("consultant_schedule_activities_care_plan_id_fkey");

        entity.HasOne(d => d.PrescriptionItem).WithMany(p => p.ConsultantScheduleActivities)
            .HasForeignKey(d => d.PrescriptionItemId)
            .HasConstraintName("consultant_schedule_activities_prescription_item_id_fkey");
    }
}
