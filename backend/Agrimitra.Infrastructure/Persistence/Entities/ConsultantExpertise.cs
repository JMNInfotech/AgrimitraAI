using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ConsultantExpertise
{
    public Guid Id { get; set; }

    public Guid ConsultantId { get; set; }

    public Guid? CropId { get; set; }

    public Guid? CropDiseaseId { get; set; }

    public string? Topic { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ConsultantProfile Consultant { get; set; } = null!;

    public virtual Crop? Crop { get; set; }

    public virtual CropDisease? CropDisease { get; set; }
}
