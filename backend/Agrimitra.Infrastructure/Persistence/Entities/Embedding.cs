using System;
using System.Collections.Generic;
using Pgvector;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class Embedding
{
    public Guid Id { get; set; }

    public Guid ChunkId { get; set; }

    public Guid? ModelVersionId { get; set; }

    public string EmbeddingModel { get; set; } = null!;

    public Vector Embedding1 { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual KnowledgeChunk Chunk { get; set; } = null!;

    public virtual AiModelVersion? ModelVersion { get; set; }
}
