using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class Address
{
    public Guid Id { get; set; }

    public Guid CountryId { get; set; }

    public Guid? StateId { get; set; }

    public Guid? DistrictId { get; set; }

    public Guid? TalukaId { get; set; }

    public Guid? VillageId { get; set; }

    public string? City { get; set; }

    public string? Line1 { get; set; }

    public string? Line2 { get; set; }

    public string? Landmark { get; set; }

    public string? Pincode { get; set; }

    public Point? Location { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ICollection<AdvertiserProfile> AdvertiserProfiles { get; set; } = new List<AdvertiserProfile>();

    public virtual ICollection<Buyer> Buyers { get; set; } = new List<Buyer>();

    public virtual ICollection<ConsultantProfile> ConsultantProfiles { get; set; } = new List<ConsultantProfile>();

    public virtual Country Country { get; set; } = null!;

    public virtual District? District { get; set; }

    public virtual ICollection<FarmerProfile> FarmerProfiles { get; set; } = new List<FarmerProfile>();

    public virtual ICollection<Farm> Farms { get; set; } = new List<Farm>();

    public virtual ICollection<LaboratoryProfile> LaboratoryProfiles { get; set; } = new List<LaboratoryProfile>();

    public virtual ICollection<Land> Lands { get; set; } = new List<Land>();

    public virtual ICollection<NurseryProfile> NurseryProfiles { get; set; } = new List<NurseryProfile>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ProductLocation> ProductLocations { get; set; } = new List<ProductLocation>();

    public virtual ICollection<ShopProfile> ShopProfiles { get; set; } = new List<ShopProfile>();

    public virtual State? State { get; set; }

    public virtual Taluka? Taluka { get; set; }

    public virtual Village? Village { get; set; }
}
