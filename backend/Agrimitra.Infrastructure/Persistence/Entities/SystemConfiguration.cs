using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class SystemConfiguration
{
    public Guid Id { get; set; }

    public string ConfigKey { get; set; } = null!;

    public string Value { get; set; } = null!;

    public string ValueType { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsSensitive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }
}
