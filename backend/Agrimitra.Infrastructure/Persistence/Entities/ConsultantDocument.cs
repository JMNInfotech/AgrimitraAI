using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ConsultantDocument
{
    public Guid Id { get; set; }

    public Guid ConsultantId { get; set; }

    public string DocumentType { get; set; } = null!;

    public string? Title { get; set; }

    public Guid FileObjectId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ConsultantProfile Consultant { get; set; } = null!;

    public virtual FileObject FileObject { get; set; } = null!;
}
