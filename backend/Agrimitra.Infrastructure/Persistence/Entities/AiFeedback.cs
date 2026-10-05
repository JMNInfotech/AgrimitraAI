using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class AiFeedback
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid? DiseaseResultId { get; set; }

    public Guid? RecommendationId { get; set; }

    public Guid? InferenceLogId { get; set; }

    public string FeedbackType { get; set; } = null!;

    public bool? IsHelpful { get; set; }

    public Guid? CorrectedCropDiseaseId { get; set; }

    public string? Comment { get; set; }

    public string CandidateStatus { get; set; } = null!;

    public Guid? CandidateReviewedBy { get; set; }

    public DateTime? CandidateReviewedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual User? CandidateReviewedByNavigation { get; set; }

    public virtual CropDisease? CorrectedCropDisease { get; set; }

    public virtual DiseaseResult? DiseaseResult { get; set; }

    public virtual AiRecommendation? Recommendation { get; set; }

    public virtual User User { get; set; } = null!;
}
