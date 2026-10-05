using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class DatasetImage
{
    public Guid Id { get; set; }

    public Guid DatasetVersionId { get; set; }

    public Guid FileObjectId { get; set; }

    public Guid? CropId { get; set; }

    public Guid? CropDiseaseId { get; set; }

    public string? Label { get; set; }

    public string Source { get; set; } = null!;

    public string? Split { get; set; }

    public string? Annotation { get; set; }

    public Guid? LabeledBy { get; set; }

    public Guid? ReviewedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual Crop? Crop { get; set; }

    public virtual CropDisease? CropDisease { get; set; }

    public virtual DatasetVersion DatasetVersion { get; set; } = null!;

    public virtual FileObject FileObject { get; set; } = null!;

    public virtual User? LabeledByNavigation { get; set; }

    public virtual User? ReviewedByNavigation { get; set; }
}
