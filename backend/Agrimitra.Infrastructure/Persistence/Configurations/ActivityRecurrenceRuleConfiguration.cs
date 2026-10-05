using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ActivityRecurrenceRuleConfiguration : IEntityTypeConfiguration<ActivityRecurrenceRule>
{
    public void Configure(EntityTypeBuilder<ActivityRecurrenceRule> entity)
    {
        entity.HasKey(e => e.Id).HasName("activity_recurrence_rules_pkey");

        entity.ToTable("activity_recurrence_rules");

        entity.HasIndex(e => e.ActivityId, "activity_recurrence_rules_activity_id_key").IsUnique();

        entity.HasIndex(e => e.MaterializedUntil, "ix_recurrence_materialize");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ActivityId).HasColumnName("activity_id");
        entity.Property(e => e.ByWeekday).HasColumnName("by_weekday");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.Frequency).HasColumnName("frequency");
        entity.Property(e => e.IntervalValue)
            .HasDefaultValue(1)
            .HasColumnName("interval_value");
        entity.Property(e => e.MaterializedUntil).HasColumnName("materialized_until");
        entity.Property(e => e.OccurrenceCount).HasColumnName("occurrence_count");
        entity.Property(e => e.UntilDate).HasColumnName("until_date");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Activity).WithOne(p => p.ActivityRecurrenceRule)
            .HasForeignKey<ActivityRecurrenceRule>(d => d.ActivityId)
            .HasConstraintName("activity_recurrence_rules_activity_id_fkey");
    }
}
