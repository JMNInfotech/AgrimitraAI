using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class Delivery
{
    public Guid Id { get; set; }

    public Guid ShipmentId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? DeliveredAt { get; set; }

    public string? ReceivedBy { get; set; }

    public Guid? ProofFileId { get; set; }

    public string? FailureReason { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual FileObject? ProofFile { get; set; }

    public virtual Shipment Shipment { get; set; } = null!;
}
