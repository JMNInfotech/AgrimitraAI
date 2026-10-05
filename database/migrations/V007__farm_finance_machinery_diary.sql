-- V007: expenses, income, production, machinery, farm diary
create table expense_categories (
    id uuid primary key default gen_random_uuid(),
    code text not null unique,
    name text not null,
    name_local jsonb not null default '{}',
    parent_id uuid references expense_categories(id),
    is_active boolean not null default true,
    sort_order int not null default 0,
    like tpl.audit including defaults
);

create table machinery (
    id uuid primary key default gen_random_uuid(),
    farmer_profile_id uuid not null references farmer_profiles(id),
    machinery_type text not null check (machinery_type in ('tractor','sprayer','pump','harvester','drip_equipment','tool','other')),
    name text not null,
    make text,
    model text,
    registration_number text,
    purchase_date date,
    purchase_price numeric(14,2) check (purchase_price is null or purchase_price >= 0),
    status text not null default 'active' check (status in ('active','under_repair','sold','retired')),
    next_service_date date,
    notes text,
    like tpl.audit_sd including defaults
);
create index ix_machinery_farmer on machinery (farmer_profile_id, status) where not is_deleted;
create index ix_machinery_next_service on machinery (next_service_date) where next_service_date is not null and not is_deleted;
alter table crop_calendar_activities add constraint fk_calendar_activities_machinery foreign key (machinery_id) references machinery(id);
create index ix_calendar_activities_machinery on crop_calendar_activities (machinery_id) where machinery_id is not null;

create table machinery_maintenance (
    id uuid primary key default gen_random_uuid(),
    machinery_id uuid not null references machinery(id) on delete cascade,
    maintenance_date date not null,
    description text not null,
    cost numeric(12,2) not null default 0 check (cost >= 0),
    vendor text,
    receipt_file_id uuid references file_objects(id),
    like tpl.audit_sd including defaults
);
create index ix_machinery_maintenance_machine on machinery_maintenance (machinery_id, maintenance_date desc);

create table machinery_services (
    id uuid primary key default gen_random_uuid(),
    machinery_id uuid not null references machinery(id) on delete cascade,
    service_date date not null,
    service_type text,
    odometer_or_hours numeric(10,1) check (odometer_or_hours is null or odometer_or_hours >= 0),
    cost numeric(12,2) not null default 0 check (cost >= 0),
    service_provider text,
    next_service_date date,
    calendar_activity_id uuid references crop_calendar_activities(id),
    notes text,
    like tpl.audit_sd including defaults,
    check (next_service_date is null or next_service_date >= service_date)
);
create index ix_machinery_services_machine on machinery_services (machinery_id, service_date desc);
create index ix_machinery_services_activity on machinery_services (calendar_activity_id);

create table machinery_insurance (
    id uuid primary key default gen_random_uuid(),
    machinery_id uuid not null references machinery(id) on delete cascade,
    provider text not null,
    policy_number text not null,
    premium numeric(12,2) check (premium is null or premium >= 0),
    start_date date not null,
    end_date date not null,
    document_file_id uuid references file_objects(id),
    like tpl.audit_sd including defaults,
    check (end_date > start_date)
);
create index ix_machinery_insurance_machine on machinery_insurance (machinery_id, end_date desc);
create index ix_machinery_insurance_expiry on machinery_insurance (end_date) where not is_deleted;

create table expenses (
    id uuid primary key default gen_random_uuid(),
    farmer_profile_id uuid not null references farmer_profiles(id),
    category_id uuid not null references expense_categories(id),
    land_id uuid references lands(id),
    crop_id uuid references crops(id),
    crop_cycle_id uuid references crop_cycles(id),
    activity_id uuid references crop_calendar_activities(id),
    machinery_id uuid references machinery(id),
    amount numeric(14,2) not null check (amount >= 0),
    currency char(3) not null default 'INR',
    expense_date date not null,
    vendor_name text,
    receipt_file_id uuid references file_objects(id),
    order_id uuid,
    notes text,
    client_mutation_id uuid unique,
    like tpl.audit_sd including defaults
);
create index ix_expenses_farmer_date on expenses (farmer_profile_id, expense_date desc) where not is_deleted;
create index ix_expenses_category on expenses (category_id);
create index ix_expenses_land on expenses (land_id);
create index ix_expenses_crop on expenses (crop_id);
create index ix_expenses_cycle on expenses (crop_cycle_id, expense_date);
create index ix_expenses_activity on expenses (activity_id);
create index ix_expenses_machinery on expenses (machinery_id);

