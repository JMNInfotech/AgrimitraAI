using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class Dataset
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Task { get; set; } = null!;

    public Guid? CropId { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual Crop? Crop { get; set; }

    public virtual ICollection<DatasetVersion> DatasetVersions { get; set; } = new List<DatasetVersion>();
}
