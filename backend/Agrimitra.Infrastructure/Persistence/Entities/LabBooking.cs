using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class LabBooking
{
    public Guid Id { get; set; }

    public string BookingNumber { get; set; } = null!;

    public Guid LaboratoryId { get; set; }

    public Guid FarmerProfileId { get; set; }

    public Guid? LandId { get; set; }

    public Guid? CropId { get; set; }

    public Guid? CropCycleId { get; set; }

    public Guid ServiceId { get; set; }

    public string SampleType { get; set; } = null!;

    public string SampleHandover { get; set; } = null!;

    public DateOnly? PreferredDate { get; set; }

    public decimal Price { get; set; }

    public string Currency { get; set; } = null!;

    public Guid? PaymentId { get; set; }

    public string Status { get; set; } = null!;

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ICollection<ChatRoom> ChatRooms { get; set; } = new List<ChatRoom>();

    public virtual Crop? Crop { get; set; }

    public virtual CropCycle? CropCycle { get; set; }

    public virtual FarmerProfile FarmerProfile { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<LabBookingEvent> LabBookingEvents { get; set; } = new List<LabBookingEvent>();

    public virtual ICollection<LabReport> LabReports { get; set; } = new List<LabReport>();

    public virtual ICollection<LabSample> LabSamples { get; set; } = new List<LabSample>();

    public virtual LaboratoryProfile Laboratory { get; set; } = null!;

    public virtual Land? Land { get; set; }

    public virtual Payment? Payment { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual LaboratoryService Service { get; set; } = null!;

    public virtual ICollection<SoilTest> SoilTests { get; set; } = new List<SoilTest>();
}
