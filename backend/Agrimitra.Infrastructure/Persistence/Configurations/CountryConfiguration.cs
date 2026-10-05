using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> entity)
    {
        entity.HasKey(e => e.Id).HasName("countries_pkey");

        entity.ToTable("countries");

        entity.HasIndex(e => e.Iso2, "countries_iso2_key").IsUnique();

        entity.HasIndex(e => e.Iso3, "countries_iso3_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasColumnName("is_active");
        entity.Property(e => e.Iso2)
            .HasMaxLength(2)
            .IsFixedLength()
            .HasColumnName("iso2");
        entity.Property(e => e.Iso3)
            .HasMaxLength(3)
            .IsFixedLength()
            .HasColumnName("iso3");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.NameLocal)
            .HasDefaultValueSql("'{}'::jsonb")
            .HasColumnType("jsonb")
            .HasColumnName("name_local");
        entity.Property(e => e.PhoneCode).HasColumnName("phone_code");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
    }
}
