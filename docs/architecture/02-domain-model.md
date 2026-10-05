# AGRIMITRA — Phase 2: Domain Model

Scope: the core spine (Identity, Geography, Profiles, Farm, Crop, Crop Calendar & Activity Engine, Smart Scheduling, Consultation, Prescription, Crop Care Plan, Diary) plus the AI-lineage aggregates those modules touch. Commerce, Ads, Chat, Lab, Nursery, Shop domain models are specified in their own phases but follow the conventions below.

Follows the rule-92 checklist: architecture → entities → relationships → contracts → permissions → rules → edge cases → screens → tests.

---

## 1. Modelling conventions (apply to every module)

| Convention | Rule |
|---|---|
| Identity | `Guid` (UUIDv7) strongly-typed IDs (`FarmId`, `LandId`, …) — no raw `Guid` in signatures |
| Aggregates | One transaction = one aggregate. Cross-aggregate links by ID only; no navigation across module boundaries |
| Base types | `Entity<TId>`, `AggregateRoot<TId>` (collects `IDomainEvent`s), `ValueObject`, `AuditableEntity` (created/updated at/by), `ITenantScoped` (`OrganizationId?`) |
| Concurrency | `RowVersion` (xmin / uint) on every aggregate root; API surfaces 409 on conflict |
| Time | All instants `DateTimeOffset` UTC. Scheduling additionally stores `LocalTime` + IANA `TimeZoneId` so "7:00 AM" stays 7:00 AM local |
| Errors | `Result<T>` with typed `Error(code, message)`; exceptions only for programmer faults. No magic strings: codes live in `*Errors` static classes |
| Enums | Persisted as strings; additive-only |
| Events | Domain events raised in aggregate, converted to integration events through the outbox in the same transaction |
| Localisation | Domain stores codes/keys; display text resolved from resource bundles by `preferredLanguage` |
| Deletion | Soft delete (`DeletedAt`) for user-owned data; append-only (never updated/deleted) for history, audit, completions, inference logs |

### Shared value objects
`GeoPoint(lat, lon)` (validated range, SRID 4326) · `GeoPolygon` (closed ring, ≥4 points, no self-intersection, area computed server-side) · `Money(amount decimal(18,2), currency)` · `Area(value, AreaUnit)` (Acre, Hectare, Guntha, SqMeter; conversion to canonical m²) · `Quantity(value, unit)` · `LocalizedText` (map `LanguageCode → string`, with fallback chain) · `PhoneNumber` (E.164) · `EmailAddress` · `TimeOfDay` · `DateRange` · `FileRef(objectKey, mime, size, sha256, scanStatus)` · `EncryptedValue` (ciphertext + key id + blind index).

---

## 2. Bounded contexts and ownership

```
Identity ─┬─> Profiles ─┬─> Farm ──> Crop ──> CropCalendar ──> Diary
          │             │                         ▲   ▲   ▲
Geography ┘             └─> Consultation ─> Prescription / CropCarePlan
                                                  │
Weather ─────────────────────> SmartScheduling ───┘      AI (advisory only)
```
Rules: arrows are *reads/commands via contracts*, never shared tables. `CropCalendar` owns activity truth; `Consultation` owns care plans/prescriptions but **publishes** activities into CropCalendar through `ICalendarScheduling` (so the calendar stays the single scheduler for consultants, farmers, machinery and system).

---

## 3. Identity & Access

**Aggregates**
- `User` (root): `Id`, `Mobile?`, `Email?`, `PreferredLanguage`, `Status {PendingVerification, Active, Locked, Suspended, Deleted}`, `FailedLoginCount`, `LockoutEndsAt?`, `Credential?` (Argon2id hash, params version), `MfaEnabled`, `Roles[]`(UserRole: roleId, grantedBy, grantedAt, expiresAt?), `Consents[]`.
- `Role` (root): `Name`, `IsSystem`, `Permissions[]` (set of `PermissionId`). `Permission`: `Code` ("calendar.activity.reschedule"), `Module`, `Description`.
- `UserSession` (root): device, refresh-token family, `RevokedAt`, `LastSeenAt`, IP, userAgent. `RefreshToken`: hash, `ReplacedBy`, `UsedAt` (reuse ⇒ revoke family).
- `OtpChallenge`: purpose {Register, Login, ResetPassword, ChangeMobile, ChangeEmail}, target, code hash, attempts, expiry.

