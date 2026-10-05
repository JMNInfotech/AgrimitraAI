using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ChatParticipantConfiguration : IEntityTypeConfiguration<ChatParticipant>
{
    public void Configure(EntityTypeBuilder<ChatParticipant> entity)
    {
        entity.HasKey(e => new { e.RoomId, e.UserId }).HasName("chat_participants_pkey");

        entity.ToTable("chat_participants");

        entity.HasIndex(e => e.UserId, "ix_chat_participants_user").HasFilter("(left_at IS NULL)");

        entity.Property(e => e.RoomId).HasColumnName("room_id");
        entity.Property(e => e.UserId).HasColumnName("user_id");
        entity.Property(e => e.IsMuted).HasColumnName("is_muted");
        entity.Property(e => e.JoinedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("joined_at");
        entity.Property(e => e.LastReadAt).HasColumnName("last_read_at");
        entity.Property(e => e.LeftAt).HasColumnName("left_at");
        entity.Property(e => e.ParticipantRole)
            .HasDefaultValueSql("'member'::text")
            .HasColumnName("participant_role");

        entity.HasOne(d => d.Room).WithMany(p => p.ChatParticipants)
            .HasForeignKey(d => d.RoomId)
            .HasConstraintName("chat_participants_room_id_fkey");

        entity.HasOne(d => d.User).WithMany(p => p.ChatParticipants)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("chat_participants_user_id_fkey");
    }
}
