using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ConsultantFarmerLinkConfiguration : IEntityTypeConfiguration<ConsultantFarmerLink>
{
    public void Configure(EntityTypeBuilder<ConsultantFarmerLink> entity)
    {
        entity.HasKey(e => e.Id).HasName("consultant_farmer_links_pkey");

        entity.ToTable("consultant_farmer_links");

        entity.HasIndex(e => e.ConsultationId, "ix_consultant_farmer_links_consultation_id_d94823");

        entity.HasIndex(e => e.ConsultantId, "ix_consultant_links_consultant").HasFilter("(revoked_at IS NULL)");

        entity.HasIndex(e => e.FarmerProfileId, "ix_consultant_links_farmer").HasFilter("(revoked_at IS NULL)");

        entity.HasIndex(e => new { e.ConsultantId, e.FarmerProfileId, e.Scope }, "ux_consultant_links_active")
            .IsUnique()
            .HasFilter("(revoked_at IS NULL)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ConsultantId).HasColumnName("consultant_id");
        entity.Property(e => e.ConsultationId).HasColumnName("consultation_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.GrantedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("granted_at");
        entity.Property(e => e.RevokedAt).HasColumnName("revoked_at");
        entity.Property(e => e.Scope)
            .HasDefaultValueSql("'crop_care'::text")
            .HasColumnName("scope");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Consultant).WithMany(p => p.ConsultantFarmerLinks)
            .HasForeignKey(d => d.ConsultantId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("consultant_farmer_links_consultant_id_fkey");

        entity.HasOne(d => d.Consultation).WithMany(p => p.ConsultantFarmerLinks)
            .HasForeignKey(d => d.ConsultationId)
            .HasConstraintName("consultant_farmer_links_consultation_id_fkey");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.ConsultantFarmerLinks)
            .HasForeignKey(d => d.FarmerProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("consultant_farmer_links_farmer_profile_id_fkey");
    }
}
