using System;
using System.Collections.Generic;
using NpgsqlTypes;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class KnowledgeChunk
{
    public Guid Id { get; set; }

    public Guid DocumentId { get; set; }

    public int ChunkIndex { get; set; }

    public string Content { get; set; } = null!;

    public int? TokenCount { get; set; }

    public string LanguageCode { get; set; } = null!;

    public string? Heading { get; set; }

    public int? PageNumber { get; set; }

    public NpgsqlTsVector? SearchVector { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual KnowledgeDocument Document { get; set; } = null!;

    public virtual ICollection<Embedding> Embeddings { get; set; } = new List<Embedding>();

    public virtual Language LanguageCodeNavigation { get; set; } = null!;
}
