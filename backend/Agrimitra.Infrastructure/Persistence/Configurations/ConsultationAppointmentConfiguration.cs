using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ConsultationAppointmentConfiguration : IEntityTypeConfiguration<ConsultationAppointment>
{
    public void Configure(EntityTypeBuilder<ConsultationAppointment> entity)
    {
        entity.HasKey(e => e.Id).HasName("consultation_appointments_pkey");

        entity.ToTable("consultation_appointments");

        entity.HasIndex(e => e.ConsultationId, "ix_appointments_consultation");

        entity.HasIndex(e => e.StartAt, "ix_appointments_start").HasFilter("(status = 'scheduled'::text)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ConsultationId).HasColumnName("consultation_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.EndAt).HasColumnName("end_at");
        entity.Property(e => e.Location)
            .HasColumnType("geography(Point,4326)")
            .HasColumnName("location");
        entity.Property(e => e.MeetingUrl).HasColumnName("meeting_url");
        entity.Property(e => e.Mode).HasColumnName("mode");
        entity.Property(e => e.ReminderSentAt).HasColumnName("reminder_sent_at");
        entity.Property(e => e.StartAt).HasColumnName("start_at");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'scheduled'::text")
            .HasColumnName("status");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Consultation).WithMany(p => p.ConsultationAppointments)
            .HasForeignKey(d => d.ConsultationId)
            .HasConstraintName("consultation_appointments_consultation_id_fkey");
    }
}
