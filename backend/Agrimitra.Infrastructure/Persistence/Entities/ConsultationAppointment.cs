using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ConsultationAppointment
{
    public Guid Id { get; set; }

    public Guid ConsultationId { get; set; }

    public DateTime StartAt { get; set; }

    public DateTime EndAt { get; set; }

    public string Mode { get; set; } = null!;

    public Point? Location { get; set; }

    public string? MeetingUrl { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? ReminderSentAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual Consultation Consultation { get; set; } = null!;
}
