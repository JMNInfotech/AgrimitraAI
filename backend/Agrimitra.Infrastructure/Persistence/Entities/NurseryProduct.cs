using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class NurseryProduct
{
    public Guid Id { get; set; }

    public Guid NurseryId { get; set; }

    public Guid CropId { get; set; }

    public Guid? CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Unit { get; set; } = null!;

    public bool IsAvailable { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ProductCategory? Category { get; set; }

    public virtual Crop Crop { get; set; } = null!;

    public virtual NurseryProfile Nursery { get; set; } = null!;

    public virtual ICollection<NurseryBatch> NurseryBatches { get; set; } = new List<NurseryBatch>();

    public virtual ICollection<NurseryProductVariety> NurseryProductVarieties { get; set; } = new List<NurseryProductVariety>();
}
