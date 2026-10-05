using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class SupportTicketMessageConfiguration : IEntityTypeConfiguration<SupportTicketMessage>
{
    public void Configure(EntityTypeBuilder<SupportTicketMessage> entity)
    {
        entity.HasKey(e => e.Id).HasName("support_ticket_messages_pkey");

        entity.ToTable("support_ticket_messages");

        entity.HasIndex(e => e.AuthorUserId, "ix_support_ticket_messages_author_user_id_873964");

        entity.HasIndex(e => new { e.TicketId, e.CreatedAt }, "ix_ticket_messages_ticket");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AuthorUserId).HasColumnName("author_user_id");
        entity.Property(e => e.Body).HasColumnName("body");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.IsInternal).HasColumnName("is_internal");
        entity.Property(e => e.TicketId).HasColumnName("ticket_id");

        entity.HasOne(d => d.AuthorUser).WithMany(p => p.SupportTicketMessages)
            .HasForeignKey(d => d.AuthorUserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("support_ticket_messages_author_user_id_fkey");

        entity.HasOne(d => d.Ticket).WithMany(p => p.SupportTicketMessages)
            .HasForeignKey(d => d.TicketId)
            .HasConstraintName("support_ticket_messages_ticket_id_fkey");
    }
}
