using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class State
{
    public Guid Id { get; set; }

    public Guid CountryId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string NameLocal { get; set; } = null!;

    public Point? Centroid { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<District> Districts { get; set; } = new List<District>();

    public virtual ICollection<Pincode> Pincodes { get; set; } = new List<Pincode>();
}
