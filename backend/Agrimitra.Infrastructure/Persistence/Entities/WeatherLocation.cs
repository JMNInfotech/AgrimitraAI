using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class WeatherLocation
{
    public Guid Id { get; set; }

    public Guid? LandId { get; set; }

    public Guid? VillageId { get; set; }

    public string? Name { get; set; }

    public Point Location { get; set; } = null!;

    public string GridKey { get; set; } = null!;

    public string Provider { get; set; } = null!;

    public string? ProviderLocationId { get; set; }

    public string TimeZoneId { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime? LastFetchedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual Land? Land { get; set; }

    public virtual ICollection<LandWeatherLocation> LandWeatherLocations { get; set; } = new List<LandWeatherLocation>();

    public virtual Village? Village { get; set; }

    public virtual ICollection<WeatherAlert> WeatherAlerts { get; set; } = new List<WeatherAlert>();
}
