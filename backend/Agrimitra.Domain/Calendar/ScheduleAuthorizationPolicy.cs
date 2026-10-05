namespace Agrimitra.Domain.Calendar;

/// <summary>
/// Domain-level guard (independent of API permissions). Consultant-origin activities are locked:
/// only the owning consultant, or an admin who states a reason, may change the schedule. The AI service
/// identity can never mutate any schedule directly.
/// </summary>
public static class ScheduleAuthorizationPolicy
{
    public static bool CanChangeSchedule(ScheduledActivity a, ActorContext actor)
    {
        if (actor.Kind == ActorKind.AiService) return false;
        if (actor.Kind == ActorKind.Admin) return !string.IsNullOrWhiteSpace(actor.AdminReason);

        if (a.IsLockedByConsultant)
            return actor.Kind == ActorKind.Consultant && actor.UserId == a.ConsultantUserId;

        return (actor.Kind == ActorKind.Farmer && actor.UserId == a.FarmerUserId) || actor.Kind == ActorKind.System;
    }

    /// <summary>Only the farmer who owns the activity records what actually happened.</summary>
    public static bool CanComplete(ScheduledActivity a, ActorContext actor) =>
        actor.Kind == ActorKind.Farmer && actor.UserId == a.FarmerUserId;

    /// <summary>Who decides an AI/farmer schedule-change proposal for this activity.</summary>
    public static bool CanDecideProposal(ScheduledActivity a, ActorContext actor) =>
        actor.Kind != ActorKind.AiService && actor.Kind != ActorKind.System &&
        (a.IsLockedByConsultant
            ? actor.Kind == ActorKind.Consultant && actor.UserId == a.ConsultantUserId
            : actor.Kind == ActorKind.Farmer && actor.UserId == a.FarmerUserId);
}
