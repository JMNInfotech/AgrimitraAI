using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> entity)
    {
        entity.HasKey(e => e.Id).HasName("messages_pkey");

        entity.ToTable("messages");

        entity.HasIndex(e => e.ReplyToId, "ix_messages_reply");

        entity.HasIndex(e => new { e.RoomId, e.CreatedAt }, "ix_messages_room_time")
            .IsDescending(false, true)
            .HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.SearchVector, "ix_messages_search").HasMethod("gin");

        entity.HasIndex(e => e.SenderUserId, "ix_messages_sender");

        entity.HasIndex(e => new { e.SenderUserId, e.ClientMessageId }, "messages_sender_user_id_client_message_id_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Body).HasColumnName("body");
        entity.Property(e => e.ClientMessageId).HasColumnName("client_message_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.EditedAt).HasColumnName("edited_at");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.MessageType)
            .HasDefaultValueSql("'text'::text")
            .HasColumnName("message_type");
        entity.Property(e => e.ReplyToId).HasColumnName("reply_to_id");
        entity.Property(e => e.RoomId).HasColumnName("room_id");
        entity.Property(e => e.SearchVector)
            .HasComputedColumnSql("to_tsvector('simple'::regconfig, COALESCE(body, ''::text))", true)
            .HasColumnName("search_vector");
        entity.Property(e => e.SenderUserId).HasColumnName("sender_user_id");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.ReplyTo).WithMany(p => p.InverseReplyTo)
            .HasForeignKey(d => d.ReplyToId)
            .HasConstraintName("messages_reply_to_id_fkey");

        entity.HasOne(d => d.Room).WithMany(p => p.Messages)
            .HasForeignKey(d => d.RoomId)
            .HasConstraintName("messages_room_id_fkey");

        entity.HasOne(d => d.SenderUser).WithMany(p => p.Messages)
            .HasForeignKey(d => d.SenderUserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("messages_sender_user_id_fkey");
    }
}
