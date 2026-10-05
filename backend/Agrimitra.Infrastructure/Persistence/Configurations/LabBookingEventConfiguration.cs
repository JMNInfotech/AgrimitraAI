using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class LabBookingEventConfiguration : IEntityTypeConfiguration<LabBookingEvent>
{
    public void Configure(EntityTypeBuilder<LabBookingEvent> entity)
    {
        entity.HasKey(e => e.Id).HasName("lab_booking_events_pkey");

        entity.ToTable("lab_booking_events");

        entity.HasIndex(e => e.ActorUserId, "ix_lab_booking_events_actor_user_id_b15592");

        entity.HasIndex(e => new { e.BookingId, e.CreatedAt }, "ix_lab_booking_events_booking");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ActorUserId).HasColumnName("actor_user_id");
        entity.Property(e => e.BookingId).HasColumnName("booking_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.FromStatus).HasColumnName("from_status");
        entity.Property(e => e.Note).HasColumnName("note");
        entity.Property(e => e.ToStatus).HasColumnName("to_status");

        entity.HasOne(d => d.ActorUser).WithMany(p => p.LabBookingEvents)
            .HasForeignKey(d => d.ActorUserId)
            .HasConstraintName("lab_booking_events_actor_user_id_fkey");

        entity.HasOne(d => d.Booking).WithMany(p => p.LabBookingEvents)
            .HasForeignKey(d => d.BookingId)
            .HasConstraintName("lab_booking_events_booking_id_fkey");
    }
}