**Invariants**: at least one verified identifier (mobile or email); mobile/email unique among non-deleted; OTP ≤5 attempts, 5-min TTL, per-target rate cap; lockout after N failures with progressive backoff; refresh-token reuse revokes the whole family; SuperAdmin role grant requires SuperAdmin actor + MFA; role removal can't leave zero SuperAdmins.

**Events**: `UserRegistered`, `UserVerified`, `LoginSucceeded/Failed`, `PasswordChanged`, `SessionRevoked`, `RoleGranted/Revoked`.

---

## 4. Geography & Address

- Master (reference aggregates, read-mostly, cached): `Country → State → District → Taluka → Village`, each with `Code`, `LocalizedText Name`, optional `Centroid`, `IsActive`. `Pincode` table maps pincode → (state, district) candidates.
- `Address` (value-object-like entity, reusable): `VillageId?`, `TalukaId`, `DistrictId`, `StateId`, `CountryId`, `City?`, `Pincode`, `Line1?`, `GeoPoint?`. Hierarchy consistency (village ∈ taluka ∈ district ∈ state) validated on write. No free-text-only geography.

---

## 5. Profiles

- `FarmerProfile` (root): `UserId`, `FarmerCode` (human ID, generated, non-guessable), `FullName`, `DateOfBirth?`, `Gender?`, `PhotoRef?`, `AddressId`, `HomeLocation?`, `GovIdRef?` (`EncryptedValue`, purpose-limited), `Consents` (data-sharing with consultants, analytics, ads-targeting-buckets), `OnboardingState`.
- `ConsultantProfile`: education, experience years, expertise (crop IDs, issue categories), languages, base location + service radius, fee schedule, availability rules, documents, `VerificationStatus {Unverified, Pending, Verified, Rejected, Suspended}`. "VERIFIED CONSULTANT" is derived solely from `Verified` set by an Admin action (audited).
- `NurseryProfile`, `LaboratoryProfile`, `ShopProfile`, `AdvertiserProfile`: same verification pattern; shop adds license details; all reference `AddressId` + `GeoPoint`.
- `VerificationRecord`: subject, documents, reviewer, decision, reason, timestamps.

**Invariants**: government ID never returned by default APIs (separate permission `farmer.govid.reveal`, reason-coded, audited); consent changes are append-only `ConsentRecord`s.

---

## 6. Farm & Land

- `Farm` (root): `FarmerId`, `Name` ("My Grapes Farm"), `AddressId?`, `Status`. Holds `Land` ids only logically.
- `Land` (root): `FarmId`, `FarmerId`, `Name`, `SurveyNumber` (Gat/Survey), `AddressId`, `Area`, `OwnershipType {Owned, Leased, Shared, Other}`, `Location: GeoPoint`, `Boundary: GeoPolygon?`, `SoilType`, `IrrigationType {Drip, Flood, Sprinkler, Rainfed, Other}`, `WaterSource`, `Documents[]`, `Images[]`, `Status {Active, Fallow, Archived}`.

**Invariants**: boundary polygon valid & non-self-intersecting; boundary area vs declared area differs >25% ⇒ warning (not block); boundary must contain/be near `Location` (≤ configurable distance); `Land.FarmerId` immutable; a land cannot be archived with active crop cycles.
**Events**: `LandCreated`, `LandBoundaryChanged` (triggers weather-grid re-binding).

---

## 7. Crop & Crop Cycle

**Master data (admin/content-managed, versioned):** `Crop` (code, `LocalizedText`, category), `CropVariety`, `CropStageTemplate` (crop-specific ordered stages with typical duration and typical activities, e.g. Grapes: Dormancy, Pruning, Bud break, Flowering, Fruit set, Veraison, Harvest).

