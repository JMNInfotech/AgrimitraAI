using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class OrderItem
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public Guid? NurseryBatchId { get; set; }

    public Guid? ProductBatchId { get; set; }

    public string TitleSnapshot { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TaxPercent { get; set; }

    public decimal? LineTotal { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual NurseryBatch? NurseryBatch { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual ProductBatch? ProductBatch { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
