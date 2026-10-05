using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class SoilMeasurementConfiguration : IEntityTypeConfiguration<SoilMeasurement>
{
    public void Configure(EntityTypeBuilder<SoilMeasurement> entity)
    {
        entity.HasKey(e => e.Id).HasName("soil_measurements_pkey");

        entity.ToTable("soil_measurements");

        entity.HasIndex(e => e.SoilParameterId, "ix_soil_measurements_param");

        entity.HasIndex(e => e.SoilReportId, "ix_soil_measurements_report");

        entity.HasIndex(e => new { e.SoilTestId, e.SoilParameterId }, "soil_measurements_soil_test_id_soil_parameter_id_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.IsExtracted).HasColumnName("is_extracted");
        entity.Property(e => e.Rating).HasColumnName("rating");
        entity.Property(e => e.SoilParameterId).HasColumnName("soil_parameter_id");
        entity.Property(e => e.SoilReportId).HasColumnName("soil_report_id");
        entity.Property(e => e.SoilTestId).HasColumnName("soil_test_id");
        entity.Property(e => e.Unit).HasColumnName("unit");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.Value)
            .HasPrecision(14, 4)
            .HasColumnName("value");

        entity.HasOne(d => d.SoilParameter).WithMany(p => p.SoilMeasurements)
            .HasForeignKey(d => d.SoilParameterId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("soil_measurements_soil_parameter_id_fkey");

        entity.HasOne(d => d.SoilReport).WithMany(p => p.SoilMeasurements)
            .HasForeignKey(d => d.SoilReportId)
            .HasConstraintName("soil_measurements_soil_report_id_fkey");

        entity.HasOne(d => d.SoilTest).WithMany(p => p.SoilMeasurements)
            .HasForeignKey(d => d.SoilTestId)
            .HasConstraintName("soil_measurements_soil_test_id_fkey");
    }
}
