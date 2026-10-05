using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class User
{
    public Guid Id { get; set; }

    public Guid? OrganizationId { get; set; }

    public string? MobileNumber { get; set; }

    public DateTime? MobileVerifiedAt { get; set; }

    public string? Email { get; set; }

    public DateTime? EmailVerifiedAt { get; set; }

    public string? PasswordHash { get; set; }

    public string? PasswordAlgorithm { get; set; }

    public DateTime? PasswordChangedAt { get; set; }

    public string PreferredLanguage { get; set; } = null!;

    public string TimeZoneId { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int FailedLoginCount { get; set; }

    public DateTime? LockoutEndAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public bool MfaEnabled { get; set; }

    public byte[]? MfaSecretEncrypted { get; set; }

    public Guid SecurityStamp { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ICollection<AdCampaign> AdCampaigns { get; set; } = new List<AdCampaign>();

    public virtual ICollection<AdCreative> AdCreatives { get; set; } = new List<AdCreative>();

    public virtual ICollection<AdvertiserProfile> AdvertiserProfiles { get; set; } = new List<AdvertiserProfile>();

    public virtual ICollection<AiFeedback> AiFeedbackCandidateReviewedByNavigations { get; set; } = new List<AiFeedback>();

    public virtual ICollection<AiFeedback> AiFeedbackUsers { get; set; } = new List<AiFeedback>();

    public virtual ICollection<AiModelVersion> AiModelVersions { get; set; } = new List<AiModelVersion>();

    public virtual ICollection<BrandProfile> BrandProfiles { get; set; } = new List<BrandProfile>();

    public virtual Cart? Cart { get; set; }

    public virtual ICollection<ChatParticipant> ChatParticipants { get; set; } = new List<ChatParticipant>();

    public virtual ICollection<Complaint> ComplaintComplainantUsers { get; set; } = new List<Complaint>();

    public virtual ICollection<Complaint> ComplaintResolvedByNavigations { get; set; } = new List<Complaint>();

    public virtual ICollection<ConsentRecord> ConsentRecords { get; set; } = new List<ConsentRecord>();

    public virtual ConsultantProfile? ConsultantProfile { get; set; }

    public virtual ICollection<ConsultantReview> ConsultantReviews { get; set; } = new List<ConsultantReview>();

    public virtual ICollection<ConsultantScheduleHistory> ConsultantScheduleHistories { get; set; } = new List<ConsultantScheduleHistory>();

    public virtual ICollection<CropActivityCompletion> CropActivityCompletions { get; set; } = new List<CropActivityCompletion>();

    public virtual ICollection<CropActivityEvidence> CropActivityEvidences { get; set; } = new List<CropActivityEvidence>();

    public virtual ICollection<CropActivityHistory> CropActivityHistories { get; set; } = new List<CropActivityHistory>();

    public virtual ICollection<CropCalendarActivity> CropCalendarActivities { get; set; } = new List<CropCalendarActivity>();

    public virtual ICollection<CropCycleStageHistory> CropCycleStageHistories { get; set; } = new List<CropCycleStageHistory>();

    public virtual ICollection<DatasetImage> DatasetImageLabeledByNavigations { get; set; } = new List<DatasetImage>();

    public virtual ICollection<DatasetImage> DatasetImageReviewedByNavigations { get; set; } = new List<DatasetImage>();

    public virtual ICollection<DatasetVersion> DatasetVersions { get; set; } = new List<DatasetVersion>();

    public virtual ICollection<DiseaseScan> DiseaseScans { get; set; } = new List<DiseaseScan>();

    public virtual FarmerProfile? FarmerProfile { get; set; }

    public virtual ICollection<FileObject> FileObjects { get; set; } = new List<FileObject>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<KnowledgeDocument> KnowledgeDocumentApprovedByNavigations { get; set; } = new List<KnowledgeDocument>();

    public virtual ICollection<KnowledgeDocument> KnowledgeDocumentAuthorUsers { get; set; } = new List<KnowledgeDocument>();

    public virtual ICollection<LabBookingEvent> LabBookingEvents { get; set; } = new List<LabBookingEvent>();

    public virtual ICollection<LabReport> LabReports { get; set; } = new List<LabReport>();

    public virtual ICollection<LabResult> LabResultReviewers { get; set; } = new List<LabResult>();

    public virtual ICollection<LabResult> LabResultTechnicians { get; set; } = new List<LabResult>();

    public virtual ICollection<LabSample> LabSamples { get; set; } = new List<LabSample>();

    public virtual ICollection<LaboratoryProfile> LaboratoryProfiles { get; set; } = new List<LaboratoryProfile>();

    public virtual ICollection<MessageReadReceipt> MessageReadReceipts { get; set; } = new List<MessageReadReceipt>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<NotificationPreference> NotificationPreferences { get; set; } = new List<NotificationPreference>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<NurseryProfile> NurseryProfiles { get; set; } = new List<NurseryProfile>();

    public virtual ICollection<OrderStatusHistory> OrderStatusHistories { get; set; } = new List<OrderStatusHistory>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Organization? Organization { get; set; }

    public virtual ICollection<OtpVerification> OtpVerifications { get; set; } = new List<OtpVerification>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Language PreferredLanguageNavigation { get; set; } = null!;

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual ICollection<Refund> RefundApprovedByNavigations { get; set; } = new List<Refund>();

    public virtual ICollection<Refund> RefundRequestedByNavigations { get; set; } = new List<Refund>();

    public virtual ICollection<Review> ReviewModeratedByNavigations { get; set; } = new List<Review>();

    public virtual ICollection<Review> ReviewReviewerUsers { get; set; } = new List<Review>();

    public virtual ICollection<ScheduleChangeProposal> ScheduleChangeProposalDecidedByNavigations { get; set; } = new List<ScheduleChangeProposal>();

    public virtual ICollection<ScheduleChangeProposal> ScheduleChangeProposalDeciderUsers { get; set; } = new List<ScheduleChangeProposal>();

    public virtual ICollection<ScheduleChangeProposal> ScheduleChangeProposalRaisedByUsers { get; set; } = new List<ScheduleChangeProposal>();

    public virtual ICollection<ShopProfile> ShopProfiles { get; set; } = new List<ShopProfile>();

    public virtual ICollection<SoilReport> SoilReports { get; set; } = new List<SoilReport>();

    public virtual ICollection<SupportTicket> SupportTicketAssignedAgents { get; set; } = new List<SupportTicket>();

    public virtual ICollection<SupportTicketMessage> SupportTicketMessages { get; set; } = new List<SupportTicketMessage>();

    public virtual ICollection<SupportTicket> SupportTicketUsers { get; set; } = new List<SupportTicket>();

    public virtual ICollection<UserDevice> UserDevices { get; set; } = new List<UserDevice>();

    public virtual ICollection<UserRole> UserRoleGrantedByNavigations { get; set; } = new List<UserRole>();

    public virtual ICollection<UserRole> UserRoleUsers { get; set; } = new List<UserRole>();

    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();

    public virtual ICollection<VerificationRecord> VerificationRecords { get; set; } = new List<VerificationRecord>();
}
