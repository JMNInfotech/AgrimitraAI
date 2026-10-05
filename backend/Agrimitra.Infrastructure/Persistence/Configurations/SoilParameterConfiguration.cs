using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class SoilParameterConfiguration : IEntityTypeConfiguration<SoilParameter>
{
    public void Configure(EntityTypeBuilder<SoilParameter> entity)
    {
        entity.HasKey(e => e.Id).HasName("soil_parameters_pkey");

        entity.ToTable("soil_parameters");

        entity.HasIndex(e => e.Code, "soil_parameters_code_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Category).HasColumnName("category");
        entity.Property(e => e.Code).HasColumnName("code");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DefaultUnit).HasColumnName("default_unit");
        entity.Property(e => e.MaxPlausible)
            .HasPrecision(14, 4)
            .HasColumnName("max_plausible");
        entity.Property(e => e.MinPlausible)
            .HasPrecision(14, 4)
            .HasColumnName("min_plausible");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.NameLocal)
            .HasDefaultValueSql("'{}'::jsonb")
            .HasColumnType("jsonb")
            .HasColumnName("name_local");
        entity.Property(e => e.SortOrder).HasColumnName("sort_order");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
    }
}
