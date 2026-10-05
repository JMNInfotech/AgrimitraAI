using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class Crop
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string NameLocal { get; set; } = null!;

    public string? ScientificName { get; set; }

    public string Category { get; set; } = null!;

    public bool IsPerennial { get; set; }

    public string? IconKey { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<AiModel> AiModels { get; set; } = new List<AiModel>();

    public virtual ICollection<ConsultantCropCarePlan> ConsultantCropCarePlans { get; set; } = new List<ConsultantCropCarePlan>();

    public virtual ICollection<ConsultantExpertise> ConsultantExpertises { get; set; } = new List<ConsultantExpertise>();

    public virtual ICollection<CropCalendarActivity> CropCalendarActivities { get; set; } = new List<CropCalendarActivity>();

    public virtual ICollection<CropCycle> CropCycles { get; set; } = new List<CropCycle>();

    public virtual ICollection<CropDisease> CropDiseases { get; set; } = new List<CropDisease>();

    public virtual ICollection<CropStage> CropStages { get; set; } = new List<CropStage>();

    public virtual ICollection<CropVariety> CropVarieties { get; set; } = new List<CropVariety>();

    public virtual ICollection<DatasetImage> DatasetImages { get; set; } = new List<DatasetImage>();

    public virtual ICollection<Dataset> Datasets { get; set; } = new List<Dataset>();

    public virtual ICollection<DiseaseResult> DiseaseResults { get; set; } = new List<DiseaseResult>();

    public virtual ICollection<DiseaseScan> DiseaseScans { get; set; } = new List<DiseaseScan>();

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual ICollection<FarmDiary> FarmDiaries { get; set; } = new List<FarmDiary>();

    public virtual ICollection<IncomeRecord> IncomeRecords { get; set; } = new List<IncomeRecord>();

    public virtual ICollection<KnowledgeDocument> KnowledgeDocuments { get; set; } = new List<KnowledgeDocument>();

    public virtual ICollection<LabBooking> LabBookings { get; set; } = new List<LabBooking>();

    public virtual ICollection<NurseryProduct> NurseryProducts { get; set; } = new List<NurseryProduct>();

    public virtual ICollection<ProductionRecord> ProductionRecords { get; set; } = new List<ProductionRecord>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
