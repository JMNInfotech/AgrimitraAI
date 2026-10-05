-- V012: advertising (isolated from AI), knowledge/RAG, support, audit, platform tables
create table advertisers (
    id uuid primary key default gen_random_uuid(),
    advertiser_profile_id uuid not null unique references advertiser_profiles(id),
    display_name text not null,
    status text not null default 'active' check (status in ('active','suspended','closed')),
    currency char(3) not null default 'INR',
    like tpl.audit_sd including defaults
);

create table ad_campaigns (
    id uuid primary key default gen_random_uuid(),
    advertiser_id uuid not null references advertisers(id),
    name text not null,
    objective text check (objective in ('awareness','traffic','conversions','seasonal')),
    status text not null default 'draft' check (status in ('draft','pending_approval','approved','rejected','scheduled','active','paused','completed','expired')),
    pricing_model text not null check (pricing_model in ('cpm','cpc','fixed_placement','campaign_package')),
    bid_amount numeric(12,4) check (bid_amount is null or bid_amount >= 0),
    total_budget numeric(14,2) not null check (total_budget >= 0),
    daily_budget numeric(14,2) check (daily_budget is null or daily_budget >= 0),
    spent_amount numeric(14,2) not null default 0 check (spent_amount >= 0),
    currency char(3) not null default 'INR',
    start_at timestamptz not null,
    end_at timestamptz not null,
    frequency_cap_per_day int check (frequency_cap_per_day is null or frequency_cap_per_day > 0),
    submitted_at timestamptz,
    approved_by uuid references users(id),
    approved_at timestamptz,
    rejection_reason text,
    like tpl.audit_sd including defaults,
    check (end_at > start_at),
    check (spent_amount <= total_budget),
    check (status not in ('approved','scheduled','active') or approved_at is not null),
    check (pricing_model not in ('cpm','cpc') or bid_amount is not null)
);
create index ix_ad_campaigns_advertiser on ad_campaigns (advertiser_id, status) where not is_deleted;
create index ix_ad_campaigns_active_window on ad_campaigns (start_at, end_at) where status in ('scheduled','active');
create index ix_ad_campaigns_approval on ad_campaigns (submitted_at) where status = 'pending_approval';

create table ad_creatives (
    id uuid primary key default gen_random_uuid(),
    campaign_id uuid not null references ad_campaigns(id) on delete cascade,
    ad_type text not null check (ad_type in ('product','service','nursery','laboratory','consultant_promotion','seasonal','sponsored_recommendation','banner','in_feed','location_based')),
    title text not null,
    description text,
    product_id uuid references products(id),
    category_id uuid references product_categories(id),
    image_file_id uuid references file_objects(id),
    video_file_id uuid references file_objects(id),
    cta_label text,
    landing_url text check (landing_url is null or landing_url ~ '^https://'),
    language_code text references languages(code),
    sponsored_label text not null default 'Sponsored' check (length(btrim(sponsored_label)) > 0),
    status text not null default 'draft' check (status in ('draft','pending_approval','approved','rejected','paused')),
    reviewed_by uuid references users(id),
    reviewed_at timestamptz,
    like tpl.audit_sd including defaults
);
create index ix_ad_creatives_campaign on ad_creatives (campaign_id) where not is_deleted;
create index ix_ad_creatives_product on ad_creatives (product_id);
create index ix_ad_creatives_category on ad_creatives (category_id);

create table ad_placements (
    id uuid primary key default gen_random_uuid(),
    code text not null unique check (code in ('home_banner','marketplace_banner','crop_page','product_page','ai_result_page','farm_dashboard','search_results','sponsored_product_section')),
    name text not null,
    allowed_ad_types text[] not null default '{}',
    max_items int not null default 1 check (max_items > 0),
    base_price_cpm numeric(12,4),
    is_active boolean not null default true,
    like tpl.audit including defaults
);

