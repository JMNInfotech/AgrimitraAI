using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class LandBoundary
{
    public Guid Id { get; set; }

    public Guid LandId { get; set; }

    public Polygon Boundary { get; set; } = null!;

    public decimal? AreaSqMeters { get; set; }

    public string Source { get; set; } = null!;

    public bool IsCurrent { get; set; }

    public int Version { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual Land Land { get; set; } = null!;
}
