using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class NotificationTemplate
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string Channel { get; set; } = null!;

    public string LanguageCode { get; set; } = null!;

    public string? Subject { get; set; }

    public string Body { get; set; } = null!;

    public string? ProviderTemplateId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual Language LanguageCodeNavigation { get; set; } = null!;
}
