using System;
using System.Collections.Generic;
using System.Net;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class OtpVerification
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public string Purpose { get; set; } = null!;

    public string Channel { get; set; } = null!;

    public string Target { get; set; } = null!;

    public string CodeHash { get; set; } = null!;

    public int Attempts { get; set; }

    public int MaxAttempts { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime? VerifiedAt { get; set; }

    public DateTime? ConsumedAt { get; set; }

    public IPAddress? IpAddress { get; set; }

    public virtual User? User { get; set; }
}
