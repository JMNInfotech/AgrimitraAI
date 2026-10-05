using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class CropCalendarActivityConfiguration : IEntityTypeConfiguration<CropCalendarActivity>
{
    public void Configure(EntityTypeBuilder<CropCalendarActivity> entity)
    {
        entity.HasKey(e => e.Id).HasName("crop_calendar_activities_pkey");

        entity.ToTable("crop_calendar_activities");

        entity.HasIndex(e => e.AiRecommendationId, "ix_calendar_activities_ai_rec").HasFilter("(ai_recommendation_id IS NOT NULL)");

        entity.HasIndex(e => e.CarePlanId, "ix_calendar_activities_care_plan");

        entity.HasIndex(e => new { e.ConsultantId, e.StartDate }, "ix_calendar_activities_consultant_date").HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.CropId, "ix_calendar_activities_crop");

        entity.HasIndex(e => new { e.CropCycleId, e.StartDate }, "ix_calendar_activities_cycle");

        entity.HasIndex(e => new { e.FarmerProfileId, e.StartDate }, "ix_calendar_activities_farmer_date").HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => new { e.LandId, e.StartDate }, "ix_calendar_activities_land_date").HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.MachineryId, "ix_calendar_activities_machinery").HasFilter("(machinery_id IS NOT NULL)");

        entity.HasIndex(e => e.CalendarPlanId, "ix_calendar_activities_plan");

        entity.HasIndex(e => e.PrescriptionId, "ix_calendar_activities_rx");

        entity.HasIndex(e => new { e.Status, e.StartDate }, "ix_calendar_activities_status").HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.ActivityType, "ix_calendar_activities_type");

        entity.HasIndex(e => e.AssignedByUserId, "ix_crop_calendar_activities_assigned_by_user_id_697bc8");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ActivityType).HasColumnName("activity_type");
        entity.Property(e => e.AiRecommendationId).HasColumnName("ai_recommendation_id");
        entity.Property(e => e.AssignedByUserId).HasColumnName("assigned_by_user_id");
        entity.Property(e => e.CalendarPlanId).HasColumnName("calendar_plan_id");
        entity.Property(e => e.CarePlanId).HasColumnName("care_plan_id");
        entity.Property(e => e.CarePlanVersion).HasColumnName("care_plan_version");
        entity.Property(e => e.CompletionDate).HasColumnName("completion_date");
        entity.Property(e => e.CompletionNotes).HasColumnName("completion_notes");
        entity.Property(e => e.ConsultantId).HasColumnName("consultant_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropCycleId).HasColumnName("crop_cycle_id");
        entity.Property(e => e.CropId).HasColumnName("crop_id");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.EndTime).HasColumnName("end_time");
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.Instructions).HasColumnName("instructions");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.IsLockedByConsultant)
            .HasComputedColumnSql("(origin = 'consultant'::text)", true)
            .HasColumnName("is_locked_by_consultant");
        entity.Property(e => e.IsMandatory).HasColumnName("is_mandatory");
        entity.Property(e => e.LandId).HasColumnName("land_id");
        entity.Property(e => e.MachineryId).HasColumnName("machinery_id");
        entity.Property(e => e.Origin).HasColumnName("origin");
        entity.Property(e => e.PrescriptionId).HasColumnName("prescription_id");
        entity.Property(e => e.Priority)
            .HasDefaultValueSql("'normal'::text")
            .HasColumnName("priority");
        entity.Property(e => e.ReminderOffsetsMinutes)
            .HasDefaultValueSql("'{1440,60}'::integer[]")
            .HasColumnName("reminder_offsets_minutes");
        entity.Property(e => e.StartDate).HasColumnName("start_date");
        entity.Property(e => e.StartTime).HasColumnName("start_time");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'scheduled'::text")
            .HasColumnName("status");
        entity.Property(e => e.TimeZoneId)
            .HasDefaultValueSql("'Asia/Kolkata'::text")
            .HasColumnName("time_zone_id");
        entity.Property(e => e.Title).HasColumnName("title");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.AiRecommendation).WithMany(p => p.CropCalendarActivities)
            .HasForeignKey(d => d.AiRecommendationId)
            .HasConstraintName("fk_calendar_activities_ai_rec");

        entity.HasOne(d => d.AssignedByUser).WithMany(p => p.CropCalendarActivities)
            .HasForeignKey(d => d.AssignedByUserId)
            .HasConstraintName("crop_calendar_activities_assigned_by_user_id_fkey");

        entity.HasOne(d => d.CalendarPlan).WithMany(p => p.CropCalendarActivities)
            .HasForeignKey(d => d.CalendarPlanId)
            .HasConstraintName("crop_calendar_activities_calendar_plan_id_fkey");

        entity.HasOne(d => d.CarePlan).WithMany(p => p.CropCalendarActivities)
            .HasForeignKey(d => d.CarePlanId)
            .HasConstraintName("crop_calendar_activities_care_plan_id_fkey");

        entity.HasOne(d => d.Consultant).WithMany(p => p.CropCalendarActivities)
            .HasForeignKey(d => d.ConsultantId)
            .HasConstraintName("crop_calendar_activities_consultant_id_fkey");

        entity.HasOne(d => d.CropCycle).WithMany(p => p.CropCalendarActivities)
            .HasForeignKey(d => d.CropCycleId)
            .HasConstraintName("crop_calendar_activities_crop_cycle_id_fkey");

        entity.HasOne(d => d.Crop).WithMany(p => p.CropCalendarActivities)
            .HasForeignKey(d => d.CropId)
            .HasConstraintName("crop_calendar_activities_crop_id_fkey");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.CropCalendarActivities)
            .HasForeignKey(d => d.FarmerProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("crop_calendar_activities_farmer_profile_id_fkey");

        entity.HasOne(d => d.Land).WithMany(p => p.CropCalendarActivities)
            .HasForeignKey(d => d.LandId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("crop_calendar_activities_land_id_fkey");

        entity.HasOne(d => d.Machinery).WithMany(p => p.CropCalendarActivities)
            .HasForeignKey(d => d.MachineryId)
            .HasConstraintName("fk_calendar_activities_machinery");

        entity.HasOne(d => d.Prescription).WithMany(p => p.CropCalendarActivities)
            .HasForeignKey(d => d.PrescriptionId)
            .HasConstraintName("crop_calendar_activities_prescription_id_fkey");
    }
}
