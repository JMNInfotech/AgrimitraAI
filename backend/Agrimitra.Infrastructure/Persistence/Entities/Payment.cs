using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class Payment
{
    public Guid Id { get; set; }

    public string PaymentNumber { get; set; } = null!;

    public Guid PayerUserId { get; set; }

    public string Purpose { get; set; } = null!;

    public Guid? ConsultationId { get; set; }

    public Guid? LabBookingId { get; set; }

    public Guid? OrderId { get; set; }

    public Guid? AdBillingId { get; set; }

    public decimal Amount { get; set; }

    public decimal RefundedAmount { get; set; }

    public string Currency { get; set; } = null!;

    public string Gateway { get; set; } = null!;

    public string? GatewayOrderId { get; set; }

    public string? Method { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? PaidAt { get; set; }

    public DateTime? VerifiedAt { get; set; }

    public string? FailureReason { get; set; }

    public string? IdempotencyKey { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual AdBilling? AdBilling { get; set; }

    public virtual Consultation? Consultation { get; set; }

    public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual LabBooking? LabBooking { get; set; }

    public virtual ICollection<LabBooking> LabBookings { get; set; } = new List<LabBooking>();

    public virtual Order? Order { get; set; }

    public virtual User PayerUser { get; set; } = null!;

    public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();

    public virtual ICollection<Refund> Refunds { get; set; } = new List<Refund>();
}
