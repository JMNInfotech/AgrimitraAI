using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class MachineryMaintenance
{
    public Guid Id { get; set; }

    public Guid MachineryId { get; set; }

    public DateOnly MaintenanceDate { get; set; }

    public string Description { get; set; } = null!;

    public decimal Cost { get; set; }

    public string? Vendor { get; set; }

    public Guid? ReceiptFileId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual Machinery Machinery { get; set; } = null!;

    public virtual FileObject? ReceiptFile { get; set; }
}
