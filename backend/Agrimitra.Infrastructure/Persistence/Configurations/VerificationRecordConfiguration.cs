using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class VerificationRecordConfiguration : IEntityTypeConfiguration<VerificationRecord>
{
    public void Configure(EntityTypeBuilder<VerificationRecord> entity)
    {
        entity.HasKey(e => e.Id).HasName("verification_records_pkey");

        entity.ToTable("verification_records");

        entity.HasIndex(e => new { e.Status, e.SubmittedAt }, "ix_verification_queue").HasFilter("(status = ANY (ARRAY['pending'::text, 'under_review'::text]))");

        entity.HasIndex(e => e.ReviewedBy, "ix_verification_records_reviewed_by_218052");

        entity.HasIndex(e => new { e.SubjectType, e.SubjectId, e.SubmittedAt }, "ix_verification_subject").IsDescending(false, false, true);

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.RejectionReason).HasColumnName("rejection_reason");
        entity.Property(e => e.ReviewedAt).HasColumnName("reviewed_at");
        entity.Property(e => e.ReviewedBy).HasColumnName("reviewed_by");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'pending'::text")
            .HasColumnName("status");
        entity.Property(e => e.SubjectId).HasColumnName("subject_id");
        entity.Property(e => e.SubjectType).HasColumnName("subject_type");
        entity.Property(e => e.SubmittedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("submitted_at");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.ReviewedByNavigation).WithMany(p => p.VerificationRecords)
            .HasForeignKey(d => d.ReviewedBy)
            .HasConstraintName("verification_records_reviewed_by_fkey");
    }
}
