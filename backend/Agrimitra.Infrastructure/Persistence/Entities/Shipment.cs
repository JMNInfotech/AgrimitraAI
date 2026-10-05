using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class Shipment
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public string? Carrier { get; set; }

    public string? TrackingNumber { get; set; }

    public string? TrackingUrl { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? ShippedAt { get; set; }

    public DateOnly? EstimatedDeliveryOn { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual Delivery? Delivery { get; set; }

    public virtual Order Order { get; set; } = null!;
}
