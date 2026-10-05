using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> entity)
    {
        entity.HasKey(e => e.Id).HasName("outbox_messages_pkey");

        entity.ToTable("outbox_messages");

        entity.HasIndex(e => e.AvailableAt, "ix_outbox_pending").HasFilter("(processed_at IS NULL)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AggregateId).HasColumnName("aggregate_id");
        entity.Property(e => e.AggregateType).HasColumnName("aggregate_type");
        entity.Property(e => e.Attempts).HasColumnName("attempts");
        entity.Property(e => e.AvailableAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("available_at");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.EventType).HasColumnName("event_type");
        entity.Property(e => e.Headers)
            .HasDefaultValueSql("'{}'::jsonb")
            .HasColumnType("jsonb")
            .HasColumnName("headers");
        entity.Property(e => e.LastError).HasColumnName("last_error");
        entity.Property(e => e.Payload)
            .HasColumnType("jsonb")
            .HasColumnName("payload");
        entity.Property(e => e.ProcessedAt).HasColumnName("processed_at");
    }
}
