using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class InvoiceLineConfiguration : IEntityTypeConfiguration<InvoiceLine>
{
    public void Configure(EntityTypeBuilder<InvoiceLine> entity)
    {
        entity.HasKey(e => e.Id).HasName("invoice_lines_pkey");

        entity.ToTable("invoice_lines");

        entity.HasIndex(e => new { e.InvoiceId, e.LineNo }, "invoice_lines_invoice_id_line_no_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
        entity.Property(e => e.LineNo).HasColumnName("line_no");
        entity.Property(e => e.Quantity)
            .HasPrecision(12, 3)
            .HasColumnName("quantity");
        entity.Property(e => e.TaxPercent)
            .HasPrecision(5, 2)
            .HasColumnName("tax_percent");
        entity.Property(e => e.UnitPrice)
            .HasPrecision(12, 2)
            .HasColumnName("unit_price");

        entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceLines)
            .HasForeignKey(d => d.InvoiceId)
            .HasConstraintName("invoice_lines_invoice_id_fkey");
    }
}