create table ad_campaign_placements (
    campaign_id uuid not null references ad_campaigns(id) on delete cascade,
    placement_id uuid not null references ad_placements(id),
    priority int not null default 0,
    primary key (campaign_id, placement_id)
);
create index ix_ad_campaign_placements_placement on ad_campaign_placements (placement_id);

-- Targeting uses coarse buckets only; no farmer identifiers exist in advertising tables.
create table ad_targeting (
    id uuid primary key default gen_random_uuid(),
    campaign_id uuid not null unique references ad_campaigns(id) on delete cascade,
    state_ids uuid[] not null default '{}',
    district_ids uuid[] not null default '{}',
    taluka_ids uuid[] not null default '{}',
    center_location geography(Point,4326),
    radius_km numeric(8,2) check (radius_km is null or radius_km > 0),
    crop_ids uuid[] not null default '{}',
    crop_stage_codes text[] not null default '{}',
    farmer_segments text[] not null default '{}' check (farmer_segments <@ array['marginal','small','medium','large']::text[]),
    language_codes text[] not null default '{}',
    seasons text[] not null default '{}' check (seasons <@ array['kharif','rabi','summer','perennial','year_round']::text[]),
    farm_size_min_sq_m numeric(14,2) check (farm_size_min_sq_m is null or farm_size_min_sq_m >= 0),
    farm_size_max_sq_m numeric(14,2),
    interest_category_ids uuid[] not null default '{}',
    like tpl.audit including defaults,
    check ((center_location is null) = (radius_km is null)),
    check (farm_size_max_sq_m is null or farm_size_min_sq_m is null or farm_size_max_sq_m >= farm_size_min_sq_m)
);
create index ix_ad_targeting_districts on ad_targeting using gin (district_ids);
create index ix_ad_targeting_crops on ad_targeting using gin (crop_ids);
create index ix_ad_targeting_center on ad_targeting using gist (center_location);

create table ad_impressions (
    id uuid not null default gen_random_uuid(),
    campaign_id uuid not null references ad_campaigns(id),
    creative_id uuid not null references ad_creatives(id),
    placement_id uuid not null references ad_placements(id),
    viewer_hash bytea not null,
    district_id uuid references districts(id),
    language_code text,
    cost numeric(12,6) not null default 0 check (cost >= 0),
    occurred_at timestamptz not null default now(),
    primary key (id, occurred_at)
) partition by range (occurred_at);
create table ad_impressions_default partition of ad_impressions default;
create index ix_ad_impressions_campaign on ad_impressions (campaign_id, occurred_at desc);
create index ix_ad_impressions_creative on ad_impressions (creative_id);
create index ix_ad_impressions_placement on ad_impressions (placement_id);
create index ix_ad_impressions_viewer on ad_impressions (campaign_id, viewer_hash);

create table ad_clicks (
    id uuid not null default gen_random_uuid(),
    campaign_id uuid not null references ad_campaigns(id),
    creative_id uuid not null references ad_creatives(id),
    placement_id uuid not null references ad_placements(id),
    click_token text not null,
    viewer_hash bytea not null,
    cost numeric(12,6) not null default 0 check (cost >= 0),
    is_valid boolean not null default true,
    occurred_at timestamptz not null default now(),
    primary key (id, occurred_at)
) partition by range (occurred_at);
create table ad_clicks_default partition of ad_clicks default;
create index ix_ad_clicks_campaign on ad_clicks (campaign_id, occurred_at desc);
create index ix_ad_clicks_creative on ad_clicks (creative_id);
create index ix_ad_clicks_placement on ad_clicks (placement_id);
create index ix_ad_clicks_token on ad_clicks (click_token);

