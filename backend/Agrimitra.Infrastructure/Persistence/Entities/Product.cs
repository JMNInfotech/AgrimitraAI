using System;
using System.Collections.Generic;
using NpgsqlTypes;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class Product
{
    public Guid Id { get; set; }

    public string SellerType { get; set; } = null!;

    public Guid? NurseryId { get; set; }

    public Guid? ShopId { get; set; }

    public Guid? LaboratoryId { get; set; }

    public Guid? ConsultantId { get; set; }

    public Guid? NurseryBatchId { get; set; }

    public Guid? ShopProductId { get; set; }

    public Guid? LaboratoryServiceId { get; set; }

    public Guid? ConsultationServiceId { get; set; }

    public string Kind { get; set; } = null!;

    public Guid CategoryId { get; set; }

    public Guid? CropId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string Currency { get; set; } = null!;

    public string? Unit { get; set; }

    public bool IsAvailable { get; set; }

    public bool IsVerifiedSeller { get; set; }

    public decimal RatingAverage { get; set; }

    public int RatingCount { get; set; }

    public string Status { get; set; } = null!;

    public NpgsqlTsVector? SearchVector { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ICollection<AdCreative> AdCreatives { get; set; } = new List<AdCreative>();

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ProductCategory Category { get; set; } = null!;

    public virtual ConsultantProfile? Consultant { get; set; }

    public virtual ConsultationService? ConsultationService { get; set; }

    public virtual Crop? Crop { get; set; }

    public virtual LaboratoryProfile? Laboratory { get; set; }

    public virtual LaboratoryService? LaboratoryService { get; set; }

    public virtual NurseryProfile? Nursery { get; set; }

    public virtual NurseryBatch? NurseryBatch { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ProductImage? ProductImage { get; set; }

    public virtual ICollection<ProductLocation> ProductLocations { get; set; } = new List<ProductLocation>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ShopProfile? Shop { get; set; }

    public virtual ShopProduct? ShopProduct { get; set; }
}
