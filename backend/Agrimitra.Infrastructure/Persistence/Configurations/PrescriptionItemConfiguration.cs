using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class PrescriptionItemConfiguration : IEntityTypeConfiguration<PrescriptionItem>
{
    public void Configure(EntityTypeBuilder<PrescriptionItem> entity)
    {
        entity.HasKey(e => e.Id).HasName("prescription_items_pkey");

        entity.ToTable("prescription_items");

        entity.HasIndex(e => new { e.PrescriptionId, e.LineNo }, "prescription_items_prescription_id_line_no_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ActiveIngredient).HasColumnName("active_ingredient");
        entity.Property(e => e.ApplicationMethod).HasColumnName("application_method");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.Dosage).HasColumnName("dosage");
        entity.Property(e => e.DosageUnit).HasColumnName("dosage_unit");
        entity.Property(e => e.DosageValue)
            .HasPrecision(12, 3)
            .HasColumnName("dosage_value");
        entity.Property(e => e.DurationDays).HasColumnName("duration_days");
        entity.Property(e => e.Frequency).HasColumnName("frequency");
        entity.Property(e => e.LineNo).HasColumnName("line_no");
        entity.Property(e => e.Precautions).HasColumnName("precautions");
        entity.Property(e => e.PrescriptionId).HasColumnName("prescription_id");
        entity.Property(e => e.Treatment).HasColumnName("treatment");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Prescription).WithMany(p => p.PrescriptionItems)
            .HasForeignKey(d => d.PrescriptionId)
            .HasConstraintName("prescription_items_prescription_id_fkey");
    }
}
