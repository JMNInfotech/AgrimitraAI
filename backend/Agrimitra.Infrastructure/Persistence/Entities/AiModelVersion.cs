using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class AiModelVersion
{
    public Guid Id { get; set; }

    public Guid ModelId { get; set; }

    public string Version { get; set; } = null!;

    public string Status { get; set; } = null!;

    public Guid? DatasetVersionId { get; set; }

    public decimal? Accuracy { get; set; }

    public decimal? PrecisionScore { get; set; }

    public decimal? RecallScore { get; set; }

    public decimal? F1Score { get; set; }

    public decimal ConfidenceThreshold { get; set; }

    public string? ArtifactUri { get; set; }

    public string? ArtifactSha256 { get; set; }

    public string Environment { get; set; } = null!;

    public DateTime? TrainedAt { get; set; }

    public DateTime? DeployedAt { get; set; }

    public Guid? ApprovedBy { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<AiRecommendation> AiRecommendations { get; set; } = new List<AiRecommendation>();

    public virtual User? ApprovedByNavigation { get; set; }

    public virtual DatasetVersion? DatasetVersion { get; set; }

    public virtual ICollection<DiseaseResult> DiseaseResults { get; set; } = new List<DiseaseResult>();

    public virtual ICollection<Embedding> Embeddings { get; set; } = new List<Embedding>();

    public virtual AiModel Model { get; set; } = null!;

    public virtual ICollection<ScheduleChangeProposal> ScheduleChangeProposals { get; set; } = new List<ScheduleChangeProposal>();
}
