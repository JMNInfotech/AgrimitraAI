using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class KnowledgeDocument
{
    public Guid Id { get; set; }

    public Guid? CategoryId { get; set; }

    public Guid? CropId { get; set; }

    public string Title { get; set; } = null!;

    public string SourceType { get; set; } = null!;

    public string? SourceReference { get; set; }

    public string LanguageCode { get; set; } = null!;

    public Guid? FileObjectId { get; set; }

    public string? ContentHash { get; set; }

    public int Version { get; set; }

    public string Status { get; set; } = null!;

    public string IngestionStatus { get; set; } = null!;

    public Guid? AuthorUserId { get; set; }

    public Guid? ApprovedBy { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual User? ApprovedByNavigation { get; set; }

    public virtual User? AuthorUser { get; set; }

    public virtual KnowledgeCategory? Category { get; set; }

    public virtual Crop? Crop { get; set; }

    public virtual FileObject? FileObject { get; set; }

    public virtual ICollection<KnowledgeChunk> KnowledgeChunks { get; set; } = new List<KnowledgeChunk>();

    public virtual Language LanguageCodeNavigation { get; set; } = null!;
}
