using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ProductBatch
{
    public Guid Id { get; set; }

    public Guid ShopProductId { get; set; }

    public string BatchNumber { get; set; } = null!;

    public DateOnly? ManufacturedOn { get; set; }

    public DateOnly ExpiryDate { get; set; }

    public int QuantityReceived { get; set; }

    public int QuantityAvailable { get; set; }

    public decimal? CostPrice { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ShopProduct ShopProduct { get; set; } = null!;
}
