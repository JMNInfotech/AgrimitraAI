using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class DiseaseScanImage
{
    public Guid Id { get; set; }

    public Guid DiseaseScanId { get; set; }

    public Guid FileObjectId { get; set; }

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual DiseaseScan DiseaseScan { get; set; } = null!;

    public virtual FileObject FileObject { get; set; } = null!;
}
