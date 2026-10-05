using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ChatRoomConfiguration : IEntityTypeConfiguration<ChatRoom>
{
    public void Configure(EntityTypeBuilder<ChatRoom> entity)
    {
        entity.HasKey(e => e.Id).HasName("chat_rooms_pkey");

        entity.ToTable("chat_rooms");

        entity.HasIndex(e => e.ConsultationId, "chat_rooms_consultation_id_key").IsUnique();

        entity.HasIndex(e => e.LabBookingId, "ix_chat_rooms_lab_booking");

        entity.HasIndex(e => e.LastMessageAt, "ix_chat_rooms_last").IsDescending();

        entity.HasIndex(e => e.OrderId, "ix_chat_rooms_order");

        entity.HasIndex(e => e.TicketId, "ix_chat_rooms_ticket");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ConsultationId).HasColumnName("consultation_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.IsArchived).HasColumnName("is_archived");
        entity.Property(e => e.LabBookingId).HasColumnName("lab_booking_id");
        entity.Property(e => e.LastMessageAt).HasColumnName("last_message_at");
        entity.Property(e => e.OrderId).HasColumnName("order_id");
        entity.Property(e => e.RoomType).HasColumnName("room_type");
        entity.Property(e => e.TicketId).HasColumnName("ticket_id");
        entity.Property(e => e.Title).HasColumnName("title");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Consultation).WithOne(p => p.ChatRoom)
            .HasForeignKey<ChatRoom>(d => d.ConsultationId)
            .HasConstraintName("chat_rooms_consultation_id_fkey");

        entity.HasOne(d => d.LabBooking).WithMany(p => p.ChatRooms)
            .HasForeignKey(d => d.LabBookingId)
            .HasConstraintName("chat_rooms_lab_booking_id_fkey");

        entity.HasOne(d => d.Order).WithMany(p => p.ChatRooms)
            .HasForeignKey(d => d.OrderId)
            .HasConstraintName("chat_rooms_order_id_fkey");

        entity.HasOne(d => d.Ticket).WithMany(p => p.ChatRooms)
            .HasForeignKey(d => d.TicketId)
            .HasConstraintName("fk_chat_rooms_ticket");
    }
}
