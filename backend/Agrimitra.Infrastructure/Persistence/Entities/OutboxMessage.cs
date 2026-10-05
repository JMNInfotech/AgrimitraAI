using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class OutboxMessage
{
    public Guid Id { get; set; }

    public string EventType { get; set; } = null!;

    public string? AggregateType { get; set; }

    public Guid? AggregateId { get; set; }

    public string Payload { get; set; } = null!;

    public string Headers { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime AvailableAt { get; set; }

    public DateTime? ProcessedAt { get; set; }

    public int Attempts { get; set; }

    public string? LastError { get; set; }
}
