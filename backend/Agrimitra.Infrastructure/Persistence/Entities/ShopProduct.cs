using System;
using System.Collections.Generic;
using NpgsqlTypes;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ShopProduct
{
    public Guid Id { get; set; }

    public Guid ShopId { get; set; }

    public Guid CategoryId { get; set; }

    public Guid? ManufacturerId { get; set; }

    public string Name { get; set; } = null!;

    public string? Brand { get; set; }

    public string? ActiveIngredient { get; set; }

    public string? Formulation { get; set; }

    public string? RegistrationNumber { get; set; }

    public decimal PackSize { get; set; }

    public string PackUnit { get; set; } = null!;

    public decimal? Mrp { get; set; }

    public decimal Price { get; set; }

    public string Currency { get; set; } = null!;

    public decimal TaxPercent { get; set; }

    public string? Description { get; set; }

    public Guid? ImageFileId { get; set; }

    public bool IsRestricted { get; set; }

    public bool IsAvailable { get; set; }

    public NpgsqlTsVector? SearchVector { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ProductCategory Category { get; set; } = null!;

    public virtual FileObject? ImageFile { get; set; }

    public virtual Manufacturer? Manufacturer { get; set; }

    public virtual Product? Product { get; set; }

    public virtual ICollection<ProductBatch> ProductBatches { get; set; } = new List<ProductBatch>();

    public virtual ProductInventory? ProductInventory { get; set; }

    public virtual ShopProfile Shop { get; set; } = null!;
}