create table ad_conversions (
    id uuid not null default gen_random_uuid(),
    campaign_id uuid not null references ad_campaigns(id),
    creative_id uuid references ad_creatives(id),
    click_token text,
    conversion_type text not null check (conversion_type in ('product_view','add_to_cart','order','lab_booking','consultation_booking','call','other')),
    value_amount numeric(14,2) check (value_amount is null or value_amount >= 0),
    conversion_ref uuid,
    occurred_at timestamptz not null default now(),
    primary key (id, occurred_at)
) partition by range (occurred_at);
create table ad_conversions_default partition of ad_conversions default;
create index ix_ad_conversions_campaign on ad_conversions (campaign_id, occurred_at desc);
create index ix_ad_conversions_creative on ad_conversions (creative_id);
create index ix_ad_conversions_token on ad_conversions (click_token) where click_token is not null;

create table ad_daily_rollups (
    campaign_id uuid not null references ad_campaigns(id) on delete cascade,
    creative_id uuid not null references ad_creatives(id) on delete cascade,
    placement_id uuid not null references ad_placements(id),
    day date not null,
    impressions bigint not null default 0 check (impressions >= 0),
    unique_impressions bigint not null default 0 check (unique_impressions >= 0),
    clicks bigint not null default 0 check (clicks >= 0),
    conversions bigint not null default 0 check (conversions >= 0),
    spend numeric(14,4) not null default 0 check (spend >= 0),
    attributed_revenue numeric(14,2) not null default 0 check (attributed_revenue >= 0),
    primary key (campaign_id, creative_id, placement_id, day),
    check (clicks <= impressions or impressions = 0),
    check (unique_impressions <= impressions)
);
create index ix_ad_rollups_day on ad_daily_rollups (day);
create index ix_ad_rollups_creative on ad_daily_rollups (creative_id);
create index ix_ad_rollups_placement on ad_daily_rollups (placement_id);

create table ad_billing (
    id uuid primary key default gen_random_uuid(),
    advertiser_id uuid not null references advertisers(id),
    campaign_id uuid references ad_campaigns(id),
    billing_type text not null check (billing_type in ('prepaid_topup','spend_invoice','package_fee','refund')),
    period_start date,
    period_end date,
    amount numeric(14,2) not null check (amount >= 0),
    tax_amount numeric(14,2) not null default 0 check (tax_amount >= 0),
    currency char(3) not null default 'INR',
    status text not null default 'pending' check (status in ('pending','invoiced','paid','failed','refunded','void')),
    invoice_number text unique,
    paid_at timestamptz,
    like tpl.audit including defaults,
    check (period_end is null or period_start is null or period_end >= period_start)
);
create index ix_ad_billing_advertiser on ad_billing (advertiser_id, created_at desc);
create index ix_ad_billing_campaign on ad_billing (campaign_id);
alter table payments add constraint fk_payments_ad_billing foreign key (ad_billing_id) references ad_billing(id);
alter table invoices add constraint fk_invoices_ad_billing foreign key (ad_billing_id) references ad_billing(id);
create index ix_payments_ad_billing on payments (ad_billing_id);
create index ix_invoices_ad_billing on invoices (ad_billing_id);

create table ad_budget_ledger (
    id uuid primary key default gen_random_uuid(),
    advertiser_id uuid not null references advertisers(id),
    campaign_id uuid references ad_campaigns(id),
    entry_type text not null check (entry_type in ('deposit','allocation','spend','refund','adjustment')),
    amount numeric(14,4) not null,
    balance_after numeric(14,4) not null,
    reference_id uuid,
    created_at timestamptz not null default now()
);
create index ix_ad_ledger_advertiser on ad_budget_ledger (advertiser_id, created_at desc);
create index ix_ad_ledger_campaign on ad_budget_ledger (campaign_id, created_at desc);

-- Knowledge / RAG
create table knowledge_categories (
    id uuid primary key default gen_random_uuid(),
    parent_id uuid references knowledge_categories(id),
    code text not null unique,
    name text not null,
    name_local jsonb not null default '{}',
    like tpl.audit including defaults
);
create index ix_knowledge_categories_parent on knowledge_categories (parent_id);

