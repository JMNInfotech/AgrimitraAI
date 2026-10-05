using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class LandDocument
{
    public Guid Id { get; set; }

    public Guid LandId { get; set; }

    public string DocumentType { get; set; } = null!;

    public string? Title { get; set; }

    public Guid FileObjectId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual FileObject FileObject { get; set; } = null!;

    public virtual Land Land { get; set; } = null!;
}
