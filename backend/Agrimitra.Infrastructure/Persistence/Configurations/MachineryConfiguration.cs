using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class MachineryConfiguration : IEntityTypeConfiguration<Machinery>
{
    public void Configure(EntityTypeBuilder<Machinery> entity)
    {
        entity.HasKey(e => e.Id).HasName("machinery_pkey");

        entity.ToTable("machinery");

        entity.HasIndex(e => new { e.FarmerProfileId, e.Status }, "ix_machinery_farmer").HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.NextServiceDate, "ix_machinery_next_service").HasFilter("((next_service_date IS NOT NULL) AND (NOT is_deleted))");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.MachineryType).HasColumnName("machinery_type");
        entity.Property(e => e.Make).HasColumnName("make");
        entity.Property(e => e.Model).HasColumnName("model");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.NextServiceDate).HasColumnName("next_service_date");
        entity.Property(e => e.Notes).HasColumnName("notes");
        entity.Property(e => e.PurchaseDate).HasColumnName("purchase_date");
        entity.Property(e => e.PurchasePrice)
            .HasPrecision(14, 2)
            .HasColumnName("purchase_price");
        entity.Property(e => e.RegistrationNumber).HasColumnName("registration_number");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'active'::text")
            .HasColumnName("status");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.Machineries)
            .HasForeignKey(d => d.FarmerProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("machinery_farmer_profile_id_fkey");
    }
}