create table knowledge_documents (
    id uuid primary key default gen_random_uuid(),
    category_id uuid references knowledge_categories(id),
    crop_id uuid references crops(id),
    title text not null,
    source_type text not null check (source_type in ('agri_guide','crop_guide','disease_info','pest_info','soil_info','approved_document','consultant_knowledge','admin_content','product_info','faq')),
    source_reference text,
    language_code text not null references languages(code),
    file_object_id uuid references file_objects(id),
    content_hash text,
    version int not null default 1 check (version > 0),
    status text not null default 'draft' check (status in ('draft','in_review','approved','archived')),
    ingestion_status text not null default 'pending' check (ingestion_status in ('pending','processing','indexed','failed')),
    author_user_id uuid references users(id),
    approved_by uuid references users(id),
    approved_at timestamptz,
    like tpl.audit_sd including defaults,
    check (status <> 'approved' or approved_at is not null)
);
create index ix_knowledge_documents_category on knowledge_documents (category_id);
create index ix_knowledge_documents_crop on knowledge_documents (crop_id);
create index ix_knowledge_documents_status on knowledge_documents (status, ingestion_status) where not is_deleted;
create unique index ux_knowledge_documents_hash on knowledge_documents (content_hash, language_code) where content_hash is not null and not is_deleted;

create table knowledge_chunks (
    id uuid primary key default gen_random_uuid(),
    document_id uuid not null references knowledge_documents(id) on delete cascade,
    chunk_index int not null check (chunk_index >= 0),
    content text not null,
    token_count int check (token_count is null or token_count > 0),
    language_code text not null references languages(code),
    heading text,
    page_number int,
    search_vector tsvector generated always as (to_tsvector('simple', content)) stored,
    like tpl.audit including defaults,
    unique (document_id, chunk_index)
);
create index ix_knowledge_chunks_search on knowledge_chunks using gin (search_vector);

create table embeddings (
    id uuid primary key default gen_random_uuid(),
    chunk_id uuid not null references knowledge_chunks(id) on delete cascade,
    model_version_id uuid references ai_model_versions(id),
    embedding_model text not null,
    embedding vector(1024) not null,
    created_at timestamptz not null default now(),
    unique (chunk_id, embedding_model)
);
create index ix_embeddings_model_version on embeddings (model_version_id);
create index ix_embeddings_hnsw on embeddings using hnsw (embedding vector_cosine_ops) with (m = 16, ef_construction = 64);

-- Support
create table support_tickets (
    id uuid primary key default gen_random_uuid(),
    ticket_number text not null unique,
    user_id uuid not null references users(id),
    category text not null check (category in ('account','payment','order','booking','consultation','lab','ai','technical','advertisement','other')),
    priority text not null default 'normal' check (priority in ('low','normal','high','urgent')),
    subject text not null,
    description text not null,
    assigned_agent_id uuid references users(id),
    status text not null default 'open' check (status in ('open','assigned','in_progress','resolved','closed')),
    resolution text,
    related_entity_type text,
    related_entity_id uuid,
    first_response_at timestamptz,
    resolved_at timestamptz,
    closed_at timestamptz,
    like tpl.audit including defaults,
    check (status <> 'assigned' or assigned_agent_id is not null),
    check (status not in ('resolved','closed') or resolution is not null)
);
create index ix_tickets_user on support_tickets (user_id, created_at desc);
create index ix_tickets_queue on support_tickets (status, priority, created_at) where status in ('open','assigned','in_progress');
create index ix_tickets_agent on support_tickets (assigned_agent_id, status);
create index ix_tickets_entity on support_tickets (related_entity_type, related_entity_id);
alter table chat_rooms add constraint fk_chat_rooms_ticket foreign key (ticket_id) references support_tickets(id);

create table support_ticket_messages (
    id uuid primary key default gen_random_uuid(),
    ticket_id uuid not null references support_tickets(id) on delete cascade,
    author_user_id uuid not null references users(id),
    body text not null,
    is_internal boolean not null default false,
    created_at timestamptz not null default now()
);
create index ix_ticket_messages_ticket on support_ticket_messages (ticket_id, created_at);

