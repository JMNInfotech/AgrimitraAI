using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ConsentRecordConfiguration : IEntityTypeConfiguration<ConsentRecord>
{
    public void Configure(EntityTypeBuilder<ConsentRecord> entity)
    {
        entity.HasKey(e => e.Id).HasName("consent_records_pkey");

        entity.ToTable("consent_records");

        entity.HasIndex(e => new { e.UserId, e.ConsentType, e.CreatedAt }, "ix_consent_user_type").IsDescending(false, false, true);

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ConsentType).HasColumnName("consent_type");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.IpAddress).HasColumnName("ip_address");
        entity.Property(e => e.IsGranted).HasColumnName("is_granted");
        entity.Property(e => e.PolicyVersion).HasColumnName("policy_version");
        entity.Property(e => e.Source).HasColumnName("source");
        entity.Property(e => e.UserId).HasColumnName("user_id");

        entity.HasOne(d => d.User).WithMany(p => p.ConsentRecords)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("consent_records_user_id_fkey");
    }
}
