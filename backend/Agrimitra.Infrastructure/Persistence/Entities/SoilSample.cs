using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class SoilSample
{
    public Guid Id { get; set; }

    public Guid FarmerProfileId { get; set; }

    public Guid LandId { get; set; }

    public Guid? CropCycleId { get; set; }

    public string SampleCode { get; set; } = null!;

    public DateOnly CollectedOn { get; set; }

    public decimal? DepthCm { get; set; }

    public Point? Location { get; set; }

    public string? CollectedBy { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual CropCycle? CropCycle { get; set; }

    public virtual FarmerProfile FarmerProfile { get; set; } = null!;

    public virtual ICollection<LabSample> LabSamples { get; set; } = new List<LabSample>();

    public virtual Land Land { get; set; } = null!;

    public virtual ICollection<SoilTest> SoilTests { get; set; } = new List<SoilTest>();
}
