using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class LabResultValueConfiguration : IEntityTypeConfiguration<LabResultValue>
{
    public void Configure(EntityTypeBuilder<LabResultValue> entity)
    {
        entity.HasKey(e => e.Id).HasName("lab_result_values_pkey");

        entity.ToTable("lab_result_values");

        entity.HasIndex(e => e.SoilParameterId, "ix_lab_result_values_param");

        entity.HasIndex(e => e.LabResultId, "ix_lab_result_values_result");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.Flag).HasColumnName("flag");
        entity.Property(e => e.LabResultId).HasColumnName("lab_result_id");
        entity.Property(e => e.ParameterName).HasColumnName("parameter_name");
        entity.Property(e => e.ReferenceRange).HasColumnName("reference_range");
        entity.Property(e => e.SoilParameterId).HasColumnName("soil_parameter_id");
        entity.Property(e => e.Unit).HasColumnName("unit");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.ValueNumeric)
            .HasPrecision(14, 4)
            .HasColumnName("value_numeric");
        entity.Property(e => e.ValueText).HasColumnName("value_text");

        entity.HasOne(d => d.LabResult).WithMany(p => p.LabResultValues)
            .HasForeignKey(d => d.LabResultId)
            .HasConstraintName("lab_result_values_lab_result_id_fkey");

        entity.HasOne(d => d.SoilParameter).WithMany(p => p.LabResultValues)
            .HasForeignKey(d => d.SoilParameterId)
            .HasConstraintName("lab_result_values_soil_parameter_id_fkey");
    }
}