create table support_ticket_attachments (
    id uuid primary key default gen_random_uuid(),
    ticket_id uuid not null references support_tickets(id) on delete cascade,
    message_id uuid references support_ticket_messages(id) on delete cascade,
    file_object_id uuid not null references file_objects(id),
    created_at timestamptz not null default now()
);
create index ix_ticket_attachments_ticket on support_ticket_attachments (ticket_id);
create index ix_ticket_attachments_message on support_ticket_attachments (message_id);

create table complaints (
    id uuid primary key default gen_random_uuid(),
    complaint_number text not null unique,
    complainant_user_id uuid not null references users(id),
    against_type text not null check (against_type in ('user','consultant','nursery','laboratory','shop','product','order','advertisement','review','other')),
    against_id uuid,
    category text not null,
    description text not null,
    status text not null default 'open' check (status in ('open','under_review','resolved','rejected','escalated')),
    ticket_id uuid references support_tickets(id),
    resolution text,
    resolved_by uuid references users(id),
    resolved_at timestamptz,
    like tpl.audit including defaults,
    check (status not in ('resolved','rejected') or resolved_at is not null)
);
create index ix_complaints_complainant on complaints (complainant_user_id);
create index ix_complaints_against on complaints (against_type, against_id);
create index ix_complaints_status on complaints (status, created_at) where status in ('open','under_review','escalated');
create index ix_complaints_ticket on complaints (ticket_id);

create table system_configurations (
    id uuid primary key default gen_random_uuid(),
    config_key text not null unique check (config_key ~ '^[a-z0-9_.]+$'),
    value jsonb not null,
    value_type text not null default 'string' check (value_type in ('string','number','boolean','json')),
    description text,
    is_sensitive boolean not null default false,
    like tpl.audit including defaults
);

-- Audit log (append-only, monthly-partitionable)
create table audit_logs (
    id uuid not null default gen_random_uuid(),
    user_id uuid,
    actor_role text,
    action text not null,
    entity_type text not null,
    entity_id text,
    old_value jsonb,
    new_value jsonb,
    reason text,
    ip_address inet,
    device text,
    correlation_id text,
    created_at timestamptz not null default now(),
    primary key (id, created_at)
) partition by range (created_at);
create table audit_logs_default partition of audit_logs default;
create index ix_audit_user on audit_logs (user_id, created_at desc);
create index ix_audit_entity on audit_logs (entity_type, entity_id, created_at desc);
create index ix_audit_action on audit_logs (action, created_at desc);

-- Platform plumbing for reliable messaging / idempotency
create table outbox_messages (
    id uuid primary key default gen_random_uuid(),
    event_type text not null,
    aggregate_type text,
    aggregate_id uuid,
    payload jsonb not null,
    headers jsonb not null default '{}',
    created_at timestamptz not null default now(),
    available_at timestamptz not null default now(),
    processed_at timestamptz,
    attempts int not null default 0 check (attempts >= 0),
    last_error text
);
create index ix_outbox_pending on outbox_messages (available_at) where processed_at is null;

create table inbox_messages (
    consumer text not null,
    message_id uuid not null,
    processed_at timestamptz not null default now(),
    primary key (consumer, message_id)
);

create table idempotency_keys (
    key text not null,
    user_id uuid not null,
    request_hash text not null,
    response_status int,
    response_body jsonb,
    created_at timestamptz not null default now(),
    expires_at timestamptz not null,
    primary key (user_id, key)
);
create index ix_idempotency_expiry on idempotency_keys (expires_at);

create table background_job_runs (
    id uuid primary key default gen_random_uuid(),
    job_name text not null,
    status text not null check (status in ('running','succeeded','failed')),
    started_at timestamptz not null default now(),
    finished_at timestamptz,
    processed_count int,
    error text
);
create index ix_job_runs_name on background_job_runs (job_name, started_at desc);
