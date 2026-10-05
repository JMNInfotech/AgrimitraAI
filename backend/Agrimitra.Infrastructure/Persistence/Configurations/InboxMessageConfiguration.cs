using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class InboxMessageConfiguration : IEntityTypeConfiguration<InboxMessage>
{
    public void Configure(EntityTypeBuilder<InboxMessage> entity)
    {
        entity.HasKey(e => new { e.Consumer, e.MessageId }).HasName("inbox_messages_pkey");

        entity.ToTable("inbox_messages");

        entity.Property(e => e.Consumer).HasColumnName("consumer");
        entity.Property(e => e.MessageId).HasColumnName("message_id");
        entity.Property(e => e.ProcessedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("processed_at");
    }
}
