using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ProductionRecord
{
    public Guid Id { get; set; }

    public Guid FarmerProfileId { get; set; }

    public Guid LandId { get; set; }

    public Guid CropId { get; set; }

    public Guid? CropCycleId { get; set; }

    public Guid? HarvestRecordId { get; set; }

    public DateOnly ProductionDate { get; set; }

    public decimal Quantity { get; set; }

    public string Unit { get; set; } = null!;

    public string? QualityGrade { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual Crop Crop { get; set; } = null!;

    public virtual CropCycle? CropCycle { get; set; }

    public virtual FarmerProfile FarmerProfile { get; set; } = null!;

    public virtual HarvestRecord? HarvestRecord { get; set; }

    public virtual ICollection<IncomeRecord> IncomeRecords { get; set; } = new List<IncomeRecord>();

    public virtual Land Land { get; set; } = null!;
}
