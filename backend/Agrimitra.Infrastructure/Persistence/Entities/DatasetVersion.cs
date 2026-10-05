using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class DatasetVersion
{
    public Guid Id { get; set; }

    public Guid DatasetId { get; set; }

    public string Version { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int ImageCount { get; set; }

    public int TrainCount { get; set; }

    public int ValidationCount { get; set; }

    public int TestCount { get; set; }

    public int? SplitSeed { get; set; }

    public Guid? ManifestFileId { get; set; }

    public Guid? ApprovedBy { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<AiModelVersion> AiModelVersions { get; set; } = new List<AiModelVersion>();

    public virtual User? ApprovedByNavigation { get; set; }

    public virtual Dataset Dataset { get; set; } = null!;

    public virtual ICollection<DatasetImage> DatasetImages { get; set; } = new List<DatasetImage>();

    public virtual ICollection<DiseaseResult> DiseaseResults { get; set; } = new List<DiseaseResult>();

    public virtual FileObject? ManifestFile { get; set; }
}
