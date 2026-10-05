using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class IdempotencyKeyConfiguration : IEntityTypeConfiguration<IdempotencyKey>
{
    public void Configure(EntityTypeBuilder<IdempotencyKey> entity)
    {
        entity.HasKey(e => new { e.UserId, e.Key }).HasName("idempotency_keys_pkey");

        entity.ToTable("idempotency_keys");

        entity.HasIndex(e => e.ExpiresAt, "ix_idempotency_expiry");

        entity.Property(e => e.UserId).HasColumnName("user_id");
        entity.Property(e => e.Key).HasColumnName("key");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
        entity.Property(e => e.RequestHash).HasColumnName("request_hash");
        entity.Property(e => e.ResponseBody)
            .HasColumnType("jsonb")
            .HasColumnName("response_body");
        entity.Property(e => e.ResponseStatus).HasColumnName("response_status");
    }
}
