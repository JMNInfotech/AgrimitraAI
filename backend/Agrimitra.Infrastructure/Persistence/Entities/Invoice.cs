using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class Invoice
{
    public Guid Id { get; set; }

    public string InvoiceNumber { get; set; } = null!;

    public string InvoiceType { get; set; } = null!;

    public Guid? OrderId { get; set; }

    public Guid? LabBookingId { get; set; }

    public Guid? ConsultationId { get; set; }

    public Guid? AdBillingId { get; set; }

    public Guid? PaymentId { get; set; }

    public Guid BillToUserId { get; set; }

    public string IssuerName { get; set; } = null!;

    public string? IssuerGstin { get; set; }

    public decimal Subtotal { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal? TotalAmount { get; set; }

    public string Currency { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime IssuedAt { get; set; }

    public Guid? PdfFileObjectId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual AdBilling? AdBilling { get; set; }

    public virtual User BillToUser { get; set; } = null!;

    public virtual Consultation? Consultation { get; set; }

    public virtual ICollection<InvoiceLine> InvoiceLines { get; set; } = new List<InvoiceLine>();

    public virtual LabBooking? LabBooking { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Payment? Payment { get; set; }

    public virtual FileObject? PdfFileObject { get; set; }
}
