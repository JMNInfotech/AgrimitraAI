using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ProductLocation
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Guid? AddressId { get; set; }

    public Point Location { get; set; } = null!;

    public decimal? ServiceRadiusKm { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual Address? Address { get; set; }

    public virtual Product Product { get; set; } = null!;
}
