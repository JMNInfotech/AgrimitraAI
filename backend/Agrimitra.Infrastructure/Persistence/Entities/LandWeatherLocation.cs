using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class LandWeatherLocation
{
    public Guid LandId { get; set; }

    public Guid WeatherLocationId { get; set; }

    public decimal? DistanceMeters { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual Land Land { get; set; } = null!;

    public virtual WeatherLocation WeatherLocation { get; set; } = null!;
}
