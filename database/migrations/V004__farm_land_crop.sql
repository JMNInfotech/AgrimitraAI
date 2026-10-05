-- V004: land, soil/irrigation masters, crops, crop cycles
create table soil_types (
    id uuid primary key default gen_random_uuid(),
    code text not null unique,
    name text not null,
    name_local jsonb not null default '{}',
    description text,
    is_active boolean not null default true,
    like tpl.audit including defaults
);
create table irrigation_types (
    id uuid primary key default gen_random_uuid(),
    code text not null unique,
    name text not null,
    name_local jsonb not null default '{}',
    is_active boolean not null default true,
    like tpl.audit including defaults
);
create table water_sources (
    id uuid primary key default gen_random_uuid(),
    code text not null unique,
    name text not null,
    name_local jsonb not null default '{}',
    is_active boolean not null default true,
    like tpl.audit including defaults
);

create table farms (
    id uuid primary key default gen_random_uuid(),
    farmer_profile_id uuid not null references farmer_profiles(id),
    organization_id uuid references organizations(id),
    name text not null,
    description text,
    address_id uuid references addresses(id),
    status text not null default 'active' check (status in ('active','inactive','archived')),
    like tpl.audit_sd including defaults
);
create index ix_farms_farmer on farms (farmer_profile_id) where not is_deleted;
create unique index ux_farms_farmer_name on farms (farmer_profile_id, lower(name)) where not is_deleted;

create table lands (
    id uuid primary key default gen_random_uuid(),
    farm_id uuid not null references farms(id),
    farmer_profile_id uuid not null references farmer_profiles(id),
    organization_id uuid references organizations(id),
    name text not null,
    survey_number text,
    address_id uuid references addresses(id),
    area_value numeric(14,4) not null check (area_value > 0),
    area_unit text not null check (area_unit in ('acre','hectare','guntha','sq_meter')),
    area_sq_meters numeric(16,2) generated always as (
        area_value * case area_unit when 'acre' then 4046.8564224 when 'hectare' then 10000 when 'guntha' then 101.17141056 else 1 end
    ) stored,
    ownership_type text not null default 'owned' check (ownership_type in ('owned','leased','shared','other')),
    location geography(Point,4326),
    soil_type_id uuid references soil_types(id),
    irrigation_type_id uuid references irrigation_types(id),
    water_source_id uuid references water_sources(id),
    status text not null default 'active' check (status in ('active','fallow','archived')),
    notes text,
    like tpl.audit_sd including defaults
);
create index ix_lands_farm on lands (farm_id) where not is_deleted;
create index ix_lands_farmer on lands (farmer_profile_id, status) where not is_deleted;
create index ix_lands_address on lands (address_id);
create index ix_lands_soil on lands (soil_type_id);
create index ix_lands_irrigation on lands (irrigation_type_id);
create index ix_lands_water on lands (water_source_id);
create index ix_lands_location on lands using gist (location);
create unique index ux_lands_farm_survey on lands (farm_id, survey_number) where survey_number is not null and not is_deleted;

create table land_boundaries (
    id uuid primary key default gen_random_uuid(),
    land_id uuid not null references lands(id) on delete cascade,
    boundary geography(Polygon,4326) not null,
    area_sq_meters numeric(16,2) generated always as (round(st_area(boundary)::numeric, 2)) stored,
    source text not null default 'drawn' check (source in ('drawn','gps_walk','imported','satellite')),
    is_current boolean not null default true,
    version int not null default 1 check (version > 0),
    like tpl.audit including defaults,
    constraint ck_land_boundaries_valid check (st_isvalid(boundary::geometry)),
    unique (land_id, version)
);
create unique index ux_land_boundaries_current on land_boundaries (land_id) where is_current;
create index ix_land_boundaries_gist on land_boundaries using gist (boundary);

create table land_images (
    id uuid primary key default gen_random_uuid(),
    land_id uuid not null references lands(id) on delete cascade,
    file_object_id uuid not null references file_objects(id),
    caption text,
    captured_at timestamptz,
    sort_order int not null default 0,
    like tpl.audit_sd including defaults
);
create index ix_land_images_land on land_images (land_id) where not is_deleted;

create table land_documents (
    id uuid primary key default gen_random_uuid(),
    land_id uuid not null references lands(id) on delete cascade,
    document_type text not null check (document_type in ('seven_twelve_extract','ownership_deed','lease_agreement','map','soil_card','other')),
    title text,
    file_object_id uuid not null references file_objects(id),
    like tpl.audit_sd including defaults
);
create index ix_land_documents_land on land_documents (land_id) where not is_deleted;

-- Crop master data
create table crops (
    id uuid primary key default gen_random_uuid(),
    code text not null unique,
    name text not null,
    name_local jsonb not null default '{}',
    scientific_name text,
    category text not null check (category in ('fruit','vegetable','cereal','pulse','oilseed','fibre','cash_crop','spice','flower','plantation','other')),
    is_perennial boolean not null default false,
    icon_key text,
    is_active boolean not null default true,
    like tpl.audit including defaults
);
create index ix_crops_category on crops (category);
create index ix_crops_name_trgm on crops using gin (name gin_trgm_ops);

create table crop_varieties (
    id uuid primary key default gen_random_uuid(),
    crop_id uuid not null references crops(id),
    code text not null,
    name text not null,
    name_local jsonb not null default '{}',
    maturity_days int check (maturity_days is null or maturity_days > 0),
    description text,
    is_active boolean not null default true,
    like tpl.audit including defaults,
    unique (crop_id, code)
);
create index ix_crop_varieties_crop on crop_varieties (crop_id);

