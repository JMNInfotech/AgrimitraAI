using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class MessageReadReceiptConfiguration : IEntityTypeConfiguration<MessageReadReceipt>
{
    public void Configure(EntityTypeBuilder<MessageReadReceipt> entity)
    {
        entity.HasKey(e => new { e.MessageId, e.UserId }).HasName("message_read_receipts_pkey");

        entity.ToTable("message_read_receipts");

        entity.HasIndex(e => new { e.UserId, e.ReadAt }, "ix_receipts_user");

        entity.Property(e => e.MessageId).HasColumnName("message_id");
        entity.Property(e => e.UserId).HasColumnName("user_id");
        entity.Property(e => e.DeliveredAt).HasColumnName("delivered_at");
        entity.Property(e => e.ReadAt).HasColumnName("read_at");

        entity.HasOne(d => d.Message).WithMany(p => p.MessageReadReceipts)
            .HasForeignKey(d => d.MessageId)
            .HasConstraintName("message_read_receipts_message_id_fkey");

        entity.HasOne(d => d.User).WithMany(p => p.MessageReadReceipts)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("message_read_receipts_user_id_fkey");
    }
}
