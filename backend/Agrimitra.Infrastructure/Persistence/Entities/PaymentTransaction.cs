using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class PaymentTransaction
{
    public Guid Id { get; set; }

    public Guid PaymentId { get; set; }

    public string Kind { get; set; } = null!;

    public string? GatewayTransactionId { get; set; }

    public decimal Amount { get; set; }

    public string Status { get; set; } = null!;

    public bool? SignatureVerified { get; set; }

    public string? RawResponse { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Payment Payment { get; set; } = null!;
}
