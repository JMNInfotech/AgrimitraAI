# AGRIMITRA — Architecture Baseline (Phase 0/1 summary)

Authoritative summary of decisions approved at the end of Phase 1. Full Phase 1 write-up (A–P) was delivered in the design conversation; this file records the binding decisions.

## Style
Modular monolith (.NET) + separately deployed Python AI plane + .NET worker host. Modules talk through in-process contracts and a transactional-outbox event bus (RabbitMQ). Boundaries are enforced by architecture tests so any module can be extracted later.

## Binding principles
1. One domain spine: Farmer → Farm → Land → CropCycle → Stage → Plan → Activity → Evidence → Diary → Analytics.
2. Six lineage concepts are separate aggregates and never merged:
   `AIRecommendation ≠ Prescription ≠ CropCarePlan ≠ ScheduledActivity ≠ CompletedActivity ≠ ActivityEvidence`.
3. AI is advisory. It writes only to `AIRecommendation`, `ScheduleChangeProposal`, `AIInferenceLog`.
4. Advertising and AI recommendation share no tables, services or ranking features (enforced by architecture test).
5. Binary data lives only in object storage; Postgres holds metadata.
6. Tenant-ready: nullable `organization_id` + global query-filter hook (no-op in MVP).
7. Reminders are DB-driven (worker + `SKIP LOCKED`), never client timers.

## Assumptions (change by ADR)
| # | Assumption |
|---|---|
| A1 | Latest .NET LTS at scaffold time (verify; .NET 10 expected) |
| A2 | Lightweight in-house mediator (no licensed library) |
| A3 | RabbitMQ as broker |
| A4 | Farmers: mobile + OTP primary; password optional. Partners/admins: email + password (+MFA) |
| A5 | Razorpay as first payment adapter behind `IPaymentGateway` |
| A6 | React Native with native modules (Expo prebuild or RN CLI), decided in Phase 21 |
| A7 | Government ID stored only as encrypted/tokenized reference, never raw Aadhaar number unless legally required |
| A8 | SMS/WhatsApp providers behind interfaces (e.g. MSG91, WhatsApp Business API) |
| A9 | Cloud-agnostic until Phase 25 |
| A10 | LLM/STT/TTS vendors behind interfaces; chosen after Marathi/Hindi evaluation in Phase 14 |
