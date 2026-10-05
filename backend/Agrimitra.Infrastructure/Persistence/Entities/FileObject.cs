using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class FileObject
{
    public Guid Id { get; set; }

    public Guid? OwnerUserId { get; set; }

    public string Bucket { get; set; } = null!;

    public string ObjectKey { get; set; } = null!;

    public string? OriginalName { get; set; }

    public string MimeType { get; set; } = null!;

    public long SizeBytes { get; set; }

    public string? Sha256 { get; set; }

    public string Purpose { get; set; } = null!;

    public string ScanStatus { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int? Width { get; set; }

    public int? Height { get; set; }

    public decimal? DurationSeconds { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ICollection<AdCreative> AdCreativeImageFiles { get; set; } = new List<AdCreative>();

    public virtual ICollection<AdCreative> AdCreativeVideoFiles { get; set; } = new List<AdCreative>();

    public virtual ICollection<BrandProfile> BrandProfiles { get; set; } = new List<BrandProfile>();

    public virtual ICollection<ConsultantDocument> ConsultantDocuments { get; set; } = new List<ConsultantDocument>();

    public virtual ICollection<ConsultantProfile> ConsultantProfiles { get; set; } = new List<ConsultantProfile>();

    public virtual ICollection<CropActivityEvidence> CropActivityEvidences { get; set; } = new List<CropActivityEvidence>();

    public virtual ICollection<DatasetImage> DatasetImages { get; set; } = new List<DatasetImage>();

    public virtual ICollection<DatasetVersion> DatasetVersions { get; set; } = new List<DatasetVersion>();

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

    public virtual ICollection<DiseaseResult> DiseaseResults { get; set; } = new List<DiseaseResult>();

    public virtual ICollection<DiseaseScanImage> DiseaseScanImages { get; set; } = new List<DiseaseScanImage>();

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual ICollection<FarmDiaryAttachment> FarmDiaryAttachments { get; set; } = new List<FarmDiaryAttachment>();

    public virtual ICollection<FarmerProfile> FarmerProfiles { get; set; } = new List<FarmerProfile>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<KnowledgeDocument> KnowledgeDocuments { get; set; } = new List<KnowledgeDocument>();

    public virtual ICollection<LabReport> LabReports { get; set; } = new List<LabReport>();

    public virtual ICollection<LaboratoryProfile> LaboratoryProfiles { get; set; } = new List<LaboratoryProfile>();

    public virtual ICollection<LandDocument> LandDocuments { get; set; } = new List<LandDocument>();

    public virtual ICollection<LandImage> LandImages { get; set; } = new List<LandImage>();

    public virtual ICollection<MachineryInsurance> MachineryInsurances { get; set; } = new List<MachineryInsurance>();

    public virtual ICollection<MachineryMaintenance> MachineryMaintenances { get; set; } = new List<MachineryMaintenance>();

    public virtual ICollection<MessageAttachment> MessageAttachmentFileObjects { get; set; } = new List<MessageAttachment>();

    public virtual ICollection<MessageAttachment> MessageAttachmentThumbnailFiles { get; set; } = new List<MessageAttachment>();

    public virtual ICollection<NurseryProfile> NurseryProfiles { get; set; } = new List<NurseryProfile>();

    public virtual User? OwnerUser { get; set; }

    public virtual ICollection<PrescriptionAttachment> PrescriptionAttachments { get; set; } = new List<PrescriptionAttachment>();

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<ShopProduct> ShopProducts { get; set; } = new List<ShopProduct>();

    public virtual ICollection<ShopProfile> ShopProfiles { get; set; } = new List<ShopProfile>();

    public virtual ICollection<SoilReport> SoilReports { get; set; } = new List<SoilReport>();

    public virtual ICollection<SupportTicketAttachment> SupportTicketAttachments { get; set; } = new List<SupportTicketAttachment>();

    public virtual ICollection<VerificationDocument> VerificationDocuments { get; set; } = new List<VerificationDocument>();
}
