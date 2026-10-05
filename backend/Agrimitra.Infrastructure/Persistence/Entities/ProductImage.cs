using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ProductImage
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Guid FileObjectId { get; set; }

    public int SortOrder { get; set; }

    public bool IsPrimary { get; set; }

    public string? AltText { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual FileObject FileObject { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
