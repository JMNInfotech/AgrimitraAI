-- V005: consultants, consultations, prescriptions (separate from care plans), consultant crop care plans
create table consultation_services (
    id uuid primary key default gen_random_uuid(),
    consultant_id uuid not null references consultant_profiles(id),
    name text not null,
    description text,
    mode text not null check (mode in ('chat','call','video','farm_visit')),
    duration_minutes int not null check (duration_minutes between 5 and 480),
    fee numeric(12,2) not null check (fee >= 0),
    currency char(3) not null default 'INR',
    is_active boolean not null default true,
    like tpl.audit_sd including defaults
);
create index ix_consultation_services_consultant on consultation_services (consultant_id) where not is_deleted;

create table consultant_expertise (
    id uuid primary key default gen_random_uuid(),
    consultant_id uuid not null references consultant_profiles(id) on delete cascade,
    crop_id uuid references crops(id),
    crop_disease_id uuid references crop_diseases(id),
    topic text,
    like tpl.audit including defaults,
    check (crop_id is not null or crop_disease_id is not null or topic is not null)
);
create index ix_consultant_expertise_consultant on consultant_expertise (consultant_id);
create index ix_consultant_expertise_crop on consultant_expertise (crop_id);
create index ix_consultant_expertise_disease on consultant_expertise (crop_disease_id);

create table consultant_availability (
    id uuid primary key default gen_random_uuid(),
    consultant_id uuid not null references consultant_profiles(id) on delete cascade,
    day_of_week smallint check (day_of_week between 0 and 6),
    specific_date date,
    start_time time not null,
    end_time time not null,
    time_zone_id text not null default 'Asia/Kolkata',
    is_unavailable boolean not null default false,
    valid_from date,
    valid_until date,
    like tpl.audit including defaults,
    check (end_time > start_time),
    check ((day_of_week is not null) <> (specific_date is not null)),
    check (valid_until is null or valid_from is null or valid_until >= valid_from)
);
create index ix_consultant_availability_consultant on consultant_availability (consultant_id, day_of_week);
create index ix_consultant_availability_date on consultant_availability (consultant_id, specific_date) where specific_date is not null;

create table consultation_requests (
    id uuid primary key default gen_random_uuid(),
    farmer_profile_id uuid not null references farmer_profiles(id),
    consultant_id uuid not null references consultant_profiles(id),
    service_id uuid not null references consultation_services(id),
    land_id uuid references lands(id),
    crop_cycle_id uuid references crop_cycles(id),
    disease_scan_id uuid,
    requested_start_at timestamptz not null,
    problem_description text,
    share_farm_data_consent boolean not null default false,
    status text not null default 'requested' check (status in ('requested','awaiting_payment','paid','accepted','rejected','cancelled','expired')),
    decided_at timestamptz,
    rejection_reason text,
    like tpl.audit_sd including defaults
);
create index ix_consultation_requests_consultant on consultation_requests (consultant_id, status, requested_start_at);
create index ix_consultation_requests_farmer on consultation_requests (farmer_profile_id, created_at desc);
create index ix_consultation_requests_service on consultation_requests (service_id);
create index ix_consultation_requests_land on consultation_requests (land_id);
create index ix_consultation_requests_cycle on consultation_requests (crop_cycle_id);

create table consultations (
    id uuid primary key default gen_random_uuid(),
    request_id uuid not null unique references consultation_requests(id),
    farmer_profile_id uuid not null references farmer_profiles(id),
    consultant_id uuid not null references consultant_profiles(id),
    service_id uuid not null references consultation_services(id),
    land_id uuid references lands(id),
    crop_cycle_id uuid references crop_cycles(id),
    payment_id uuid,
    chat_room_id uuid,
    status text not null default 'accepted' check (status in ('accepted','in_progress','completed','cancelled','no_show','refunded')),
    fee_amount numeric(12,2) not null check (fee_amount >= 0),
    currency char(3) not null default 'INR',
    started_at timestamptz,
    completed_at timestamptz,
    data_access_expires_at timestamptz,
    summary text,
    like tpl.audit_sd including defaults,
    check (completed_at is null or started_at is null or completed_at >= started_at)
);
create index ix_consultations_consultant on consultations (consultant_id, status);
create index ix_consultations_farmer on consultations (farmer_profile_id, created_at desc);
create index ix_consultations_service on consultations (service_id);
create index ix_consultations_land on consultations (land_id);
create index ix_consultations_cycle on consultations (crop_cycle_id);
create index ix_consultations_access on consultations (data_access_expires_at) where data_access_expires_at is not null;

