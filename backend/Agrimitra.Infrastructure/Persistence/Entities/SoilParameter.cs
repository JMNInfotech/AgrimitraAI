using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class SoilParameter
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string NameLocal { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string DefaultUnit { get; set; } = null!;

    public decimal? MinPlausible { get; set; }

    public decimal? MaxPlausible { get; set; }

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<LabResultValue> LabResultValues { get; set; } = new List<LabResultValue>();

    public virtual ICollection<SoilMeasurement> SoilMeasurements { get; set; } = new List<SoilMeasurement>();

    public virtual ICollection<SoilTest> SoilTests { get; set; } = new List<SoilTest>();
}
