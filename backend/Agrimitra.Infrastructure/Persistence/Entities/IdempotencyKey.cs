using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class IdempotencyKey
{
    public string Key { get; set; } = null!;

    public Guid UserId { get; set; }

    public string RequestHash { get; set; } = null!;

    public int? ResponseStatus { get; set; }

    public string? ResponseBody { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }
}