create table buyers (
    id uuid primary key default gen_random_uuid(),
    farmer_profile_id uuid not null references farmer_profiles(id),
    name text not null,
    buyer_type text check (buyer_type in ('trader','mandi','processor','retailer','exporter','individual','other')),
    mobile_number text,
    address_id uuid references addresses(id),
    notes text,
    like tpl.audit_sd including defaults
);
create index ix_buyers_farmer on buyers (farmer_profile_id) where not is_deleted;

create table production_records (
    id uuid primary key default gen_random_uuid(),
    farmer_profile_id uuid not null references farmer_profiles(id),
    land_id uuid not null references lands(id),
    crop_id uuid not null references crops(id),
    crop_cycle_id uuid references crop_cycles(id),
    harvest_record_id uuid references harvest_records(id),
    production_date date not null,
    quantity numeric(14,3) not null check (quantity >= 0),
    unit text not null check (unit in ('kg','quintal','tonne','piece','dozen','box','crate','litre')),
    quality_grade text,
    notes text,
    like tpl.audit_sd including defaults
);
create index ix_production_farmer_date on production_records (farmer_profile_id, production_date desc) where not is_deleted;
create index ix_production_land on production_records (land_id);
create index ix_production_crop on production_records (crop_id);
create index ix_production_cycle on production_records (crop_cycle_id);
create index ix_production_harvest on production_records (harvest_record_id);

create table income_records (
    id uuid primary key default gen_random_uuid(),
    farmer_profile_id uuid not null references farmer_profiles(id),
    land_id uuid references lands(id),
    crop_id uuid references crops(id),
    crop_cycle_id uuid references crop_cycles(id),
    production_record_id uuid references production_records(id),
    buyer_id uuid references buyers(id),
    produce_name text,
    quantity numeric(14,3) not null check (quantity > 0),
    unit text not null check (unit in ('kg','quintal','tonne','piece','dozen','box','crate','litre')),
    selling_price_per_unit numeric(14,2) not null check (selling_price_per_unit >= 0),
    revenue numeric(16,2) generated always as (round(quantity * selling_price_per_unit, 2)) stored,
    other_charges numeric(14,2) not null default 0 check (other_charges >= 0),
    currency char(3) not null default 'INR',
    income_date date not null,
    notes text,
    client_mutation_id uuid unique,
    like tpl.audit_sd including defaults
);
create index ix_income_farmer_date on income_records (farmer_profile_id, income_date desc) where not is_deleted;
create index ix_income_land on income_records (land_id);
create index ix_income_crop on income_records (crop_id);
create index ix_income_cycle on income_records (crop_cycle_id);
create index ix_income_buyer on income_records (buyer_id);
create index ix_income_production on income_records (production_record_id);

create table farm_diary (
    id uuid primary key default gen_random_uuid(),
    farmer_profile_id uuid not null references farmer_profiles(id),
    land_id uuid references lands(id),
    crop_id uuid references crops(id),
    crop_cycle_id uuid references crop_cycles(id),
    activity_id uuid references crop_calendar_activities(id),
    occurrence_id uuid references crop_activity_occurrences(id),
    entry_kind text not null check (entry_kind in ('note','irrigation','fertilizer','pest_observation','disease_observation','weather_observation','farm_activity','harvest')),
    title text,
    body text,
    occurred_at timestamptz not null default now(),
    source text not null default 'manual' check (source in ('manual','activity_completion','disease_scan','voice')),
    is_private boolean not null default true,
    client_mutation_id uuid unique,
    search_vector tsvector generated always as (to_tsvector('simple', coalesce(title,'') || ' ' || coalesce(body,''))) stored,
    like tpl.audit_sd including defaults
);
create index ix_diary_farmer_date on farm_diary (farmer_profile_id, occurred_at desc) where not is_deleted;
create index ix_diary_land on farm_diary (land_id);
create index ix_diary_crop on farm_diary (crop_id);
create index ix_diary_cycle on farm_diary (crop_cycle_id, occurred_at desc);
create index ix_diary_activity on farm_diary (activity_id);
create index ix_diary_occurrence on farm_diary (occurrence_id);
create index ix_diary_search on farm_diary using gin (search_vector);

create table farm_diary_attachments (
    id uuid primary key default gen_random_uuid(),
    diary_id uuid not null references farm_diary(id) on delete cascade,
    file_object_id uuid not null references file_objects(id),
    caption text,
    sort_order int not null default 0,
    like tpl.audit including defaults
);
create index ix_diary_attachments_diary on farm_diary_attachments (diary_id);
