using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class Language
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string NativeName { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsRtl { get; set; }

    public int SortOrder { get; set; }

    public virtual ICollection<AdCreative> AdCreatives { get; set; } = new List<AdCreative>();

    public virtual ICollection<AiRecommendation> AiRecommendations { get; set; } = new List<AiRecommendation>();

    public virtual ICollection<KnowledgeChunk> KnowledgeChunks { get; set; } = new List<KnowledgeChunk>();

    public virtual ICollection<KnowledgeDocument> KnowledgeDocuments { get; set; } = new List<KnowledgeDocument>();

    public virtual ICollection<NotificationTemplate> NotificationTemplates { get; set; } = new List<NotificationTemplate>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<SoilReport> SoilReports { get; set; } = new List<SoilReport>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<ConsultantProfile> Consultants { get; set; } = new List<ConsultantProfile>();
}
