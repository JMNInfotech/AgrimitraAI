using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class LabResultConfiguration : IEntityTypeConfiguration<LabResult>
{
    public void Configure(EntityTypeBuilder<LabResult> entity)
    {
        entity.HasKey(e => e.Id).HasName("lab_results_pkey");

        entity.ToTable("lab_results");

        entity.HasIndex(e => e.TechnicianSubmittedAt, "ix_lab_results_review").HasFilter("(status = 'submitted'::text)");

        entity.HasIndex(e => e.ReviewerId, "ix_lab_results_reviewer_id_3fc625");

        entity.HasIndex(e => e.TechnicianId, "ix_lab_results_technician_id_dbcfb1");

        entity.HasIndex(e => e.TestTypeId, "ix_lab_results_test_type");

        entity.HasIndex(e => new { e.SampleId, e.TestTypeId }, "lab_results_sample_id_test_type_id_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.ReviewNotes).HasColumnName("review_notes");
        entity.Property(e => e.ReviewedAt).HasColumnName("reviewed_at");
        entity.Property(e => e.ReviewerId).HasColumnName("reviewer_id");
        entity.Property(e => e.SampleId).HasColumnName("sample_id");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'draft'::text")
            .HasColumnName("status");
        entity.Property(e => e.Summary).HasColumnName("summary");
        entity.Property(e => e.TechnicianId).HasColumnName("technician_id");
        entity.Property(e => e.TechnicianSubmittedAt).HasColumnName("technician_submitted_at");
        entity.Property(e => e.TestTypeId).HasColumnName("test_type_id");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Reviewer).WithMany(p => p.LabResultReviewers)
            .HasForeignKey(d => d.ReviewerId)
            .HasConstraintName("lab_results_reviewer_id_fkey");

        entity.HasOne(d => d.Sample).WithMany(p => p.LabResults)
            .HasForeignKey(d => d.SampleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("lab_results_sample_id_fkey");

        entity.HasOne(d => d.Technician).WithMany(p => p.LabResultTechnicians)
            .HasForeignKey(d => d.TechnicianId)
            .HasConstraintName("lab_results_technician_id_fkey");

        entity.HasOne(d => d.TestType).WithMany(p => p.LabResults)
            .HasForeignKey(d => d.TestTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("lab_results_test_type_id_fkey");
    }
}
