using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class TalukaConfiguration : IEntityTypeConfiguration<Taluka>
{
    public void Configure(EntityTypeBuilder<Taluka> entity)
    {
        entity.HasKey(e => e.Id).HasName("talukas_pkey");

        entity.ToTable("talukas");

        entity.HasIndex(e => e.DistrictId, "ix_talukas_district");

        entity.HasIndex(e => new { e.DistrictId, e.Code }, "talukas_district_id_code_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Centroid)
            .HasColumnType("geography(Point,4326)")
            .HasColumnName("centroid");
        entity.Property(e => e.Code).HasColumnName("code");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DistrictId).HasColumnName("district_id");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasColumnName("is_active");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.NameLocal)
            .HasDefaultValueSql("'{}'::jsonb")
            .HasColumnType("jsonb")
            .HasColumnName("name_local");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.District).WithMany(p => p.Talukas)
            .HasForeignKey(d => d.DistrictId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("talukas_district_id_fkey");
    }
}