**`CropCycle`** (root): `LandId`, `FarmerId`, `CropId`, `VarietyId?`, `Season {Kharif, Rabi, Summer, Perennial}`, `PlantingDate`, `ExpectedHarvestDate?`, `ActualHarvestDate?`, `Area`, `PlantCount?`, `CurrentStageId`, `Status {Planned, Active, Harvested, Failed, Closed}`, `StageHistory[]` (stage, startedAt, endedAt, source {Farmer, Consultant, AI-suggested-confirmed, System}), `HarvestRecords[]`, `DiseaseHistory[]` (references to confirmed scan/consultant diagnoses).

**Invariants**: crop-cycle area ≤ land area minus other overlapping active cycles' area (soft-warn for intercropping flag); stage transitions follow template order unless override with reason; `ActualHarvestDate ≥ PlantingDate`; only one *open* stage at a time; closing a cycle cancels future occurrences (via event) but keeps history.
**Events**: `CropCycleStarted`, `CropStageChanged` (→ smart scheduling + plan suggestions), `CropHarvested`, `CropCycleClosed`.

---

## 8. Crop Calendar & Activity Engine (core)

### 8.1 Concepts and their owners
| Concept | Aggregate | Owner | Mutability |
|---|---|---|---|
| AI Recommendation | `AIRecommendation` | AI module | immutable after creation; only `Feedback` appended |
| Consultant Prescription | `Prescription` | Prescription module | versioned (new version on edit) |
| Crop Care Plan | `CropCarePlan` | Consultation module | Draft → Published (versioned) → Completed/Cancelled |
| Scheduled Activity | `ScheduledActivity` + `ActivityOccurrence` | CropCalendar | state machine, every change logged |
| Completed Activity | `ActivityCompletion` | CropCalendar | **append-only** |
| Activity Evidence | `ActivityEvidence` | CropCalendar | append-only (soft-withdraw with reason) |

Provenance links (nullable, informational, never cascading): `ScheduledActivity.CarePlanId`, `.PrescriptionId`, `.AiRecommendationId`, `.OriginType`.

### 8.2 `ScheduledActivity` (root — the *definition*)
`FarmerId`, `LandId`, `CropCycleId?`, `Type` (enum: Irrigation, Fertilizer, Pesticide, Fungicide, Herbicide, PestMonitoring, DiseaseInspection, SoilTesting, Pruning, Weeding, Planting, Harvesting, MachineryUsage, ConsultantFollowUp, LabTest, MachineryService, Other), `Title: LocalizedText?`, `Description`, `Instructions`, `Priority {Low, Normal, High, Critical}`, `IsMandatory`, `OriginType {Consultant, Farmer, AiAccepted, Machinery, System}`, `AssignedBy` (user), `ConsultantId?`, `CarePlanId?`, `PrescriptionId?`, `LockedByConsultant` (derived: `OriginType == Consultant`), `Schedule` (value object below), `ReminderConfig[]`, `Attachments[]`, `Version`.

**`Schedule` value object**: `StartDate`, `StartTime: TimeOfDay?`, `EndTime?`, `TimeZoneId`, `Recurrence?`:
`Recurrence { Frequency {Daily, Weekly, EveryNDays, Custom}, Interval, ByWeekday[]?, Count?, Until? }` — exactly one of `Count`/`Until`/neither-with-horizon; rules produce occurrences via a pure `RecurrenceExpander` (unit/property-tested).

### 8.3 `ActivityOccurrence` (concrete instance; materialised on a rolling 90-day horizon)
`ActivityId`, `SeqNo`, `ScheduledStartUtc`, `ScheduledEndUtc?`, `Status`, `RescheduledToOccurrenceId?`, `OverrideInstructions?`, `Completion?` (1:0..1), `RowVersion`.

**Status state machine**
```
PLANNED ──publish/confirm──> SCHEDULED ──T-24h (worker)──> UPCOMING ──start──> IN_PROGRESS ──> COMPLETED
   │                             │                            │                     │
   └──────────cancel──────────>  CANCELLED <──────────────────┴─────────────────────┘
SCHEDULED/UPCOMING ──reschedule──> RESCHEDULED (terminal; links to new occurrence in SCHEDULED)
SCHEDULED/UPCOMING/IN_PROGRESS ──skip / not-applicable (+reason)──> SKIPPED
```
Terminal: COMPLETED, SKIPPED, CANCELLED, RESCHEDULED. `UPCOMING` is derived/advanced by the worker, not set by users. "Missed" is a *computed flag* (`end < now` and non-terminal), not a status, and raises `ActivityMissed`.

