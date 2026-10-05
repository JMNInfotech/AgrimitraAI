using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ShopProfile
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string ShopName { get; set; } = null!;

    public string OwnerName { get; set; } = null!;

    public string? MobileNumber { get; set; }

    public string? Email { get; set; }

    public Guid? AddressId { get; set; }

    public Point? Location { get; set; }

    public string? LicenseNumber { get; set; }

    public string? LicenseType { get; set; }

    public DateOnly? LicenseValidUntil { get; set; }

    public string? Gstin { get; set; }

    public Guid? ShopImageId { get; set; }

    public string VerificationStatus { get; set; } = null!;

    public DateTime? VerifiedAt { get; set; }

    public decimal RatingAverage { get; set; }

    public int RatingCount { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual FileObject? ShopImage { get; set; }

    public virtual ICollection<ShopProduct> ShopProducts { get; set; } = new List<ShopProduct>();

    public virtual User User { get; set; } = null!;
}
