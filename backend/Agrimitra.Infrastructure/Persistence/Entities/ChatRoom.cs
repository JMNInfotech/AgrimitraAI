using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ChatRoom
{
    public Guid Id { get; set; }

    public string RoomType { get; set; } = null!;

    public Guid? ConsultationId { get; set; }

    public Guid? OrderId { get; set; }

    public Guid? LabBookingId { get; set; }

    public Guid? TicketId { get; set; }

    public string? Title { get; set; }

    public bool IsArchived { get; set; }

    public DateTime? LastMessageAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<ChatParticipant> ChatParticipants { get; set; } = new List<ChatParticipant>();

    public virtual Consultation? Consultation { get; set; }

    public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

    public virtual LabBooking? LabBooking { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual Order? Order { get; set; }

    public virtual SupportTicket? Ticket { get; set; }
}
