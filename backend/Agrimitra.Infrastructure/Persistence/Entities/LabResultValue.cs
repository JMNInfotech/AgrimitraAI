using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class LabResultValue
{
    public Guid Id { get; set; }

    public Guid LabResultId { get; set; }

    public Guid? SoilParameterId { get; set; }

    public string ParameterName { get; set; } = null!;

    public decimal? ValueNumeric { get; set; }

    public string? ValueText { get; set; }

    public string? Unit { get; set; }

    public string? ReferenceRange { get; set; }

    public string? Flag { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual LabResult LabResult { get; set; } = null!;

    public virtual SoilParameter? SoilParameter { get; set; }
}
