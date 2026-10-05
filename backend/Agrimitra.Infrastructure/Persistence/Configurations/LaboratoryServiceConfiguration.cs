using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class LaboratoryServiceConfiguration : IEntityTypeConfiguration<LaboratoryService>
{
    public void Configure(EntityTypeBuilder<LaboratoryService> entity)
    {
        entity.HasKey(e => e.Id).HasName("laboratory_services_pkey");

        entity.ToTable("laboratory_services");

        entity.HasIndex(e => e.TestTypeId, "ix_lab_services_test");

        entity.HasIndex(e => new { e.LaboratoryId, e.TestTypeId }, "ux_lab_services_lab_test")
            .IsUnique()
            .HasFilter("(NOT is_deleted)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.Currency)
            .HasMaxLength(3)
            .HasDefaultValueSql("'INR'::bpchar")
            .IsFixedLength()
            .HasColumnName("currency");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasColumnName("is_active");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.LaboratoryId).HasColumnName("laboratory_id");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.Price)
            .HasPrecision(12, 2)
            .HasColumnName("price");
        entity.Property(e => e.SampleCollectionAvailable).HasColumnName("sample_collection_available");
        entity.Property(e => e.TestTypeId).HasColumnName("test_type_id");
        entity.Property(e => e.TurnaroundDays)
            .HasDefaultValue(7)
            .HasColumnName("turnaround_days");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Laboratory).WithMany(p => p.LaboratoryServices)
            .HasForeignKey(d => d.LaboratoryId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("laboratory_services_laboratory_id_fkey");

        entity.HasOne(d => d.TestType).WithMany(p => p.LaboratoryServices)
            .HasForeignKey(d => d.TestTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("laboratory_services_test_type_id_fkey");
    }
}