create table consultation_appointments (
    id uuid primary key default gen_random_uuid(),
    consultation_id uuid not null references consultations(id) on delete cascade,
    start_at timestamptz not null,
    end_at timestamptz not null,
    mode text not null check (mode in ('chat','call','video','farm_visit')),
    location geography(Point,4326),
    meeting_url text,
    status text not null default 'scheduled' check (status in ('scheduled','completed','cancelled','rescheduled','no_show')),
    reminder_sent_at timestamptz,
    like tpl.audit including defaults,
    check (end_at > start_at)
);
create index ix_appointments_consultation on consultation_appointments (consultation_id);
create index ix_appointments_start on consultation_appointments (start_at) where status = 'scheduled';

create table consultant_farmer_links (
    id uuid primary key default gen_random_uuid(),
    consultant_id uuid not null references consultant_profiles(id),
    farmer_profile_id uuid not null references farmer_profiles(id),
    consultation_id uuid references consultations(id),
    scope text not null default 'crop_care' check (scope in ('crop_care','read_only','full')),
    granted_at timestamptz not null default now(),
    expires_at timestamptz,
    revoked_at timestamptz,
    like tpl.audit including defaults,
    check (expires_at is null or expires_at > granted_at)
);
create index ix_consultant_links_consultant on consultant_farmer_links (consultant_id) where revoked_at is null;
create index ix_consultant_links_farmer on consultant_farmer_links (farmer_profile_id) where revoked_at is null;
create unique index ux_consultant_links_active on consultant_farmer_links (consultant_id, farmer_profile_id, scope) where revoked_at is null;

create table consultant_reviews (
    id uuid primary key default gen_random_uuid(),
    consultation_id uuid not null unique references consultations(id),
    consultant_id uuid not null references consultant_profiles(id),
    farmer_profile_id uuid not null references farmer_profiles(id),
    rating smallint not null check (rating between 1 and 5),
    comment text,
    status text not null default 'pending' check (status in ('pending','approved','rejected','hidden')),
    moderated_by uuid references users(id),
    moderated_at timestamptz,
    consultant_reply text,
    like tpl.audit_sd including defaults
);
create index ix_consultant_reviews_consultant on consultant_reviews (consultant_id, status);
create index ix_consultant_reviews_farmer on consultant_reviews (farmer_profile_id);

-- Prescriptions: immutable once issued; edits create a new version row that supersedes the old.
create table prescriptions (
    id uuid primary key default gen_random_uuid(),
    prescription_number text not null unique,
    consultation_id uuid references consultations(id),
    consultant_id uuid not null references consultant_profiles(id),
    farmer_profile_id uuid not null references farmer_profiles(id),
    land_id uuid references lands(id),
    crop_cycle_id uuid references crop_cycles(id),
    version int not null default 1 check (version > 0),
    supersedes_id uuid references prescriptions(id),
    diagnosis text,
    advisory text,
    precautions text,
    status text not null default 'draft' check (status in ('draft','issued','superseded','revoked')),
    issued_at timestamptz,
    pdf_file_object_id uuid references file_objects(id),
    follow_up_date date,
    like tpl.audit_sd including defaults,
    check (status <> 'issued' or issued_at is not null)
);
create index ix_prescriptions_farmer on prescriptions (farmer_profile_id, created_at desc) where not is_deleted;
create index ix_prescriptions_consultant on prescriptions (consultant_id, status);
create index ix_prescriptions_consultation on prescriptions (consultation_id);
create index ix_prescriptions_land on prescriptions (land_id);
create index ix_prescriptions_cycle on prescriptions (crop_cycle_id);
create index ix_prescriptions_supersedes on prescriptions (supersedes_id);
create index ix_prescriptions_followup on prescriptions (follow_up_date) where follow_up_date is not null;

create table prescription_items (
    id uuid primary key default gen_random_uuid(),
    prescription_id uuid not null references prescriptions(id) on delete cascade,
    line_no int not null check (line_no > 0),
    treatment text not null,
    active_ingredient text,
    dosage text,
    dosage_value numeric(12,3),
    dosage_unit text,
    frequency text,
    duration_days int check (duration_days is null or duration_days > 0),
    application_method text,
    precautions text,
    like tpl.audit including defaults,
    unique (prescription_id, line_no)
);

create table prescription_attachments (
    id uuid primary key default gen_random_uuid(),
    prescription_id uuid not null references prescriptions(id) on delete cascade,
    file_object_id uuid not null references file_objects(id),
    caption text,
    like tpl.audit including defaults
);
create index ix_prescription_attachments_rx on prescription_attachments (prescription_id);

create table prescription_follow_ups (
    id uuid primary key default gen_random_uuid(),
    prescription_id uuid not null references prescriptions(id) on delete cascade,
    follow_up_date date not null,
    status text not null default 'pending' check (status in ('pending','done','missed','cancelled')),
    notes text,
    completed_at timestamptz,
    like tpl.audit including defaults
);
create index ix_prescription_followups_rx on prescription_follow_ups (prescription_id);
create index ix_prescription_followups_due on prescription_follow_ups (follow_up_date) where status = 'pending';

