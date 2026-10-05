using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class LabSample
{
    public Guid Id { get; set; }

    public Guid BookingId { get; set; }

    public string SampleCode { get; set; } = null!;

    public Guid LaboratoryId { get; set; }

    public Guid? SoilSampleId { get; set; }

    public string SampleType { get; set; } = null!;

    public DateTime? ReceivedAt { get; set; }

    public Guid? ReceivedBy { get; set; }

    public string? Condition { get; set; }

    public string? StorageLocation { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual LabBooking Booking { get; set; } = null!;

    public virtual ICollection<LabResult> LabResults { get; set; } = new List<LabResult>();

    public virtual LaboratoryProfile Laboratory { get; set; } = null!;

    public virtual User? ReceivedByNavigation { get; set; }

    public virtual SoilSample? SoilSample { get; set; }
}