### 8.4 Who may do what (domain-level, in addition to API permissions)
| Action | Farmer-origin activity | Consultant-origin activity (locked) |
|---|---|---|
| Edit/cancel/reschedule | Farmer | **Only the owning consultant (or Admin with reason)** |
| Complete / add notes / upload evidence | Farmer | Farmer (consultant sees) |
| Skip / mark N/A | Farmer | Farmer **requests**; if mandatory ⇒ creates `ScheduleChangeRequest` for consultant, occurrence unchanged until approved; if non-mandatory ⇒ allowed with reason, consultant notified |
| Reschedule request | direct | via request → consultant approves/rejects |
| AI | may only create `ScheduleChangeProposal` | same |

### 8.5 `ActivityCompletion` (append-only)
`OccurrenceId`, `CompletedBy` (user), `CompletedAtUtc` (actual), `ScheduledAtUtc` (snapshot), `Notes`, `ConsultantInstructionsSnapshot`, `Outcome {Completed, Skipped, NotApplicable}`, `Source {Online, OfflineSync}`, `ClientTimestamp`, `DeviceId`. Corrections are new compensating records, never edits.
`ActivityEvidence`: `CompletionId`, `FileRef` (photo/doc), `Caption`, `CapturedAt`, `Location?` (opt-in).

### 8.6 `CropActivityHistory` (append-only audit of schedule changes)
`ActivityId`, `OccurrenceId?`, `ChangeType {Created, Edited, Rescheduled, Cancelled, Added, Removed, Completed, Skipped, ProposalApproved, ProposalRejected, FollowUpSet}`, `ActorUserId`, `ActorRole`, `Before` / `After` (jsonb snapshots), `Reason`, `Via {Direct, AiProposalApproval, FarmerRequestApproval, System}`, `At`. Mirrored to `AuditLogs`.

### 8.7 `ActivityReminder`
`OccurrenceId`, `FireAtUtc` (= occurrence start − offset), `OffsetMinutes`, `Channels[] {InApp, Push, Sms, WhatsApp}`, `Status {Pending, Processing, Sent, Failed, Cancelled}`, `Attempts`, `LockedUntil`. Rescheduling/cancelling occurrences cancels and regenerates reminders in the same transaction.

### 8.8 Smart scheduling aggregates
- `ScheduleConflict`: `OccurrenceId`, `Kind {Rain, HighWind, Heat, DiseaseRisk, StageMismatch, Overlap, Other}`, `Evidence` (weather snapshot id, rule id/version), `Severity`, `DetectedAt`, `Status {Open, Proposed, Resolved, Dismissed}`.
- `ScheduleChangeProposal` (root): `ConflictId`, `OccurrenceId`, `ProposedStartUtc?`, `Rationale: LocalizedText`, `RaisedBy {AI, System, Farmer}`, `ModelVersionId?`, `Status {Pending, Approved, Rejected, Expired}`, `DecidedBy?` (**must be the owning consultant or, for farmer-origin activities, the farmer**), `DecidedAt`, `DecisionNote`.
  - Approve ⇒ domain command `RescheduleOccurrence(actor=decider, via=AiProposalApproval)`; reject ⇒ history entry only.
  - Expires if the occurrence starts or changes first.
- `ScheduleChangeRequest`: same shape but raised by Farmer against a locked activity.

**Hard rule (enforced in `ScheduledActivity` aggregate, test-covered):** any mutation of a consultant-origin activity requires an `Actor` whose `UserId == ConsultantId` or an Admin override carrying a reason. The AI service identity has no such permission, so a silent AI change is impossible by construction.

### 8.9 Events
`ActivityScheduled`, `ActivityRescheduled`, `ActivityCancelled`, `ActivityReminderDue`, `ActivityCompleted`, `ActivityMissed`, `ScheduleConflictDetected`, `ScheduleChangeProposed`, `ScheduleChangeDecided`, `CarePlanPublished` (consumed by Notification, Diary, Analytics).

---

## 9. Consultation, Prescription, Care Plan

