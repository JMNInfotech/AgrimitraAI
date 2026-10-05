using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class PincodeConfiguration : IEntityTypeConfiguration<Pincode>
{
    public void Configure(EntityTypeBuilder<Pincode> entity)
    {
        entity.HasKey(e => e.Pincode1).HasName("pincodes_pkey");

        entity.ToTable("pincodes");

        entity.HasIndex(e => e.CountryId, "ix_pincodes_country_id_ebdc79");

        entity.HasIndex(e => e.DistrictId, "ix_pincodes_district_id_528790");

        entity.HasIndex(e => e.StateId, "ix_pincodes_state_id_6a3d64");

        entity.Property(e => e.Pincode1).HasColumnName("pincode");
        entity.Property(e => e.CountryId).HasColumnName("country_id");
        entity.Property(e => e.DistrictId).HasColumnName("district_id");
        entity.Property(e => e.StateId).HasColumnName("state_id");

        entity.HasOne(d => d.Country).WithMany(p => p.Pincodes)
            .HasForeignKey(d => d.CountryId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("pincodes_country_id_fkey");

        entity.HasOne(d => d.District).WithMany(p => p.Pincodes)
            .HasForeignKey(d => d.DistrictId)
            .HasConstraintName("pincodes_district_id_fkey");

        entity.HasOne(d => d.State).WithMany(p => p.Pincodes)
            .HasForeignKey(d => d.StateId)
            .HasConstraintName("pincodes_state_id_fkey");
    }
}
