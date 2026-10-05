using System;
using System.Collections.Generic;
using System.Net;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ConsentRecord
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string ConsentType { get; set; } = null!;

    public bool IsGranted { get; set; }

    public string PolicyVersion { get; set; } = null!;

    public string? Source { get; set; }

    public IPAddress? IpAddress { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
