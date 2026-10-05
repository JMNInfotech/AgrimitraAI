using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class HarvestRecord
{
    public Guid Id { get; set; }

    public Guid CropCycleId { get; set; }

    public Guid LandId { get; set; }

    public DateOnly HarvestDate { get; set; }

    public decimal Quantity { get; set; }

    public string Unit { get; set; } = null!;

    public string? QualityGrade { get; set; }

    public int? LabourCount { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual CropCycle CropCycle { get; set; } = null!;

    public virtual Land Land { get; set; } = null!;

    public virtual ICollection<ProductionRecord> ProductionRecords { get; set; } = new List<ProductionRecord>();
}
