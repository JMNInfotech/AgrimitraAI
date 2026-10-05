using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class AdTargeting
{
    public Guid Id { get; set; }

    public Guid CampaignId { get; set; }

    public List<Guid> StateIds { get; set; } = null!;

    public List<Guid> DistrictIds { get; set; } = null!;

    public List<Guid> TalukaIds { get; set; } = null!;

    public Point? CenterLocation { get; set; }

    public decimal? RadiusKm { get; set; }

    public List<Guid> CropIds { get; set; } = null!;

    public List<string> CropStageCodes { get; set; } = null!;

    public List<string> FarmerSegments { get; set; } = null!;

    public List<string> LanguageCodes { get; set; } = null!;

    public List<string> Seasons { get; set; } = null!;

    public decimal? FarmSizeMinSqM { get; set; }

    public decimal? FarmSizeMaxSqM { get; set; }

    public List<Guid> InterestCategoryIds { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual AdCampaign Campaign { get; set; } = null!;
}