- `ConsultationRequest` → `Consultation` (root): `FarmerId`, `ConsultantId`, `ServiceId`, `SlotUtc`, `Mode {Chat, Call, Visit}`, `PaymentId?`, `Status {Requested, AwaitingPayment, Paid, Accepted, InProgress, Completed, Rejected, Cancelled, NoShow, Refunded}`, `ChatRoomId`, `LandId?`, `CropCycleId?`, `ConsentToShareFarmData` (snapshot of what the consultant may read). **Payment confirmed only by server-side webhook/verification event** (`PaymentSucceeded`), never by client.
- `Prescription` (root, versioned): `ConsultationId`, `FarmerId`, `LandId`, `CropCycleId`, `Diagnosis`, `Advisory`, `Items[]` (`Treatment`, `ActiveIngredient?`, `Dosage`, `Frequency`, `Duration`, `Precautions`), `FollowUpDate?`, `Status {Draft, Issued, Superseded, Revoked}`, `IssuedAt`, `PdfFileRef?`. Issued prescriptions are immutable; an edit creates a new version and supersedes the old.
- `CropCarePlan` (root, versioned): `ConsultationId?`, `ConsultantId`, `FarmerId`, `LandId`, `CropCycleId`, `PrescriptionId?`, `Status {Draft, Published, Superseded, Completed, Cancelled}`, `Items[]` (`PlanActivityDraft`: type, title, instructions, schedule/recurrence, mandatory, reminders, attachments), `FollowUpDate?`, `PublishedVersion`.
  - **Publish** = validate → freeze version N → call `ICalendarScheduling.CreateFromPlan` (idempotent on `(planId, version, itemId)`) → raise `CarePlanPublished`. Re-publish creates version N+1: diff → add/edit/cancel occurrences (only future, non-completed), each recorded in history and notified once as a batched farmer notification.
  - Consultant must hold an active relationship (`Consultation` Accepted/InProgress/Completed within retention window or explicit `ConsultantFarmerLink` with farmer consent) to read/write for that farmer.

---

## 10. Diary

`FarmDiaryEntry` (root): `FarmerId`, `LandId?`, `CropCycleId?`, `Kind {Note, Irrigation, Fertilizer, PestObservation, DiseaseObservation, Weather, FarmActivity, Harvest}`, `Text`, `Attachments[]`, `OccurredAt`, `Source {Manual, ActivityCompletion, Scan}`, `SourceRef?`, `ClientId` (idempotent offline create), `IsPrivate` (default true; consultant visibility only per consent).
Auto-entry: handler on `ActivityCompleted` creates an entry when the farmer's setting `autoDiary=true` (default true).

---

## 11. AI lineage aggregates touched by the spine

- `AIInferenceLog` (append-only): `ModelVersionId`, `Kind`, `InputMetadata` (no raw PII), `Output`, `Confidence`, `UserId`, `CropId?`, `CoarseLocation?`, `At`, `LatencyMs`, `Feedback?`.
- `AIRecommendation`: `InferenceLogId`, `FarmerId`, `CropCycleId?`, `Kind`, `Content: LocalizedText`, `Sources[]`, `RiskLevel`, `EscalationRequired`, `Disclaimer`, `Status {Shown, Dismissed, Accepted}`. "Accept" can create a farmer-origin `ScheduledActivity` (`OriginType=AiAccepted`, `AiRecommendationId` set) — it never creates a consultant-origin one.
- `DiseaseScan` / `DiseaseResult`: result `Status {Confident, NeedsExpertReview, Failed}`; threshold read from `AIModelVersion.ConfidenceThreshold`.
- `AIFeedback` (farmer yes/no, consultant confirmation) → `TrainingCandidate {Proposed, Approved, Rejected}`; only AIDataAdmin may approve; no auto-training.

---

## 12. Cross-aggregate relationship summary

