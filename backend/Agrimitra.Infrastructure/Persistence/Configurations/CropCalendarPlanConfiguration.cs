using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class CropCalendarPlanConfiguration : IEntityTypeConfiguration<CropCalendarPlan>
{
    public void Configure(EntityTypeBuilder<CropCalendarPlan> entity)
    {
        entity.HasKey(e => e.Id).HasName("crop_calendar_plans_pkey");

        entity.ToTable("crop_calendar_plans");

        entity.HasIndex(e => e.CarePlanId, "ix_calendar_plans_care_plan");

        entity.HasIndex(e => e.CropCycleId, "ix_calendar_plans_cycle");

        entity.HasIndex(e => new { e.FarmerProfileId, e.Status }, "ix_calendar_plans_farmer").HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.LandId, "ix_calendar_plans_land");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CarePlanId).HasColumnName("care_plan_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropCycleId).HasColumnName("crop_cycle_id");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.EndDate).HasColumnName("end_date");
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.LandId).HasColumnName("land_id");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.Source).HasColumnName("source");
        entity.Property(e => e.StartDate).HasColumnName("start_date");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'active'::text")
            .HasColumnName("status");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.CarePlan).WithMany(p => p.CropCalendarPlans)
            .HasForeignKey(d => d.CarePlanId)
            .HasConstraintName("crop_calendar_plans_care_plan_id_fkey");

        entity.HasOne(d => d.CropCycle).WithMany(p => p.CropCalendarPlans)
            .HasForeignKey(d => d.CropCycleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("crop_calendar_plans_crop_cycle_id_fkey");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.CropCalendarPlans)
            .HasForeignKey(d => d.FarmerProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("crop_calendar_plans_farmer_profile_id_fkey");

        entity.HasOne(d => d.Land).WithMany(p => p.CropCalendarPlans)
            .HasForeignKey(d => d.LandId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("crop_calendar_plans_land_id_fkey");
    }
}