create table crop_stages (
    id uuid primary key default gen_random_uuid(),
    crop_id uuid not null references crops(id),
    code text not null,
    name text not null,
    name_local jsonb not null default '{}',
    sequence int not null check (sequence > 0),
    typical_duration_days int check (typical_duration_days is null or typical_duration_days > 0),
    description text,
    like tpl.audit including defaults,
    unique (crop_id, code),
    unique (crop_id, sequence)
);

create table crop_cycles (
    id uuid primary key default gen_random_uuid(),
    land_id uuid not null references lands(id),
    farmer_profile_id uuid not null references farmer_profiles(id),
    crop_id uuid not null references crops(id),
    variety_id uuid references crop_varieties(id),
    current_stage_id uuid references crop_stages(id),
    name text,
    season text check (season in ('kharif','rabi','summer','perennial','year_round')),
    planting_date date not null,
    expected_harvest_date date,
    actual_harvest_date date,
    area_value numeric(14,4) check (area_value is null or area_value > 0),
    area_unit text check (area_unit in ('acre','hectare','guntha','sq_meter')),
    plant_count int check (plant_count is null or plant_count > 0),
    irrigation_type_id uuid references irrigation_types(id),
    status text not null default 'active' check (status in ('planned','active','harvested','failed','closed')),
    quality_grade text,
    closed_at timestamptz,
    like tpl.audit_sd including defaults,
    check ((area_value is null) = (area_unit is null)),
    check (expected_harvest_date is null or expected_harvest_date >= planting_date),
    check (actual_harvest_date is null or actual_harvest_date >= planting_date)
);
create index ix_crop_cycles_land_status on crop_cycles (land_id, status) where not is_deleted;
create index ix_crop_cycles_farmer_status on crop_cycles (farmer_profile_id, status) where not is_deleted;
create index ix_crop_cycles_crop on crop_cycles (crop_id);
create index ix_crop_cycles_variety on crop_cycles (variety_id);
create index ix_crop_cycles_stage on crop_cycles (current_stage_id);
create index ix_crop_cycles_harvest on crop_cycles (expected_harvest_date) where status = 'active';

create table crop_cycle_stage_history (
    id uuid primary key default gen_random_uuid(),
    crop_cycle_id uuid not null references crop_cycles(id) on delete cascade,
    stage_id uuid not null references crop_stages(id),
    started_at timestamptz not null,
    ended_at timestamptz,
    source text not null default 'farmer' check (source in ('farmer','consultant','ai_confirmed','system')),
    changed_by uuid references users(id),
    notes text,
    created_at timestamptz not null default now(),
    check (ended_at is null or ended_at >= started_at)
);
create index ix_stage_history_cycle on crop_cycle_stage_history (crop_cycle_id, started_at desc);
create unique index ux_stage_history_open on crop_cycle_stage_history (crop_cycle_id) where ended_at is null;
create index ix_stage_history_stage on crop_cycle_stage_history (stage_id);

create table crop_diseases (
    id uuid primary key default gen_random_uuid(),
    crop_id uuid references crops(id),
    code text not null unique,
    kind text not null check (kind in ('disease','pest','deficiency','disorder')),
    name text not null,
    name_local jsonb not null default '{}',
    scientific_name text,
    description text,
    symptoms text,
    prevention text,
    is_active boolean not null default true,
    like tpl.audit including defaults
);
create index ix_crop_diseases_crop on crop_diseases (crop_id);
create index ix_crop_diseases_kind on crop_diseases (kind);

-- disease_scan_id / consultation_id foreign keys are added in later migrations (circular dependency)
create table crop_disease_history (
    id uuid primary key default gen_random_uuid(),
    crop_cycle_id uuid not null references crop_cycles(id) on delete cascade,
    crop_disease_id uuid not null references crop_diseases(id),
    detected_on date not null,
    severity text check (severity in ('low','moderate','high','severe')),
    source text not null check (source in ('farmer','ai_scan','consultant','lab')),
    disease_scan_id uuid,
    consultation_id uuid,
    is_confirmed boolean not null default false,
    resolved_on date,
    notes text,
    like tpl.audit_sd including defaults,
    check (resolved_on is null or resolved_on >= detected_on)
);
create index ix_crop_disease_history_cycle on crop_disease_history (crop_cycle_id, detected_on desc);
create index ix_crop_disease_history_disease on crop_disease_history (crop_disease_id);

create table crop_production (
    id uuid primary key default gen_random_uuid(),
    crop_cycle_id uuid not null unique references crop_cycles(id) on delete cascade,
    expected_quantity numeric(14,3) check (expected_quantity is null or expected_quantity >= 0),
    actual_quantity numeric(14,3) check (actual_quantity is null or actual_quantity >= 0),
    unit text not null check (unit in ('kg','quintal','tonne','piece','dozen','box','crate','litre')),
    quality_grade text,
    market_price_per_unit numeric(12,2) check (market_price_per_unit is null or market_price_per_unit >= 0),
    like tpl.audit including defaults
);

create table harvest_records (
    id uuid primary key default gen_random_uuid(),
    crop_cycle_id uuid not null references crop_cycles(id),
    land_id uuid not null references lands(id),
    harvest_date date not null,
    quantity numeric(14,3) not null check (quantity >= 0),
    unit text not null check (unit in ('kg','quintal','tonne','piece','dozen','box','crate','litre')),
    quality_grade text,
    labour_count int check (labour_count is null or labour_count >= 0),
    notes text,
    like tpl.audit_sd including defaults
);
create index ix_harvest_cycle on harvest_records (crop_cycle_id, harvest_date desc) where not is_deleted;
create index ix_harvest_land on harvest_records (land_id);
