using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ConsultationRequestConfiguration : IEntityTypeConfiguration<ConsultationRequest>
{
    public void Configure(EntityTypeBuilder<ConsultationRequest> entity)
    {
        entity.HasKey(e => e.Id).HasName("consultation_requests_pkey");

        entity.ToTable("consultation_requests");

        entity.HasIndex(e => new { e.ConsultantId, e.Status, e.RequestedStartAt }, "ix_consultation_requests_consultant");

        entity.HasIndex(e => e.CropCycleId, "ix_consultation_requests_cycle");

        entity.HasIndex(e => new { e.FarmerProfileId, e.CreatedAt }, "ix_consultation_requests_farmer").IsDescending(false, true);

        entity.HasIndex(e => e.LandId, "ix_consultation_requests_land");

        entity.HasIndex(e => e.ServiceId, "ix_consultation_requests_service");

        entity.HasIndex(e => e.DiseaseScanId, "ix_requests_scan");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ConsultantId).HasColumnName("consultant_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropCycleId).HasColumnName("crop_cycle_id");
        entity.Property(e => e.DecidedAt).HasColumnName("decided_at");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.DiseaseScanId).HasColumnName("disease_scan_id");
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.LandId).HasColumnName("land_id");
        entity.Property(e => e.ProblemDescription).HasColumnName("problem_description");
        entity.Property(e => e.RejectionReason).HasColumnName("rejection_reason");
        entity.Property(e => e.RequestedStartAt).HasColumnName("requested_start_at");
        entity.Property(e => e.ServiceId).HasColumnName("service_id");
        entity.Property(e => e.ShareFarmDataConsent).HasColumnName("share_farm_data_consent");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'requested'::text")
            .HasColumnName("status");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Consultant).WithMany(p => p.ConsultationRequests)
            .HasForeignKey(d => d.ConsultantId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("consultation_requests_consultant_id_fkey");

        entity.HasOne(d => d.CropCycle).WithMany(p => p.ConsultationRequests)
            .HasForeignKey(d => d.CropCycleId)
            .HasConstraintName("consultation_requests_crop_cycle_id_fkey");

        entity.HasOne(d => d.DiseaseScan).WithMany(p => p.ConsultationRequests)
            .HasForeignKey(d => d.DiseaseScanId)
            .HasConstraintName("fk_requests_scan");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.ConsultationRequests)
            .HasForeignKey(d => d.FarmerProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("consultation_requests_farmer_profile_id_fkey");

        entity.HasOne(d => d.Land).WithMany(p => p.ConsultationRequests)
            .HasForeignKey(d => d.LandId)
            .HasConstraintName("consultation_requests_land_id_fkey");

        entity.HasOne(d => d.Service).WithMany(p => p.ConsultationRequests)
            .HasForeignKey(d => d.ServiceId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("consultation_requests_service_id_fkey");
    }
}
