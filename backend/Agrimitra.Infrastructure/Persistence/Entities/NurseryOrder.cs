using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class NurseryOrder
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid NurseryId { get; set; }

    public DateOnly? ReadyOn { get; set; }

    public DateTime? PickupSlotStart { get; set; }

    public DateTime? PickupSlotEnd { get; set; }

    public string Status { get; set; } = null!;

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual NurseryProfile Nursery { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
