using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class NurseryBatch
{
    public Guid Id { get; set; }

    public Guid NurseryId { get; set; }

    public Guid NurseryProductId { get; set; }

    public Guid? ProductVarietyId { get; set; }

    public string BatchCode { get; set; } = null!;

    public int? PlantAgeDays { get; set; }

    public DateOnly? SownOn { get; set; }

    public DateOnly? ReadyOn { get; set; }

    public int QuantityTotal { get; set; }

    public int QuantityAvailable { get; set; }

    public decimal UnitPrice { get; set; }

    public string Currency { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual NurseryProfile Nursery { get; set; } = null!;

    public virtual NurseryInventory? NurseryInventory { get; set; }

    public virtual NurseryProduct NurseryProduct { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Product? Product { get; set; }

    public virtual NurseryProductVariety? ProductVariety { get; set; }
}
