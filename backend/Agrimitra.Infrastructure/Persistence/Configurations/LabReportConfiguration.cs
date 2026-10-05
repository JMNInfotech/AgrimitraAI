using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class LabReportConfiguration : IEntityTypeConfiguration<LabReport>
{
    public void Configure(EntityTypeBuilder<LabReport> entity)
    {
        entity.HasKey(e => e.Id).HasName("lab_reports_pkey");

        entity.ToTable("lab_reports");

        entity.HasIndex(e => e.BookingId, "ix_lab_reports_booking");

        entity.HasIndex(e => e.IssuedBy, "ix_lab_reports_issued_by_101f72");

        entity.HasIndex(e => new { e.LaboratoryId, e.IssuedAt }, "ix_lab_reports_lab").IsDescending(false, true);

        entity.HasIndex(e => e.PdfFileObjectId, "ix_lab_reports_pdf_file_object_id_7308fe");

        entity.HasIndex(e => e.SoilReportId, "ix_lab_reports_soil");

        entity.HasIndex(e => e.ReportNumber, "lab_reports_report_number_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.BookingId).HasColumnName("booking_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.IssuedAt).HasColumnName("issued_at");
        entity.Property(e => e.IssuedBy).HasColumnName("issued_by");
        entity.Property(e => e.LaboratoryId).HasColumnName("laboratory_id");
        entity.Property(e => e.PdfFileObjectId).HasColumnName("pdf_file_object_id");
        entity.Property(e => e.ReportNumber).HasColumnName("report_number");
        entity.Property(e => e.SoilReportId).HasColumnName("soil_report_id");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'draft'::text")
            .HasColumnName("status");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Booking).WithMany(p => p.LabReports)
            .HasForeignKey(d => d.BookingId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("lab_reports_booking_id_fkey");

        entity.HasOne(d => d.IssuedByNavigation).WithMany(p => p.LabReports)
            .HasForeignKey(d => d.IssuedBy)
            .HasConstraintName("lab_reports_issued_by_fkey");

        entity.HasOne(d => d.Laboratory).WithMany(p => p.LabReports)
            .HasForeignKey(d => d.LaboratoryId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("lab_reports_laboratory_id_fkey");

        entity.HasOne(d => d.PdfFileObject).WithMany(p => p.LabReports)
            .HasForeignKey(d => d.PdfFileObjectId)
            .HasConstraintName("lab_reports_pdf_file_object_id_fkey");

        entity.HasOne(d => d.SoilReport).WithMany(p => p.LabReports)
            .HasForeignKey(d => d.SoilReportId)
            .HasConstraintName("lab_reports_soil_report_id_fkey");
    }
}
