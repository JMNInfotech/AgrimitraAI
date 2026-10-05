using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class CropDisease
{
    public Guid Id { get; set; }

    public Guid? CropId { get; set; }

    public string Code { get; set; } = null!;

    public string Kind { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string NameLocal { get; set; } = null!;

    public string? ScientificName { get; set; }

    public string? Description { get; set; }

    public string? Symptoms { get; set; }

    public string? Prevention { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<AiFeedback> AiFeedbacks { get; set; } = new List<AiFeedback>();

    public virtual ICollection<ConsultantExpertise> ConsultantExpertises { get; set; } = new List<ConsultantExpertise>();

    public virtual Crop? Crop { get; set; }

    public virtual ICollection<CropDiseaseHistory> CropDiseaseHistories { get; set; } = new List<CropDiseaseHistory>();

    public virtual ICollection<DatasetImage> DatasetImages { get; set; } = new List<DatasetImage>();

    public virtual ICollection<DiseaseResult> DiseaseResults { get; set; } = new List<DiseaseResult>();
}
