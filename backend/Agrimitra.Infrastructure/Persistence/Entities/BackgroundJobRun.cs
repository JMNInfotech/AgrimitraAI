using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class BackgroundJobRun
{
    public Guid Id { get; set; }

    public string JobName { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime StartedAt { get; set; }

    public DateTime? FinishedAt { get; set; }

    public int? ProcessedCount { get; set; }

    public string? Error { get; set; }
}
