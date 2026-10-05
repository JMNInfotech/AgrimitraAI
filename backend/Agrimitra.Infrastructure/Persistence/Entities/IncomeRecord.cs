using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class IncomeRecord
{
    public Guid Id { get; set; }

    public Guid FarmerProfileId { get; set; }

    public Guid? LandId { get; set; }

    public Guid? CropId { get; set; }

    public Guid? CropCycleId { get; set; }

    public Guid? ProductionRecordId { get; set; }

    public Guid? BuyerId { get; set; }

    public string? ProduceName { get; set; }

    public decimal Quantity { get; set; }

    public string Unit { get; set; } = null!;

    public decimal SellingPricePerUnit { get; set; }

    public decimal? Revenue { get; set; }

    public decimal OtherCharges { get; set; }

    public string Currency { get; set; } = null!;

    public DateOnly IncomeDate { get; set; }

    public string? Notes { get; set; }

    public Guid? ClientMutationId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual Buyer? Buyer { get; set; }

    public virtual Crop? Crop { get; set; }

    public virtual CropCycle? CropCycle { get; set; }

    public virtual FarmerProfile FarmerProfile { get; set; } = null!;

    public virtual Land? Land { get; set; }

    public virtual ProductionRecord? ProductionRecord { get; set; }
}
