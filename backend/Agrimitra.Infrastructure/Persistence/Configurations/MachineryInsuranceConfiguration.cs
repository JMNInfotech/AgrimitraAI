using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class MachineryInsuranceConfiguration : IEntityTypeConfiguration<MachineryInsurance>
{
    public void Configure(EntityTypeBuilder<MachineryInsurance> entity)
    {
        entity.HasKey(e => e.Id).HasName("machinery_insurance_pkey");

        entity.ToTable("machinery_insurance");

        entity.HasIndex(e => e.DocumentFileId, "ix_machinery_insurance_document_file_id_9503a4");

        entity.HasIndex(e => e.EndDate, "ix_machinery_insurance_expiry").HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => new { e.MachineryId, e.EndDate }, "ix_machinery_insurance_machine").IsDescending(false, true);

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.DocumentFileId).HasColumnName("document_file_id");
        entity.Property(e => e.EndDate).HasColumnName("end_date");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.MachineryId).HasColumnName("machinery_id");
        entity.Property(e => e.PolicyNumber).HasColumnName("policy_number");
        entity.Property(e => e.Premium)
            .HasPrecision(12, 2)
            .HasColumnName("premium");
        entity.Property(e => e.Provider).HasColumnName("provider");
        entity.Property(e => e.StartDate).HasColumnName("start_date");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.DocumentFile).WithMany(p => p.MachineryInsurances)
            .HasForeignKey(d => d.DocumentFileId)
            .HasConstraintName("machinery_insurance_document_file_id_fkey");

        entity.HasOne(d => d.Machinery).WithMany(p => p.MachineryInsurances)
            .HasForeignKey(d => d.MachineryId)
            .HasConstraintName("machinery_insurance_machinery_id_fkey");
    }
}
