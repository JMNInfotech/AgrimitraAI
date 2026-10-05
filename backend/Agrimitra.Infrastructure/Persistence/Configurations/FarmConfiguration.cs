using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class FarmConfiguration : IEntityTypeConfiguration<Farm>
{
    public void Configure(EntityTypeBuilder<Farm> entity)
    {
        entity.HasKey(e => e.Id).HasName("farms_pkey");

        entity.ToTable("farms");

        entity.HasIndex(e => e.AddressId, "ix_farms_address_id_d483f1");

        entity.HasIndex(e => e.FarmerProfileId, "ix_farms_farmer").HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.OrganizationId, "ix_farms_organization_id_4489e7");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AddressId).HasColumnName("address_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.OrganizationId).HasColumnName("organization_id");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'active'::text")
            .HasColumnName("status");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Address).WithMany(p => p.Farms)
            .HasForeignKey(d => d.AddressId)
            .HasConstraintName("farms_address_id_fkey");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.Farms)
            .HasForeignKey(d => d.FarmerProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("farms_farmer_profile_id_fkey");

        entity.HasOne(d => d.Organization).WithMany(p => p.Farms)
            .HasForeignKey(d => d.OrganizationId)
            .HasConstraintName("farms_organization_id_fkey");
    }
}
