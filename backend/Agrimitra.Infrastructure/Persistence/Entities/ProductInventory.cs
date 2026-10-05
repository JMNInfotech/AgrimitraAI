using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ProductInventory
{
    public Guid ShopProductId { get; set; }

    public int QuantityOnHand { get; set; }

    public int QuantityReserved { get; set; }

    public int? LowStockThreshold { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ShopProduct ShopProduct { get; set; } = null!;
}