-- Consultant crop care plans: separate from prescriptions, versioned on publish.
create table consultant_crop_care_plans (
    id uuid primary key default gen_random_uuid(),
    consultation_id uuid references consultations(id),
    consultant_id uuid not null references consultant_profiles(id),
    farmer_profile_id uuid not null references farmer_profiles(id),
    land_id uuid not null references lands(id),
    crop_id uuid not null references crops(id),
    crop_cycle_id uuid not null references crop_cycles(id),
    prescription_id uuid references prescriptions(id),
    title text not null,
    description text,
    status text not null default 'draft' check (status in ('draft','published','superseded','completed','cancelled')),
    published_version int not null default 0 check (published_version >= 0),
    published_at timestamptz,
    follow_up_date date,
    like tpl.audit_sd including defaults,
    check (status <> 'published' or published_at is not null)
);
create index ix_care_plans_consultant on consultant_crop_care_plans (consultant_id, status) where not is_deleted;
create index ix_care_plans_farmer on consultant_crop_care_plans (farmer_profile_id, status) where not is_deleted;
create index ix_care_plans_land on consultant_crop_care_plans (land_id);
create index ix_care_plans_crop on consultant_crop_care_plans (crop_id);
create index ix_care_plans_cycle on consultant_crop_care_plans (crop_cycle_id);
create index ix_care_plans_consultation on consultant_crop_care_plans (consultation_id);
create index ix_care_plans_prescription on consultant_crop_care_plans (prescription_id);

-- Plan items (authoring model). On publish they materialise into crop_calendar_activities (FK added in V006).
create table consultant_schedule_activities (
    id uuid primary key default gen_random_uuid(),
    care_plan_id uuid not null references consultant_crop_care_plans(id) on delete cascade,
    calendar_activity_id uuid,
    plan_version int not null default 0 check (plan_version >= 0),
    activity_type text not null check (activity_type in ('irrigation','fertilizer','pesticide','fungicide','herbicide','pest_monitoring','disease_inspection','soil_testing','pruning','weeding','planting','harvesting','machinery_usage','consultant_follow_up','lab_test','other')),
    title text not null,
    description text,
    instructions text,
    priority text not null default 'normal' check (priority in ('low','normal','high','critical')),
    is_mandatory boolean not null default false,
    start_date date not null,
    start_time time,
    end_time time,
    time_zone_id text not null default 'Asia/Kolkata',
    recurrence_frequency text check (recurrence_frequency in ('daily','weekly','every_n_days')),
    recurrence_interval int check (recurrence_interval is null or recurrence_interval between 1 and 365),
    recurrence_weekdays smallint[] check (recurrence_weekdays is null or recurrence_weekdays <@ array[0,1,2,3,4,5,6]::smallint[]),
    recurrence_count int check (recurrence_count is null or recurrence_count between 1 and 1000),
    recurrence_until date,
    reminder_offsets_minutes int[] not null default '{1440,60}',
    prescription_item_id uuid references prescription_items(id),
    status text not null default 'draft' check (status in ('draft','published','changed','removed')),
    like tpl.audit_sd including defaults,
    check (end_time is null or (start_time is not null and end_time > start_time)),
    check ((recurrence_frequency is null) = (recurrence_interval is null)),
    check (recurrence_count is null or recurrence_until is null),
    check (recurrence_weekdays is null or recurrence_frequency = 'weekly'),
    check (recurrence_until is null or recurrence_until >= start_date)
);
create index ix_schedule_activities_plan on consultant_schedule_activities (care_plan_id, start_date) where not is_deleted;
create index ix_schedule_activities_calendar on consultant_schedule_activities (calendar_activity_id);

create table consultant_schedule_history (
    id uuid not null default gen_random_uuid() primary key,
    care_plan_id uuid not null references consultant_crop_care_plans(id),
    schedule_activity_id uuid references consultant_schedule_activities(id),
    change_type text not null check (change_type in ('plan_created','plan_published','plan_republished','plan_cancelled','activity_added','activity_edited','activity_removed','activity_rescheduled','activity_cancelled','follow_up_set')),
    plan_version int,
    actor_user_id uuid not null references users(id),
    actor_role text not null,
    reason text,
    old_value jsonb,
    new_value jsonb,
    created_at timestamptz not null default now()
);
create index ix_schedule_history_plan on consultant_schedule_history (care_plan_id, created_at desc);
create index ix_schedule_history_activity on consultant_schedule_history (schedule_activity_id, created_at desc);
create index ix_schedule_history_actor on consultant_schedule_history (actor_user_id);
