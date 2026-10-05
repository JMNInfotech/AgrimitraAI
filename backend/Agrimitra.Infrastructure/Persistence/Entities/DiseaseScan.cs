using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class DiseaseScan
{
    public Guid Id { get; set; }

    public Guid FarmerProfileId { get; set; }

    public Guid UserId { get; set; }

    public Guid? LandId { get; set; }

    public Guid? CropId { get; set; }

    public Guid? CropCycleId { get; set; }

    public Point? Location { get; set; }

    public DateTime CapturedAt { get; set; }

    public string Status { get; set; } = null!;

    public Guid? ClientMutationId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ICollection<ConsultationRequest> ConsultationRequests { get; set; } = new List<ConsultationRequest>();

    public virtual Crop? Crop { get; set; }

    public virtual CropCycle? CropCycle { get; set; }

    public virtual ICollection<CropDiseaseHistory> CropDiseaseHistories { get; set; } = new List<CropDiseaseHistory>();

    public virtual ICollection<DiseaseResult> DiseaseResults { get; set; } = new List<DiseaseResult>();

    public virtual ICollection<DiseaseScanImage> DiseaseScanImages { get; set; } = new List<DiseaseScanImage>();

    public virtual FarmerProfile FarmerProfile { get; set; } = null!;

    public virtual Land? Land { get; set; }

    public virtual User User { get; set; } = null!;
}
