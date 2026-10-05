using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class SoilTest
{
    public Guid Id { get; set; }

    public Guid SoilSampleId { get; set; }

    public Guid? LabBookingId { get; set; }

    public string Source { get; set; } = null!;

    public DateOnly? TestedOn { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual LabBooking? LabBooking { get; set; }

    public virtual ICollection<SoilMeasurement> SoilMeasurements { get; set; } = new List<SoilMeasurement>();

    public virtual ICollection<SoilReport> SoilReports { get; set; } = new List<SoilReport>();

    public virtual SoilSample SoilSample { get; set; } = null!;

    public virtual ICollection<SoilParameter> SoilParameters { get; set; } = new List<SoilParameter>();
}
