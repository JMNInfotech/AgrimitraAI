using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class VillageConfiguration : IEntityTypeConfiguration<Village>
{
    public void Configure(EntityTypeBuilder<Village> entity)
    {
        entity.HasKey(e => e.Id).HasName("villages_pkey");

        entity.ToTable("villages");

        entity.HasIndex(e => e.Name, "ix_villages_name_trgm")
            .HasMethod("gin")
            .HasOperators(new[] { "gin_trgm_ops" });

        entity.HasIndex(e => e.Pincode, "ix_villages_pincode").HasFilter("(pincode IS NOT NULL)");

        entity.HasIndex(e => e.TalukaId, "ix_villages_taluka");

        entity.HasIndex(e => new { e.TalukaId, e.Code }, "villages_taluka_id_code_key").IsUnique();

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
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasColumnName("is_active");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.NameLocal)
            .HasDefaultValueSql("'{}'::jsonb")
            .HasColumnType("jsonb")
            .HasColumnName("name_local");
        entity.Property(e => e.Pincode).HasColumnName("pincode");
        entity.Property(e => e.TalukaId).HasColumnName("taluka_id");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Taluka).WithMany(p => p.Villages)
            .HasForeignKey(d => d.TalukaId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("villages_taluka_id_fkey");
    }
}
