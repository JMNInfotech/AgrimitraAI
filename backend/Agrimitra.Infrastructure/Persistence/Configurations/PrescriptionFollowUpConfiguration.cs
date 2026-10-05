using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class PrescriptionFollowUpConfiguration : IEntityTypeConfiguration<PrescriptionFollowUp>
{
    public void Configure(EntityTypeBuilder<PrescriptionFollowUp> entity)
    {
        entity.HasKey(e => e.Id).HasName("prescription_follow_ups_pkey");

        entity.ToTable("prescription_follow_ups");

        entity.HasIndex(e => e.FollowUpDate, "ix_prescription_followups_due").HasFilter("(status = 'pending'::text)");

        entity.HasIndex(e => e.PrescriptionId, "ix_prescription_followups_rx");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.FollowUpDate).HasColumnName("follow_up_date");
        entity.Property(e => e.Notes).HasColumnName("notes");
        entity.Property(e => e.PrescriptionId).HasColumnName("prescription_id");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'pending'::text")
            .HasColumnName("status");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Prescription).WithMany(p => p.PrescriptionFollowUps)
            .HasForeignKey(d => d.PrescriptionId)
            .HasConstraintName("prescription_follow_ups_prescription_id_fkey");
    }
}
