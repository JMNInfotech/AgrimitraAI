namespace Agrimitra.Domain.Calendar;

public enum ActivityType
{
    Irrigation, Fertilizer, Pesticide, Fungicide, Herbicide, PestMonitoring, DiseaseInspection,
    SoilTesting, Pruning, Weeding, Planting, Harvesting, MachineryUsage, ConsultantFollowUp,
    LabTest, MachineryService, Other
}

public enum ActivityPriority { Low, Normal, High, Critical }

public enum ActivityOrigin { Consultant, Farmer, AiAccepted, Machinery, System }

public enum OccurrenceStatus { Planned, Scheduled, Upcoming, InProgress, Completed, Skipped, Cancelled, Rescheduled }

public enum OccurrenceAction { Confirm, MarkUpcoming, Start, Complete, Skip, Cancel, Reschedule }

public enum RecurrenceFrequency { Daily, Weekly, EveryNDays }

public enum ActorKind { Farmer, Consultant, Admin, System, AiService }

public enum ChangeVia { Direct, AiProposalApproval, FarmerRequestApproval, System }

public enum ChangeType { Created, Edited, Rescheduled, Cancelled, Completed, Skipped, ProposalApproved, ProposalRejected }

public enum CompletionOutcome { Completed, Skipped, NotApplicable }
