using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class CropCycleStageHistory
{
    public Guid Id { get; set; }

    public Guid CropCycleId { get; set; }

    public Guid StageId { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? EndedAt { get; set; }

    public string Source { get; set; } = null!;

    public Guid? ChangedBy { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User? ChangedByNavigation { get; set; }

    public virtual CropCycle CropCycle { get; set; } = null!;

    public virtual CropStage Stage { get; set; } = null!;
}
