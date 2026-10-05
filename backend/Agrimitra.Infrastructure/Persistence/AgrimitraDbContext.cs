using System;
using System.Collections.Generic;
using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Agrimitra.Infrastructure.Persistence;

public partial class AgrimitraDbContext : DbContext
{
    public AgrimitraDbContext(DbContextOptions<AgrimitraDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivityRecurrenceRule> ActivityRecurrenceRules { get; set; }

    public virtual DbSet<AdBilling> AdBillings { get; set; }

    public virtual DbSet<AdBudgetLedger> AdBudgetLedgers { get; set; }

    public virtual DbSet<AdCampaign> AdCampaigns { get; set; }

    public virtual DbSet<AdCampaignPlacement> AdCampaignPlacements { get; set; }

    public virtual DbSet<AdCreative> AdCreatives { get; set; }

    public virtual DbSet<AdDailyRollup> AdDailyRollups { get; set; }

    public virtual DbSet<AdPlacement> AdPlacements { get; set; }

    public virtual DbSet<AdTargeting> AdTargetings { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Advertiser> Advertisers { get; set; }

    public virtual DbSet<AdvertiserProfile> AdvertiserProfiles { get; set; }

    public virtual DbSet<AiAlert> AiAlerts { get; set; }

    public virtual DbSet<AiFeedback> AiFeedbacks { get; set; }

    public virtual DbSet<AiModel> AiModels { get; set; }

    public virtual DbSet<AiModelVersion> AiModelVersions { get; set; }

    public virtual DbSet<AiRecommendation> AiRecommendations { get; set; }

    public virtual DbSet<BackgroundJobRun> BackgroundJobRuns { get; set; }

    public virtual DbSet<BrandProfile> BrandProfiles { get; set; }

    public virtual DbSet<Buyer> Buyers { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<ChatParticipant> ChatParticipants { get; set; }

    public virtual DbSet<ChatRoom> ChatRooms { get; set; }

    public virtual DbSet<Complaint> Complaints { get; set; }

    public virtual DbSet<ConsentRecord> ConsentRecords { get; set; }

    public virtual DbSet<ConsultantAvailability> ConsultantAvailabilities { get; set; }

    public virtual DbSet<ConsultantCropCarePlan> ConsultantCropCarePlans { get; set; }

    public virtual DbSet<ConsultantDocument> ConsultantDocuments { get; set; }

    public virtual DbSet<ConsultantExpertise> ConsultantExpertises { get; set; }

    public virtual DbSet<ConsultantFarmerLink> ConsultantFarmerLinks { get; set; }

    public virtual DbSet<ConsultantProfile> ConsultantProfiles { get; set; }

    public virtual DbSet<ConsultantReview> ConsultantReviews { get; set; }

    public virtual DbSet<ConsultantScheduleActivity> ConsultantScheduleActivities { get; set; }

    public virtual DbSet<ConsultantScheduleHistory> ConsultantScheduleHistories { get; set; }

    public virtual DbSet<Consultation> Consultations { get; set; }

    public virtual DbSet<ConsultationAppointment> ConsultationAppointments { get; set; }

    public virtual DbSet<ConsultationRequest> ConsultationRequests { get; set; }

    public virtual DbSet<ConsultationService> ConsultationServices { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Crop> Crops { get; set; }

    public virtual DbSet<CropActivityCompletion> CropActivityCompletions { get; set; }

    public virtual DbSet<CropActivityEvidence> CropActivityEvidences { get; set; }

    public virtual DbSet<CropActivityHistory> CropActivityHistories { get; set; }

    public virtual DbSet<CropActivityOccurrence> CropActivityOccurrences { get; set; }

    public virtual DbSet<CropActivityReminder> CropActivityReminders { get; set; }

    public virtual DbSet<CropCalendarActivity> CropCalendarActivities { get; set; }

    public virtual DbSet<CropCalendarPlan> CropCalendarPlans { get; set; }

    public virtual DbSet<CropCycle> CropCycles { get; set; }

    public virtual DbSet<CropCycleStageHistory> CropCycleStageHistories { get; set; }

    public virtual DbSet<CropDisease> CropDiseases { get; set; }

    public virtual DbSet<CropDiseaseHistory> CropDiseaseHistories { get; set; }

    public virtual DbSet<CropProduction> CropProductions { get; set; }

    public virtual DbSet<CropStage> CropStages { get; set; }

    public virtual DbSet<CropVariety> CropVarieties { get; set; }

    public virtual DbSet<Dataset> Datasets { get; set; }

    public virtual DbSet<DatasetImage> DatasetImages { get; set; }

    public virtual DbSet<DatasetVersion> DatasetVersions { get; set; }

    public virtual DbSet<Delivery> Deliveries { get; set; }

    public virtual DbSet<DiseaseResult> DiseaseResults { get; set; }

    public virtual DbSet<DiseaseScan> DiseaseScans { get; set; }

    public virtual DbSet<DiseaseScanImage> DiseaseScanImages { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Embedding> Embeddings { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<ExpenseCategory> ExpenseCategories { get; set; }

    public virtual DbSet<Farm> Farms { get; set; }

    public virtual DbSet<FarmDiary> FarmDiaries { get; set; }

    public virtual DbSet<FarmDiaryAttachment> FarmDiaryAttachments { get; set; }

    public virtual DbSet<FarmerProfile> FarmerProfiles { get; set; }

    public virtual DbSet<FileObject> FileObjects { get; set; }

    public virtual DbSet<HarvestRecord> HarvestRecords { get; set; }

    public virtual DbSet<IdempotencyKey> IdempotencyKeys { get; set; }

    public virtual DbSet<InboxMessage> InboxMessages { get; set; }

    public virtual DbSet<IncomeRecord> IncomeRecords { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceLine> InvoiceLines { get; set; }

    public virtual DbSet<IrrigationType> IrrigationTypes { get; set; }

    public virtual DbSet<KnowledgeCategory> KnowledgeCategories { get; set; }

    public virtual DbSet<KnowledgeChunk> KnowledgeChunks { get; set; }

    public virtual DbSet<KnowledgeDocument> KnowledgeDocuments { get; set; }

    public virtual DbSet<LabBooking> LabBookings { get; set; }

    public virtual DbSet<LabBookingEvent> LabBookingEvents { get; set; }

    public virtual DbSet<LabReport> LabReports { get; set; }

    public virtual DbSet<LabResult> LabResults { get; set; }

    public virtual DbSet<LabResultValue> LabResultValues { get; set; }

    public virtual DbSet<LabSample> LabSamples { get; set; }

    public virtual DbSet<LabTestType> LabTestTypes { get; set; }

    public virtual DbSet<LaboratoryProfile> LaboratoryProfiles { get; set; }

    public virtual DbSet<LaboratoryService> LaboratoryServices { get; set; }

    public virtual DbSet<Land> Lands { get; set; }

    public virtual DbSet<LandBoundary> LandBoundaries { get; set; }

    public virtual DbSet<LandDocument> LandDocuments { get; set; }

    public virtual DbSet<LandImage> LandImages { get; set; }

    public virtual DbSet<LandWeatherLocation> LandWeatherLocations { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Machinery> Machineries { get; set; }

    public virtual DbSet<MachineryInsurance> MachineryInsurances { get; set; }

    public virtual DbSet<MachineryMaintenance> MachineryMaintenances { get; set; }

    public virtual DbSet<MachineryService> MachineryServices { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<MessageAttachment> MessageAttachments { get; set; }

    public virtual DbSet<MessageReadReceipt> MessageReadReceipts { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<NotificationDeliveryLog> NotificationDeliveryLogs { get; set; }

    public virtual DbSet<NotificationPreference> NotificationPreferences { get; set; }

    public virtual DbSet<NotificationTemplate> NotificationTemplates { get; set; }

    public virtual DbSet<NurseryBatch> NurseryBatches { get; set; }

    public virtual DbSet<NurseryInventory> NurseryInventories { get; set; }

    public virtual DbSet<NurseryOrder> NurseryOrders { get; set; }

    public virtual DbSet<NurseryProduct> NurseryProducts { get; set; }

    public virtual DbSet<NurseryProductVariety> NurseryProductVarieties { get; set; }

    public virtual DbSet<NurseryProfile> NurseryProfiles { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<OtpVerification> OtpVerifications { get; set; }

    public virtual DbSet<OutboxMessage> OutboxMessages { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Pincode> Pincodes { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    public virtual DbSet<PrescriptionAttachment> PrescriptionAttachments { get; set; }

    public virtual DbSet<PrescriptionFollowUp> PrescriptionFollowUps { get; set; }

    public virtual DbSet<PrescriptionItem> PrescriptionItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductBatch> ProductBatches { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductInventory> ProductInventories { get; set; }

    public virtual DbSet<ProductLocation> ProductLocations { get; set; }

    public virtual DbSet<ProductionRecord> ProductionRecords { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Refund> Refunds { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<ScheduleChangeProposal> ScheduleChangeProposals { get; set; }

    public virtual DbSet<ScheduleConflict> ScheduleConflicts { get; set; }

    public virtual DbSet<Shipment> Shipments { get; set; }

    public virtual DbSet<ShopProduct> ShopProducts { get; set; }

    public virtual DbSet<ShopProfile> ShopProfiles { get; set; }

    public virtual DbSet<SoilMeasurement> SoilMeasurements { get; set; }

    public virtual DbSet<SoilParameter> SoilParameters { get; set; }

    public virtual DbSet<SoilReport> SoilReports { get; set; }

    public virtual DbSet<SoilSample> SoilSamples { get; set; }

    public virtual DbSet<SoilTest> SoilTests { get; set; }

    public virtual DbSet<SoilType> SoilTypes { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<SupportTicket> SupportTickets { get; set; }

    public virtual DbSet<SupportTicketAttachment> SupportTicketAttachments { get; set; }

    public virtual DbSet<SupportTicketMessage> SupportTicketMessages { get; set; }

    public virtual DbSet<SystemConfiguration> SystemConfigurations { get; set; }

    public virtual DbSet<Taluka> Talukas { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserDevice> UserDevices { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserSession> UserSessions { get; set; }

    public virtual DbSet<VerificationDocument> VerificationDocuments { get; set; }

    public virtual DbSet<VerificationRecord> VerificationRecords { get; set; }

    public virtual DbSet<Village> Villages { get; set; }

    public virtual DbSet<WaterSource> WaterSources { get; set; }

    public virtual DbSet<WeatherAlert> WeatherAlerts { get; set; }

    public virtual DbSet<WeatherLocation> WeatherLocations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("citext")
            .HasPostgresExtension("pg_trgm")
            .HasPostgresExtension("postgis")
            .HasPostgresExtension("vector");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AgrimitraDbContext).Assembly);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
