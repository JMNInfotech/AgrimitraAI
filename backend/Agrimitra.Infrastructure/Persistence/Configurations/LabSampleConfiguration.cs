using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class LabSampleConfiguration : IEntityTypeConfiguration<LabSample>
{
    public void Configure(EntityTypeBuilder<LabSample> entity)
    {
        entity.HasKey(e => e.Id).HasName("lab_samples_pkey");

        entity.ToTable("lab_samples");

        entity.HasIndex(e => e.BookingId, "ix_lab_samples_booking");

        entity.HasIndex(e => e.ReceivedBy, "ix_lab_samples_received_by_b20774");

        entity.HasIndex(e => e.SoilSampleId, "ix_lab_samples_soil_sample");

        entity.HasIndex(e => new { e.LaboratoryId, e.Status }, "ix_lab_samples_status");

        entity.HasIndex(e => new { e.LaboratoryId, e.SampleCode }, "lab_samples_laboratory_id_sample_code_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.BookingId).HasColumnName("booking_id");
        entity.Property(e => e.Condition).HasColumnName("condition");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.LaboratoryId).HasColumnName("laboratory_id");
        entity.Property(e => e.ReceivedAt).HasColumnName("received_at");
        entity.Property(e => e.ReceivedBy).HasColumnName("received_by");
        entity.Property(e => e.SampleCode).HasColumnName("sample_code");
        entity.Property(e => e.SampleType).HasColumnName("sample_type");
        entity.Property(e => e.SoilSampleId).HasColumnName("soil_sample_id");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'expected'::text")
            .HasColumnName("status");
        entity.Property(e => e.StorageLocation).HasColumnName("storage_location");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Booking).WithMany(p => p.LabSamples)
            .HasForeignKey(d => d.BookingId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("lab_samples_booking_id_fkey");

        entity.HasOne(d => d.Laboratory).WithMany(p => p.LabSamples)
            .HasForeignKey(d => d.LaboratoryId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("lab_samples_laboratory_id_fkey");

        entity.HasOne(d => d.ReceivedByNavigation).WithMany(p => p.LabSamples)
            .HasForeignKey(d => d.ReceivedBy)
            .HasConstraintName("lab_samples_received_by_fkey");

        entity.HasOne(d => d.SoilSample).WithMany(p => p.LabSamples)
            .HasForeignKey(d => d.SoilSampleId)
            .HasConstraintName("lab_samples_soil_sample_id_fkey");
    }
}