```
FarmerProfile 1─* Farm 1─* Land 1─* CropCycle *─1 Crop/Variety
CropCycle 1─* StageHistory | HarvestRecord | DiseaseScan | SoilReport | ScheduledActivity | DiaryEntry | Expense | Income
Consultation 1─* Prescription(v)        Consultation 1─* CropCarePlan(v)
CropCarePlan 1─* PlanActivityDraft ──publish──> ScheduledActivity(OriginType=Consultant, CarePlanId, PrescriptionId?)
ScheduledActivity 1─* ActivityOccurrence 1─0..1 ActivityCompletion 1─* ActivityEvidence
ActivityOccurrence 1─* ActivityReminder | ScheduleConflict 1─* ScheduleChangeProposal
ScheduledActivity 1─* CropActivityHistory (append-only)
```

---

## 13. Domain services (pure, no I/O)
`RecurrenceExpander` · `OccurrenceStateMachine` · `ScheduleAuthorizationPolicy` (the lock rule above) · `AreaConverter` · `StageTransitionPolicy` · `PlanDiffer` (version N vs N+1 → add/edit/cancel set) · `ProfitabilityCalculator` (later phase).

---

## 14. Permissions introduced (seed in Phase 5)
`farm.land.{read,write}` · `crop.cycle.{read,write}` · `calendar.activity.{read,create,edit,reschedule,cancel,complete,skip}` · `calendar.proposal.{read,decide}` · `careplan.{read,create,edit,publish}` · `prescription.{read,create,edit,issue}` · `diary.{read,write}` · `farmer.govid.reveal` · `consultant.verify` — each handler additionally performs **object-level** checks (owner / active relationship / org).

## 15. Edge cases (must have tests)
- Recurrence: DST-free IST vs future multi-zone; `Count` and `Until` both given (reject); weekly with empty weekdays (defaults to start weekday); series edit "this and following" splits the rule; horizon rollover doesn't duplicate occurrences (unique `(ActivityId, SeqNo)`).
- Reschedule onto a past time (reject); reschedule an occurrence already `IN_PROGRESS`; concurrent consultant reschedule vs farmer completion (optimistic concurrency; completion wins, reschedule rejected with 409).
- Cancel a plan with completed occurrences (history preserved; only future cancelled).
- Care-plan republish while farmer offline-completed an occurrence (completion fact kept; occurrence not cancelled).
- Offline completion arrives after consultant cancelled the occurrence (accepted as completion-with-note "after cancellation", flagged for consultant).
- Proposal approved after weather changed (expiry/recheck rule); two proposals for same occurrence (latest supersedes).
- Crop cycle closed with pending occurrences (auto-cancel future, reminders cancelled).
- Consultant relationship expired between draft and publish (publish rejected).
- Duplicate publish call (idempotent by plan+version+item).
- Farmer deletes account (anonymise; retain consultant-signed prescriptions per retention policy).

## 16. API contracts and screens (detailed in Phase 4 / feature phases)
Routes: `crop-calendar`, `crop-activities`, `crop-care-plans`, `prescriptions`, `consultations` per Phase 1 §F. Screens: Farmer Crop Calendar (icon + label), Activity detail/complete, Consultant Calendar (Day/Week/Month, drag-and-drop → `reschedule`), Care Plan builder, Proposal inbox. Mobile: Home → Today's Activities, My Farm → Crop Calendar/Care Plan (offline).

## 17. Test plan for this domain (unit-level, pure)
1. Recurrence property tests (no duplicates, ordering, count/until bounds, weekday filters).
2. State-machine table test: every (state, action) pair → allowed/denied.
3. `ScheduleAuthorizationPolicy`: farmer/AI/other-consultant cannot mutate consultant-origin; owner consultant and Admin-with-reason can.
4. Plan publish idempotency and version diff.
5. Invariant tests for Land/Boundary, CropCycle, Prescription immutability.
6. Architecture tests: Domain has no infrastructure dependencies; Ads ↔ AI no references; modules reference each other only via `*.Contracts`.

## 18. Open decisions to confirm before Phase 3 (defaults used if none given)
| # | Question | Default |
|---|---|---|
| D1 | Farmer-origin activities: allow farmers to create their own recurring activities? | Yes |
| D2 | Mandatory consultant activity skip: block or request? | Request (consultant confirms) |
| D3 | Occurrence materialisation horizon | 90 days rolling |
| D4 | Consultant access to farmer data after consultation ends | 90 days, renewable by farmer consent |
| D5 | Who decides AI proposals on farmer-origin activities | Farmer |
