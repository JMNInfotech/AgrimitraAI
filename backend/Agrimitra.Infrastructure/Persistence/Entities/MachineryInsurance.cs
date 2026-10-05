using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class MachineryInsurance
{
    public Guid Id { get; set; }

    public Guid MachineryId { get; set; }

    public string Provider { get; set; } = null!;

    public string PolicyNumber { get; set; } = null!;

    public decimal? Premium { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public Guid? DocumentFileId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual FileObject? DocumentFile { get; set; }

    public virtual Machinery Machinery { get; set; } = null!;
}
