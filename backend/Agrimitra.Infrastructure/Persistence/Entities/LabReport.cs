using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class LabReport
{
    public Guid Id { get; set; }

    public string ReportNumber { get; set; } = null!;

    public Guid BookingId { get; set; }

    public Guid LaboratoryId { get; set; }

    public Guid? PdfFileObjectId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? IssuedAt { get; set; }

    public Guid? IssuedBy { get; set; }

    public Guid? SoilReportId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual LabBooking Booking { get; set; } = null!;

    public virtual User? IssuedByNavigation { get; set; }

    public virtual LaboratoryProfile Laboratory { get; set; } = null!;

    public virtual FileObject? PdfFileObject { get; set; }

    public virtual SoilReport? SoilReport { get; set; }
}
