using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ConsultantAvailabilityConfiguration : IEntityTypeConfiguration<ConsultantAvailability>
{
    public void Configure(EntityTypeBuilder<ConsultantAvailability> entity)
    {
        entity.HasKey(e => e.Id).HasName("consultant_availability_pkey");

        entity.ToTable("consultant_availability");

        entity.HasIndex(e => new { e.ConsultantId, e.DayOfWeek }, "ix_consultant_availability_consultant");

        entity.HasIndex(e => new { e.ConsultantId, e.SpecificDate }, "ix_consultant_availability_date").HasFilter("(specific_date IS NOT NULL)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ConsultantId).HasColumnName("consultant_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DayOfWeek).HasColumnName("day_of_week");
        entity.Property(e => e.EndTime).HasColumnName("end_time");
        entity.Property(e => e.IsUnavailable).HasColumnName("is_unavailable");
        entity.Property(e => e.SpecificDate).HasColumnName("specific_date");
        entity.Property(e => e.StartTime).HasColumnName("start_time");
        entity.Property(e => e.TimeZoneId)
            .HasDefaultValueSql("'Asia/Kolkata'::text")
            .HasColumnName("time_zone_id");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.ValidFrom).HasColumnName("valid_from");
        entity.Property(e => e.ValidUntil).HasColumnName("valid_until");

        entity.HasOne(d => d.Consultant).WithMany(p => p.ConsultantAvailabilities)
            .HasForeignKey(d => d.ConsultantId)
            .HasConstraintName("consultant_availability_consultant_id_fkey");
    }
}
