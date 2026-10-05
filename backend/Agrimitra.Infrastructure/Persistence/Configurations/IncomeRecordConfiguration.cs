using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class IncomeRecordConfiguration : IEntityTypeConfiguration<IncomeRecord>
{
    public void Configure(EntityTypeBuilder<IncomeRecord> entity)
    {
        entity.HasKey(e => e.Id).HasName("income_records_pkey");

        entity.ToTable("income_records");

        entity.HasIndex(e => e.ClientMutationId, "income_records_client_mutation_id_key").IsUnique();

        entity.HasIndex(e => e.BuyerId, "ix_income_buyer");

        entity.HasIndex(e => e.CropId, "ix_income_crop");

        entity.HasIndex(e => e.CropCycleId, "ix_income_cycle");

        entity.HasIndex(e => new { e.FarmerProfileId, e.IncomeDate }, "ix_income_farmer_date")
            .IsDescending(false, true)
            .HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.LandId, "ix_income_land");

        entity.HasIndex(e => e.ProductionRecordId, "ix_income_production");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.BuyerId).HasColumnName("buyer_id");
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
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.IncomeDate).HasColumnName("income_date");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.LandId).HasColumnName("land_id");
        entity.Property(e => e.Notes).HasColumnName("notes");
        entity.Property(e => e.OtherCharges)
            .HasPrecision(14, 2)
            .HasColumnName("other_charges");
        entity.Property(e => e.ProduceName).HasColumnName("produce_name");
        entity.Property(e => e.ProductionRecordId).HasColumnName("production_record_id");
        entity.Property(e => e.Quantity)
            .HasPrecision(14, 3)
            .HasColumnName("quantity");
        entity.Property(e => e.Revenue)
            .HasPrecision(16, 2)
            .HasComputedColumnSql("round((quantity * selling_price_per_unit), 2)", true)
            .HasColumnName("revenue");
        entity.Property(e => e.SellingPricePerUnit)
            .HasPrecision(14, 2)
            .HasColumnName("selling_price_per_unit");
        entity.Property(e => e.Unit).HasColumnName("unit");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Buyer).WithMany(p => p.IncomeRecords)
            .HasForeignKey(d => d.BuyerId)
            .HasConstraintName("income_records_buyer_id_fkey");

        entity.HasOne(d => d.CropCycle).WithMany(p => p.IncomeRecords)
            .HasForeignKey(d => d.CropCycleId)
            .HasConstraintName("income_records_crop_cycle_id_fkey");

        entity.HasOne(d => d.Crop).WithMany(p => p.IncomeRecords)
            .HasForeignKey(d => d.CropId)
            .HasConstraintName("income_records_crop_id_fkey");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.IncomeRecords)
            .HasForeignKey(d => d.FarmerProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("income_records_farmer_profile_id_fkey");

        entity.HasOne(d => d.Land).WithMany(p => p.IncomeRecords)
            .HasForeignKey(d => d.LandId)
            .HasConstraintName("income_records_land_id_fkey");

        entity.HasOne(d => d.ProductionRecord).WithMany(p => p.IncomeRecords)
            .HasForeignKey(d => d.ProductionRecordId)
            .HasConstraintName("income_records_production_record_id_fkey");
    }
}
