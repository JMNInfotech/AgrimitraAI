using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class NurseryProductVariety
{
    public Guid Id { get; set; }

    public Guid NurseryProductId { get; set; }

    public Guid? CropVarietyId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual CropVariety? CropVariety { get; set; }

    public virtual ICollection<NurseryBatch> NurseryBatches { get; set; } = new List<NurseryBatch>();

    public virtual NurseryProduct NurseryProduct { get; set; } = null!;
}
