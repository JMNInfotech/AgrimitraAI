using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class CropActivityEvidence
{
    public Guid Id { get; set; }

    public Guid? CompletionId { get; set; }

    public Guid OccurrenceId { get; set; }

    public Guid FileObjectId { get; set; }

    public string? Caption { get; set; }

    public DateTime? CapturedAt { get; set; }

    public Point? Location { get; set; }

    public Guid UploadedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual CropActivityCompletion? Completion { get; set; }

    public virtual FileObject FileObject { get; set; } = null!;

    public virtual CropActivityOccurrence Occurrence { get; set; } = null!;

    public virtual User UploadedByNavigation { get; set; } = null!;
}
