using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ProductLocationConfiguration : IEntityTypeConfiguration<ProductLocation>
{
    public void Configure(EntityTypeBuilder<ProductLocation> entity)
    {
        entity.HasKey(e => e.Id).HasName("product_locations_pkey");

        entity.ToTable("product_locations");

        entity.HasIndex(e => e.AddressId, "ix_product_locations_address_id_e334cd");

        entity.HasIndex(e => e.Location, "ix_product_locations_geo").HasMethod("gist");

        entity.HasIndex(e => e.ProductId, "ix_product_locations_product");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AddressId).HasColumnName("address_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.Location)
            .HasColumnType("geography(Point,4326)")
            .HasColumnName("location");
        entity.Property(e => e.ProductId).HasColumnName("product_id");
        entity.Property(e => e.ServiceRadiusKm)
            .HasPrecision(8, 2)
            .HasColumnName("service_radius_km");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Address).WithMany(p => p.ProductLocations)
            .HasForeignKey(d => d.AddressId)
            .HasConstraintName("product_locations_address_id_fkey");

        entity.HasOne(d => d.Product).WithMany(p => p.ProductLocations)
            .HasForeignKey(d => d.ProductId)
            .HasConstraintName("product_locations_product_id_fkey");
    }
}
