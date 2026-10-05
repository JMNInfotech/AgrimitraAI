using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class SupportTicketAttachmentConfiguration : IEntityTypeConfiguration<SupportTicketAttachment>
{
    public void Configure(EntityTypeBuilder<SupportTicketAttachment> entity)
    {
        entity.HasKey(e => e.Id).HasName("support_ticket_attachments_pkey");

        entity.ToTable("support_ticket_attachments");

        entity.HasIndex(e => e.FileObjectId, "ix_support_ticket_attachments_file_object_id_a20abb");

        entity.HasIndex(e => e.MessageId, "ix_ticket_attachments_message");

        entity.HasIndex(e => e.TicketId, "ix_ticket_attachments_ticket");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.FileObjectId).HasColumnName("file_object_id");
        entity.Property(e => e.MessageId).HasColumnName("message_id");
        entity.Property(e => e.TicketId).HasColumnName("ticket_id");

        entity.HasOne(d => d.FileObject).WithMany(p => p.SupportTicketAttachments)
            .HasForeignKey(d => d.FileObjectId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("support_ticket_attachments_file_object_id_fkey");

        entity.HasOne(d => d.Message).WithMany(p => p.SupportTicketAttachments)
            .HasForeignKey(d => d.MessageId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("support_ticket_attachments_message_id_fkey");

        entity.HasOne(d => d.Ticket).WithMany(p => p.SupportTicketAttachments)
            .HasForeignKey(d => d.TicketId)
            .HasConstraintName("support_ticket_attachments_ticket_id_fkey");
    }
}
