using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class AdvertiserConfiguration : IEntityTypeConfiguration<Advertiser>
{
    public void Configure(EntityTypeBuilder<Advertiser> entity)
    {
        entity.HasKey(e => e.Id).HasName("advertisers_pkey");

        entity.ToTable("advertisers");

        entity.HasIndex(e => e.AdvertiserProfileId, "advertisers_advertiser_profile_id_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AdvertiserProfileId).HasColumnName("advertiser_profile_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.Currency)
            .HasMaxLength(3)
            .HasDefaultValueSql("'INR'::bpchar")
            .IsFixedLength()
            .HasColumnName("currency");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.DisplayName).HasColumnName("display_name");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'active'::text")
            .HasColumnName("status");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.AdvertiserProfile).WithOne(p => p.Advertiser)
            .HasForeignKey<Advertiser>(d => d.AdvertiserProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("advertisers_advertiser_profile_id_fkey");
    }
}
