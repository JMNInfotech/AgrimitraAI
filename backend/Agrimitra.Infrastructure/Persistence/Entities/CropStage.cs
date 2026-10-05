using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class CropStage
{
    public Guid Id { get; set; }

    public Guid CropId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string NameLocal { get; set; } = null!;

    public int Sequence { get; set; }

    public int? TypicalDurationDays { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual Crop Crop { get; set; } = null!;

    public virtual ICollection<CropCycleStageHistory> CropCycleStageHistories { get; set; } = new List<CropCycleStageHistory>();

    public virtual ICollection<CropCycle> CropCycles { get; set; } = new List<CropCycle>();
}
