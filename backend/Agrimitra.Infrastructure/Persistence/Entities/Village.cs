using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class Village
{
    public Guid Id { get; set; }

    public Guid TalukaId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string NameLocal { get; set; } = null!;

    public string? Pincode { get; set; }

    public Point? Centroid { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual Taluka Taluka { get; set; } = null!;

    public virtual ICollection<WeatherLocation> WeatherLocations { get; set; } = new List<WeatherLocation>();
}
