-- V006: crop calendar & activity scheduling engine
create table crop_calendar_plans (
    id uuid primary key default gen_random_uuid(),
    farmer_profile_id uuid not null references farmer_profiles(id),
    land_id uuid not null references lands(id),
    crop_cycle_id uuid not null references crop_cycles(id),
    care_plan_id uuid references consultant_crop_care_plans(id),
    name text not null,
    source text not null check (source in ('system_template','farmer','consultant')),
    status text not null default 'active' check (status in ('draft','active','completed','cancelled')),
    start_date date,
    end_date date,
    like tpl.audit_sd including defaults,
    check (end_date is null or start_date is null or end_date >= start_date)
);
create index ix_calendar_plans_farmer on crop_calendar_plans (farmer_profile_id, status) where not is_deleted;
create index ix_calendar_plans_cycle on crop_calendar_plans (crop_cycle_id);
create index ix_calendar_plans_land on crop_calendar_plans (land_id);
create index ix_calendar_plans_care_plan on crop_calendar_plans (care_plan_id);

create table crop_calendar_activities (
    id uuid primary key default gen_random_uuid(),
    calendar_plan_id uuid references crop_calendar_plans(id),
    farmer_profile_id uuid not null references farmer_profiles(id),
    land_id uuid not null references lands(id),
    crop_id uuid references crops(id),
    crop_cycle_id uuid references crop_cycles(id),
    consultant_id uuid references consultant_profiles(id),
    care_plan_id uuid references consultant_crop_care_plans(id),
    care_plan_version int,
    prescription_id uuid references prescriptions(id),
    ai_recommendation_id uuid,
    machinery_id uuid,
    origin text not null check (origin in ('consultant','farmer','ai_accepted','machinery','system')),
    activity_type text not null check (activity_type in ('irrigation','fertilizer','pesticide','fungicide','herbicide','pest_monitoring','disease_inspection','soil_testing','pruning','weeding','planting','harvesting','machinery_usage','consultant_follow_up','lab_test','other')),
    title text not null,
    description text,
    instructions text,
    priority text not null default 'normal' check (priority in ('low','normal','high','critical')),
    is_mandatory boolean not null default false,
    assigned_by_user_id uuid references users(id),
    start_date date not null,
    start_time time,
    end_time time,
    time_zone_id text not null default 'Asia/Kolkata',
    status text not null default 'scheduled' check (status in ('planned','scheduled','upcoming','in_progress','completed','skipped','cancelled','rescheduled')),
    completion_date timestamptz,
    completion_notes text,
    is_locked_by_consultant boolean generated always as (origin = 'consultant') stored,
    reminder_offsets_minutes int[] not null default '{1440,60}',
    like tpl.audit_sd including defaults,
    check (end_time is null or (start_time is not null and end_time > start_time)),
    check (origin <> 'consultant' or (consultant_id is not null and assigned_by_user_id is not null)),
    check (status <> 'completed' or completion_date is not null)
);
create index ix_calendar_activities_farmer_date on crop_calendar_activities (farmer_profile_id, start_date) where not is_deleted;
create index ix_calendar_activities_land_date on crop_calendar_activities (land_id, start_date) where not is_deleted;
create index ix_calendar_activities_cycle on crop_calendar_activities (crop_cycle_id, start_date);
create index ix_calendar_activities_consultant_date on crop_calendar_activities (consultant_id, start_date) where not is_deleted;
create index ix_calendar_activities_crop on crop_calendar_activities (crop_id);
create index ix_calendar_activities_plan on crop_calendar_activities (calendar_plan_id);
create index ix_calendar_activities_care_plan on crop_calendar_activities (care_plan_id);
create index ix_calendar_activities_rx on crop_calendar_activities (prescription_id);
create index ix_calendar_activities_status on crop_calendar_activities (status, start_date) where not is_deleted;
create index ix_calendar_activities_type on crop_calendar_activities (activity_type);

alter table consultant_schedule_activities
    add constraint fk_schedule_activities_calendar foreign key (calendar_activity_id) references crop_calendar_activities(id);
create unique index ux_schedule_activities_calendar on consultant_schedule_activities (calendar_activity_id, plan_version) where calendar_activity_id is not null;

create table activity_recurrence_rules (
    id uuid primary key default gen_random_uuid(),
    activity_id uuid not null unique references crop_calendar_activities(id) on delete cascade,
    frequency text not null check (frequency in ('daily','weekly','every_n_days')),
    interval_value int not null default 1 check (interval_value between 1 and 365),
    by_weekday smallint[] check (by_weekday is null or by_weekday <@ array[0,1,2,3,4,5,6]::smallint[]),
    occurrence_count int check (occurrence_count is null or occurrence_count between 1 and 1000),
    until_date date,
    materialized_until timestamptz,
    like tpl.audit including defaults,
    check (occurrence_count is null or until_date is null),
    check (by_weekday is null or frequency = 'weekly')
);
create index ix_recurrence_materialize on activity_recurrence_rules (materialized_until);

create table crop_activity_occurrences (
    id uuid primary key default gen_random_uuid(),
    activity_id uuid not null references crop_calendar_activities(id) on delete cascade,
    seq_no int not null check (seq_no >= 0),
    revision int not null default 0 check (revision >= 0),
    scheduled_start_at timestamptz not null,
    scheduled_end_at timestamptz,
    status text not null default 'scheduled' check (status in ('planned','scheduled','upcoming','in_progress','completed','skipped','cancelled','rescheduled')),
    rescheduled_to_id uuid references crop_activity_occurrences(id),
    override_instructions text,
    like tpl.audit including defaults,
    unique (activity_id, seq_no, revision),
    check (scheduled_end_at is null or scheduled_end_at > scheduled_start_at),
    check (status <> 'rescheduled' or rescheduled_to_id is not null)
);
create index ix_occurrences_activity on crop_activity_occurrences (activity_id, scheduled_start_at);
create index ix_occurrences_start on crop_activity_occurrences (scheduled_start_at) where status in ('scheduled','upcoming','in_progress');
create index ix_occurrences_rescheduled_to on crop_activity_occurrences (rescheduled_to_id);

