using Agrimitra.SharedKernel;

namespace Agrimitra.Domain.Calendar;

public static class CalendarErrors
{
    public static readonly Error Forbidden = new("calendar.forbidden", "The actor is not allowed to change this activity.");
    public static readonly Error OccurrenceNotFound = new("calendar.occurrence_not_found", "Occurrence not found.");
    public static readonly Error InvalidTransition = new("calendar.invalid_transition", "The occurrence status does not allow this action.");
    public static readonly Error PastTime = new("calendar.past_time", "The new time must be in the future.");
    public static readonly Error ReasonRequired = new("calendar.reason_required", "A reason is required.");
    public static readonly Error InvalidRecurrence = new("calendar.invalid_recurrence", "Recurrence rule is invalid.");
    public static readonly Error InvalidSchedule = new("calendar.invalid_schedule", "Schedule is invalid.");
    public static readonly Error ApprovalRequired = new("calendar.consultant_approval_required",
        "This is a mandatory consultant activity; a change request was needed instead of a direct change.");
    public static readonly Error ProposalNotPending = new("calendar.proposal_not_pending", "The proposal has already been decided or expired.");
    public static readonly Error UnknownTimeZone = new("calendar.unknown_timezone", "Unknown time zone.");
}
