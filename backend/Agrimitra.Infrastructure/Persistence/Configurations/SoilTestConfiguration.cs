using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class SoilTestConfiguration : IEntityTypeConfiguration<SoilTest>
{
    public void Configure(EntityTypeBuilder<SoilTest> entity)
    {
        entity.HasKey(e => e.Id).HasName("soil_tests_pkey");

        entity.ToTable("soil_tests");

        entity.HasIndex(e => e.LabBookingId, "ix_soil_tests_booking");

        entity.HasIndex(e => e.SoilSampleId, "ix_soil_tests_sample");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.LabBookingId).HasColumnName("lab_booking_id");
        entity.Property(e => e.SoilSampleId).HasColumnName("soil_sample_id");
        entity.Property(e => e.Source).HasColumnName("source");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'pending'::text")
            .HasColumnName("status");
        entity.Property(e => e.TestedOn).HasColumnName("tested_on");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.LabBooking).WithMany(p => p.SoilTests)
            .HasForeignKey(d => d.LabBookingId)
            .HasConstraintName("fk_soil_tests_booking");

        entity.HasOne(d => d.SoilSample).WithMany(p => p.SoilTests)
            .HasForeignKey(d => d.SoilSampleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("soil_tests_soil_sample_id_fkey");

        entity.HasMany(d => d.SoilParameters).WithMany(p => p.SoilTests)
            .UsingEntity<Dictionary<string, object>>(
                "SoilTestParameter",
                r => r.HasOne<SoilParameter>().WithMany()
                    .HasForeignKey("SoilParameterId")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("soil_test_parameters_soil_parameter_id_fkey"),
                l => l.HasOne<SoilTest>().WithMany()
                    .HasForeignKey("SoilTestId")
                    .HasConstraintName("soil_test_parameters_soil_test_id_fkey"),
                j =>
                {
                    j.HasKey("SoilTestId", "SoilParameterId").HasName("soil_test_parameters_pkey");
                    j.ToTable("soil_test_parameters");
                    j.HasIndex(new[] { "SoilParameterId" }, "ix_soil_test_parameters_param");
                    j.IndexerProperty<Guid>("SoilTestId").HasColumnName("soil_test_id");
                    j.IndexerProperty<Guid>("SoilParameterId").HasColumnName("soil_parameter_id");
                });
    }
}