create table crop_activity_reminders (
    id uuid primary key default gen_random_uuid(),
    occurrence_id uuid not null references crop_activity_occurrences(id) on delete cascade,
    offset_minutes int not null check (offset_minutes >= 0),
    fire_at timestamptz not null,
    channels text[] not null default '{in_app,push}' check (channels <@ array['in_app','push','email','sms','whatsapp']::text[] and cardinality(channels) > 0),
    status text not null default 'pending' check (status in ('pending','processing','sent','failed','cancelled')),
    attempts int not null default 0 check (attempts >= 0),
    locked_until timestamptz,
    sent_at timestamptz,
    last_error text,
    like tpl.audit including defaults,
    unique (occurrence_id, offset_minutes)
);
create index ix_reminders_due on crop_activity_reminders (fire_at) where status in ('pending','processing');

-- Append-only record of what actually happened
create table crop_activity_completions (
    id uuid primary key default gen_random_uuid(),
    occurrence_id uuid not null references crop_activity_occurrences(id),
    completed_by uuid not null references users(id),
    outcome text not null check (outcome in ('completed','skipped','not_applicable')),
    completed_at timestamptz not null,
    scheduled_at timestamptz not null,
    notes text,
    instructions_snapshot text,
    source text not null default 'online' check (source in ('online','offline_sync')),
    client_timestamp timestamptz,
    device_id text,
    client_mutation_id uuid unique,
    created_at timestamptz not null default now()
);
create unique index ux_completions_occurrence on crop_activity_completions (occurrence_id) where outcome = 'completed';
create index ix_completions_user on crop_activity_completions (completed_by, completed_at desc);

create table crop_activity_evidence (
    id uuid primary key default gen_random_uuid(),
    completion_id uuid references crop_activity_completions(id),
    occurrence_id uuid not null references crop_activity_occurrences(id),
    file_object_id uuid not null references file_objects(id),
    caption text,
    captured_at timestamptz,
    location geography(Point,4326),
    uploaded_by uuid not null references users(id),
    like tpl.audit_sd including defaults
);
create index ix_evidence_occurrence on crop_activity_evidence (occurrence_id) where not is_deleted;
create index ix_evidence_completion on crop_activity_evidence (completion_id);

-- Append-only audit of every schedule change
create table crop_activity_history (
    id uuid primary key default gen_random_uuid(),
    activity_id uuid not null references crop_calendar_activities(id),
    occurrence_id uuid references crop_activity_occurrences(id),
    change_type text not null check (change_type in ('created','edited','rescheduled','cancelled','completed','skipped','proposal_approved','proposal_rejected','follow_up_set','added','removed')),
    actor_user_id uuid not null references users(id),
    actor_role text not null,
    via text not null default 'direct' check (via in ('direct','ai_proposal_approval','farmer_request_approval','system')),
    reason text,
    before_value jsonb,
    after_value jsonb,
    created_at timestamptz not null default now()
);
create index ix_activity_history_activity on crop_activity_history (activity_id, created_at desc);
create index ix_activity_history_occurrence on crop_activity_history (occurrence_id);
create index ix_activity_history_actor on crop_activity_history (actor_user_id, created_at desc);

create table schedule_conflicts (
    id uuid primary key default gen_random_uuid(),
    occurrence_id uuid not null references crop_activity_occurrences(id) on delete cascade,
    kind text not null check (kind in ('rain','high_wind','heat','disease_risk','stage_mismatch','overlap','other')),
    severity text not null check (severity in ('info','warning','critical')),
    rule_code text,
    rule_version text,
    evidence jsonb not null default '{}',
    weather_data_id uuid,
    status text not null default 'open' check (status in ('open','proposed','resolved','dismissed')),
    detected_at timestamptz not null default now(),
    resolved_at timestamptz,
    like tpl.audit including defaults
);
create index ix_conflicts_occurrence on schedule_conflicts (occurrence_id);
create index ix_conflicts_open on schedule_conflicts (detected_at) where status in ('open','proposed');

-- AI/system/farmer suggestions to move an occurrence. Never changes a schedule by itself.
create table schedule_change_proposals (
    id uuid primary key default gen_random_uuid(),
    activity_id uuid not null references crop_calendar_activities(id),
    occurrence_id uuid not null references crop_activity_occurrences(id),
    conflict_id uuid references schedule_conflicts(id),
    raised_by text not null check (raised_by in ('ai','system','farmer')),
    raised_by_user_id uuid references users(id),
    proposed_start_at timestamptz not null,
    rationale text not null,
    model_version_id uuid,
    decider_user_id uuid references users(id),
    status text not null default 'pending' check (status in ('pending','approved','rejected','expired')),
    decided_by uuid references users(id),
    decided_at timestamptz,
    decision_note text,
    expires_at timestamptz,
    like tpl.audit including defaults,
    check (status = 'pending' or decided_at is not null or status = 'expired')
);
create index ix_proposals_activity on schedule_change_proposals (activity_id);
create index ix_proposals_decider_pending on schedule_change_proposals (decider_user_id, created_at) where status = 'pending';
create index ix_proposals_occurrence on schedule_change_proposals (occurrence_id);
create index ix_proposals_conflict on schedule_change_proposals (conflict_id);
create unique index ux_proposals_one_pending on schedule_change_proposals (occurrence_id) where status = 'pending';
