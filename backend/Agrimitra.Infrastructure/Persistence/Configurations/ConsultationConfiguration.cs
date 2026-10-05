using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ConsultationConfiguration : IEntityTypeConfiguration<Consultation>
{
    public void Configure(EntityTypeBuilder<Consultation> entity)
    {
        entity.HasKey(e => e.Id).HasName("consultations_pkey");

        entity.ToTable("consultations");

        entity.HasIndex(e => e.RequestId, "consultations_request_id_key").IsUnique();

        entity.HasIndex(e => e.DataAccessExpiresAt, "ix_consultations_access").HasFilter("(data_access_expires_at IS NOT NULL)");

        entity.HasIndex(e => e.ChatRoomId, "ix_consultations_chat_room");

        entity.HasIndex(e => new { e.ConsultantId, e.Status }, "ix_consultations_consultant");

        entity.HasIndex(e => e.CropCycleId, "ix_consultations_cycle");

        entity.HasIndex(e => new { e.FarmerProfileId, e.CreatedAt }, "ix_consultations_farmer").IsDescending(false, true);

        entity.HasIndex(e => e.LandId, "ix_consultations_land");

        entity.HasIndex(e => e.PaymentId, "ix_consultations_payment");

        entity.HasIndex(e => e.ServiceId, "ix_consultations_service");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ChatRoomId).HasColumnName("chat_room_id");
        entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
        entity.Property(e => e.ConsultantId).HasColumnName("consultant_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropCycleId).HasColumnName("crop_cycle_id");
        entity.Property(e => e.Currency)
            .HasMaxLength(3)
            .HasDefaultValueSql("'INR'::bpchar")
            .IsFixedLength()
            .HasColumnName("currency");
        entity.Property(e => e.DataAccessExpiresAt).HasColumnName("data_access_expires_at");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.FeeAmount)
            .HasPrecision(12, 2)
            .HasColumnName("fee_amount");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.LandId).HasColumnName("land_id");
        entity.Property(e => e.PaymentId).HasColumnName("payment_id");
        entity.Property(e => e.RequestId).HasColumnName("request_id");
        entity.Property(e => e.ServiceId).HasColumnName("service_id");
        entity.Property(e => e.StartedAt).HasColumnName("started_at");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'accepted'::text")
            .HasColumnName("status");
        entity.Property(e => e.Summary).HasColumnName("summary");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.ChatRoomNavigation).WithMany(p => p.Consultations)
            .HasForeignKey(d => d.ChatRoomId)
            .HasConstraintName("fk_consultations_chat_room");

        entity.HasOne(d => d.Consultant).WithMany(p => p.Consultations)
            .HasForeignKey(d => d.ConsultantId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("consultations_consultant_id_fkey");

        entity.HasOne(d => d.CropCycle).WithMany(p => p.Consultations)
            .HasForeignKey(d => d.CropCycleId)
            .HasConstraintName("consultations_crop_cycle_id_fkey");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.Consultations)
            .HasForeignKey(d => d.FarmerProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("consultations_farmer_profile_id_fkey");

        entity.HasOne(d => d.Land).WithMany(p => p.Consultations)
            .HasForeignKey(d => d.LandId)
            .HasConstraintName("consultations_land_id_fkey");

        entity.HasOne(d => d.Payment).WithMany(p => p.Consultations)
            .HasForeignKey(d => d.PaymentId)
            .HasConstraintName("fk_consultations_payment");

        entity.HasOne(d => d.Request).WithOne(p => p.Consultation)
            .HasForeignKey<Consultation>(d => d.RequestId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("consultations_request_id_fkey");

        entity.HasOne(d => d.Service).WithMany(p => p.Consultations)
            .HasForeignKey(d => d.ServiceId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("consultations_service_id_fkey");
    }
}
