using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class SoilMeasurement
{
    public Guid Id { get; set; }

    public Guid SoilTestId { get; set; }

    public Guid? SoilReportId { get; set; }

    public Guid SoilParameterId { get; set; }

    public decimal Value { get; set; }

    public string Unit { get; set; } = null!;

    public string? Rating { get; set; }

    public bool IsExtracted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual SoilParameter SoilParameter { get; set; } = null!;

    public virtual SoilReport? SoilReport { get; set; }

    public virtual SoilTest SoilTest { get; set; } = null!;
}
