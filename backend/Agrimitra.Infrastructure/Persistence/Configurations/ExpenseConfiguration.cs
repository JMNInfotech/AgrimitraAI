using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> entity)
    {
        entity.HasKey(e => e.Id).HasName("expenses_pkey");

        entity.ToTable("expenses");

        entity.HasIndex(e => e.ClientMutationId, "expenses_client_mutation_id_key").IsUnique();

        entity.HasIndex(e => e.ActivityId, "ix_expenses_activity");

        entity.HasIndex(e => e.CategoryId, "ix_expenses_category");

        entity.HasIndex(e => e.CropId, "ix_expenses_crop");

        entity.HasIndex(e => new { e.CropCycleId, e.ExpenseDate }, "ix_expenses_cycle");

        entity.HasIndex(e => new { e.FarmerProfileId, e.ExpenseDate }, "ix_expenses_farmer_date")
            .IsDescending(false, true)
            .HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.LandId, "ix_expenses_land");

        entity.HasIndex(e => e.MachineryId, "ix_expenses_machinery");

        entity.HasIndex(e => e.OrderId, "ix_expenses_order");

        entity.HasIndex(e => e.ReceiptFileId, "ix_expenses_receipt_file_id_28d5fa");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ActivityId).HasColumnName("activity_id");
        entity.Property(e => e.Amount)
            .HasPrecision(14, 2)
            .HasColumnName("amount");
        entity.Property(e => e.CategoryId).HasColumnName("category_id");
        entity.Property(e => e.ClientMutationId).HasColumnName("client_mutation_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropCycleId).HasColumnName("crop_cycle_id");
        entity.Property(e => e.CropId).HasColumnName("crop_id");
        entity.Property(e => e.Currency)
            .HasMaxLength(3)
            .HasDefaultValueSql("'INR'::bpchar")
            .IsFixedLength()
            .HasColumnName("currency");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.ExpenseDate).HasColumnName("expense_date");
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.LandId).HasColumnName("land_id");
        entity.Property(e => e.MachineryId).HasColumnName("machinery_id");
        entity.Property(e => e.Notes).HasColumnName("notes");
        entity.Property(e => e.OrderId).HasColumnName("order_id");
        entity.Property(e => e.ReceiptFileId).HasColumnName("receipt_file_id");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.VendorName).HasColumnName("vendor_name");

        entity.HasOne(d => d.Activity).WithMany(p => p.Expenses)
            .HasForeignKey(d => d.ActivityId)
            .HasConstraintName("expenses_activity_id_fkey");

        entity.HasOne(d => d.Category).WithMany(p => p.Expenses)
            .HasForeignKey(d => d.CategoryId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("expenses_category_id_fkey");

        entity.HasOne(d => d.CropCycle).WithMany(p => p.Expenses)
            .HasForeignKey(d => d.CropCycleId)
            .HasConstraintName("expenses_crop_cycle_id_fkey");

        entity.HasOne(d => d.Crop).WithMany(p => p.Expenses)
            .HasForeignKey(d => d.CropId)
            .HasConstraintName("expenses_crop_id_fkey");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.Expenses)
            .HasForeignKey(d => d.FarmerProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("expenses_farmer_profile_id_fkey");

        entity.HasOne(d => d.Land).WithMany(p => p.Expenses)
            .HasForeignKey(d => d.LandId)
            .HasConstraintName("expenses_land_id_fkey");

        entity.HasOne(d => d.Machinery).WithMany(p => p.Expenses)
            .HasForeignKey(d => d.MachineryId)
            .HasConstraintName("expenses_machinery_id_fkey");

        entity.HasOne(d => d.Order).WithMany(p => p.Expenses)
            .HasForeignKey(d => d.OrderId)
            .HasConstraintName("fk_expenses_order");

        entity.HasOne(d => d.ReceiptFile).WithMany(p => p.Expenses)
            .HasForeignKey(d => d.ReceiptFileId)
            .HasConstraintName("expenses_receipt_file_id_fkey");
    }
}
