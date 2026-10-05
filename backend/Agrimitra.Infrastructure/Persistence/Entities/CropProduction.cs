using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class CropProduction
{
    public Guid Id { get; set; }

    public Guid CropCycleId { get; set; }

    public decimal? ExpectedQuantity { get; set; }

    public decimal? ActualQuantity { get; set; }

    public string Unit { get; set; } = null!;

    public string? QualityGrade { get; set; }

    public decimal? MarketPricePerUnit { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual CropCycle CropCycle { get; set; } = null!;
}
