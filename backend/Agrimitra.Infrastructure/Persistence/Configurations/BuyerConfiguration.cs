using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class BuyerConfiguration : IEntityTypeConfiguration<Buyer>
{
    public void Configure(EntityTypeBuilder<Buyer> entity)
    {
        entity.HasKey(e => e.Id).HasName("buyers_pkey");

        entity.ToTable("buyers");

        entity.HasIndex(e => e.AddressId, "ix_buyers_address_id_ed9ac5");

        entity.HasIndex(e => e.FarmerProfileId, "ix_buyers_farmer").HasFilter("(NOT is_deleted)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AddressId).HasColumnName("address_id");
        entity.Property(e => e.BuyerType).HasColumnName("buyer_type");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.MobileNumber).HasColumnName("mobile_number");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.Notes).HasColumnName("notes");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Address).WithMany(p => p.Buyers)
            .HasForeignKey(d => d.AddressId)
            .HasConstraintName("buyers_address_id_fkey");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.Buyers)
            .HasForeignKey(d => d.FarmerProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("buyers_farmer_profile_id_fkey");
    }
}
