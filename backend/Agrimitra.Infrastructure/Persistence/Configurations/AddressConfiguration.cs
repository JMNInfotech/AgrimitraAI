using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> entity)
    {
        entity.HasKey(e => e.Id).HasName("addresses_pkey");

        entity.ToTable("addresses");

        entity.HasIndex(e => e.CountryId, "ix_addresses_country");

        entity.HasIndex(e => e.DistrictId, "ix_addresses_district");

        entity.HasIndex(e => e.Location, "ix_addresses_location").HasMethod("gist");

        entity.HasIndex(e => e.Pincode, "ix_addresses_pincode").HasFilter("(pincode IS NOT NULL)");

        entity.HasIndex(e => e.StateId, "ix_addresses_state");

        entity.HasIndex(e => e.TalukaId, "ix_addresses_taluka");

        entity.HasIndex(e => e.VillageId, "ix_addresses_village");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.City).HasColumnName("city");
        entity.Property(e => e.CountryId).HasColumnName("country_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.DistrictId).HasColumnName("district_id");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.Landmark).HasColumnName("landmark");
        entity.Property(e => e.Line1).HasColumnName("line1");
        entity.Property(e => e.Line2).HasColumnName("line2");
        entity.Property(e => e.Location)
            .HasColumnType("geography(Point,4326)")
            .HasColumnName("location");
        entity.Property(e => e.Pincode).HasColumnName("pincode");
        entity.Property(e => e.StateId).HasColumnName("state_id");
        entity.Property(e => e.TalukaId).HasColumnName("taluka_id");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.VillageId).HasColumnName("village_id");

        entity.HasOne(d => d.Country).WithMany(p => p.Addresses)
            .HasForeignKey(d => d.CountryId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("addresses_country_id_fkey");

        entity.HasOne(d => d.District).WithMany(p => p.Addresses)
            .HasForeignKey(d => d.DistrictId)
            .HasConstraintName("addresses_district_id_fkey");

        entity.HasOne(d => d.State).WithMany(p => p.Addresses)
            .HasForeignKey(d => d.StateId)
            .HasConstraintName("addresses_state_id_fkey");

        entity.HasOne(d => d.Taluka).WithMany(p => p.Addresses)
            .HasForeignKey(d => d.TalukaId)
            .HasConstraintName("addresses_taluka_id_fkey");

        entity.HasOne(d => d.Village).WithMany(p => p.Addresses)
            .HasForeignKey(d => d.VillageId)
            .HasConstraintName("addresses_village_id_fkey");
    }
}
