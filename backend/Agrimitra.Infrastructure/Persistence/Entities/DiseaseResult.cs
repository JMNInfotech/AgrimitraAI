using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class DiseaseResult
{
    public Guid Id { get; set; }

    public Guid DiseaseScanId { get; set; }

    public Guid? InferenceLogId { get; set; }

    public DateTime? InferenceLogCreatedAt { get; set; }

    public Guid? ModelVersionId { get; set; }

    public Guid? DatasetVersionId { get; set; }

    public Guid? DetectedCropId { get; set; }

    public Guid? CropDiseaseId { get; set; }

    public decimal Confidence { get; set; }

    public string? Severity { get; set; }

    public decimal? AffectedAreaPercent { get; set; }

    public bool NeedsExpertReview { get; set; }

    public Guid? ExplanationFileId { get; set; }

    public string? RecommendedAction { get; set; }

    public string? Prevention { get; set; }

    public string Disclaimer { get; set; } = null!;

    public short Rank { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<AiFeedback> AiFeedbacks { get; set; } = new List<AiFeedback>();

    public virtual ICollection<AiRecommendation> AiRecommendations { get; set; } = new List<AiRecommendation>();

    public virtual CropDisease? CropDisease { get; set; }

    public virtual DatasetVersion? DatasetVersion { get; set; }

    public virtual Crop? DetectedCrop { get; set; }

    public virtual DiseaseScan DiseaseScan { get; set; } = null!;

    public virtual FileObject? ExplanationFile { get; set; }

    public virtual AiModelVersion? ModelVersion { get; set; }
}
