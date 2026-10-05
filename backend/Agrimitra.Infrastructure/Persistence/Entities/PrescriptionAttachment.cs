using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class PrescriptionAttachment
{
    public Guid Id { get; set; }

    public Guid PrescriptionId { get; set; }

    public Guid FileObjectId { get; set; }

    public string? Caption { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual FileObject FileObject { get; set; } = null!;

    public virtual Prescription Prescription { get; set; } = null!;
}
