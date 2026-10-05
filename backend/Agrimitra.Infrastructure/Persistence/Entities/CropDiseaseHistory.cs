using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class CropDiseaseHistory
{
    public Guid Id { get; set; }

    public Guid CropCycleId { get; set; }

    public Guid CropDiseaseId { get; set; }

    public DateOnly DetectedOn { get; set; }

    public string? Severity { get; set; }

    public string Source { get; set; } = null!;

    public Guid? DiseaseScanId { get; set; }

    public Guid? ConsultationId { get; set; }

    public bool IsConfirmed { get; set; }

    public DateOnly? ResolvedOn { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual Consultation? Consultation { get; set; }

    public virtual CropCycle CropCycle { get; set; } = null!;

    public virtual CropDisease CropDisease { get; set; } = null!;

    public virtual DiseaseScan? DiseaseScan { get; set; }
}
