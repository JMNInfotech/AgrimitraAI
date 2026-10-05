using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class LaboratoryService
{
    public Guid Id { get; set; }

    public Guid LaboratoryId { get; set; }

    public Guid TestTypeId { get; set; }

    public string? Name { get; set; }

    public decimal Price { get; set; }

    public string Currency { get; set; } = null!;

    public int TurnaroundDays { get; set; }

    public bool SampleCollectionAvailable { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ICollection<LabBooking> LabBookings { get; set; } = new List<LabBooking>();

    public virtual LaboratoryProfile Laboratory { get; set; } = null!;

    public virtual Product? Product { get; set; }

    public virtual LabTestType TestType { get; set; } = null!;
}
