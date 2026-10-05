using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class LabBookingConfiguration : IEntityTypeConfiguration<LabBooking>
{
    public void Configure(EntityTypeBuilder<LabBooking> entity)
    {
        entity.HasKey(e => e.Id).HasName("lab_bookings_pkey");

        entity.ToTable("lab_bookings");

        entity.HasIndex(e => e.CropId, "ix_lab_bookings_crop");

        entity.HasIndex(e => e.CropCycleId, "ix_lab_bookings_cycle");

        entity.HasIndex(e => new { e.FarmerProfileId, e.CreatedAt }, "ix_lab_bookings_farmer").IsDescending(false, true);

        entity.HasIndex(e => new { e.LaboratoryId, e.Status, e.CreatedAt }, "ix_lab_bookings_lab_status")
            .IsDescending(false, false, true)
            .HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.LandId, "ix_lab_bookings_land");

        entity.HasIndex(e => e.PaymentId, "ix_lab_bookings_payment");

        entity.HasIndex(e => e.ServiceId, "ix_lab_bookings_service");

        entity.HasIndex(e => e.BookingNumber, "lab_bookings_booking_number_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.BookingNumber).HasColumnName("booking_number");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropCycleId).HasColumnName("crop_cycle_id");
        entity.Property(e => e.CropId).HasColumnName("crop_id");
        entity.Property(e => e.Currency)
            .HasMaxLength(3)
            .HasDefaultValueSql("'INR'::bpchar")
            .IsFixedLength()
            .HasColumnName("currency");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.LaboratoryId).HasColumnName("laboratory_id");
        entity.Property(e => e.LandId).HasColumnName("land_id");
        entity.Property(e => e.Notes).HasColumnName("notes");
        entity.Property(e => e.PaymentId).HasColumnName("payment_id");
        entity.Property(e => e.PreferredDate).HasColumnName("preferred_date");
        entity.Property(e => e.Price)
            .HasPrecision(12, 2)
            .HasColumnName("price");
        entity.Property(e => e.SampleHandover)
            .HasDefaultValueSql("'drop_off'::text")
            .HasColumnName("sample_handover");
        entity.Property(e => e.SampleType).HasColumnName("sample_type");
        entity.Property(e => e.ServiceId).HasColumnName("service_id");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'submitted'::text")
            .HasColumnName("status");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.CropCycle).WithMany(p => p.LabBookings)
            .HasForeignKey(d => d.CropCycleId)
            .HasConstraintName("lab_bookings_crop_cycle_id_fkey");

        entity.HasOne(d => d.Crop).WithMany(p => p.LabBookings)
            .HasForeignKey(d => d.CropId)
            .HasConstraintName("lab_bookings_crop_id_fkey");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.LabBookings)
            .HasForeignKey(d => d.FarmerProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("lab_bookings_farmer_profile_id_fkey");

        entity.HasOne(d => d.Laboratory).WithMany(p => p.LabBookings)
            .HasForeignKey(d => d.LaboratoryId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("lab_bookings_laboratory_id_fkey");

        entity.HasOne(d => d.Land).WithMany(p => p.LabBookings)
            .HasForeignKey(d => d.LandId)
            .HasConstraintName("lab_bookings_land_id_fkey");

        entity.HasOne(d => d.Payment).WithMany(p => p.LabBookings)
            .HasForeignKey(d => d.PaymentId)
            .HasConstraintName("fk_lab_bookings_payment");

        entity.HasOne(d => d.Service).WithMany(p => p.LabBookings)
            .HasForeignKey(d => d.ServiceId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("lab_bookings_service_id_fkey");
    }
}
