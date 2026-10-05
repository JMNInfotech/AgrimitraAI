-- V003: geographic master data, addresses, partner/farmer profiles, verification
create table countries (
    id uuid primary key default gen_random_uuid(),
    iso2 char(2) not null unique,
    iso3 char(3) not null unique,
    name text not null,
    name_local jsonb not null default '{}',
    phone_code text,
    is_active boolean not null default true,
    like tpl.audit including defaults
);

create table states (
    id uuid primary key default gen_random_uuid(),
    country_id uuid not null references countries(id),
    code text not null,
    name text not null,
    name_local jsonb not null default '{}',
    centroid geography(Point,4326),
    is_active boolean not null default true,
    like tpl.audit including defaults,
    unique (country_id, code)
);

create table districts (
    id uuid primary key default gen_random_uuid(),
    state_id uuid not null references states(id),
    code text not null,
    name text not null,
    name_local jsonb not null default '{}',
    centroid geography(Point,4326),
    is_active boolean not null default true,
    like tpl.audit including defaults,
    unique (state_id, code)
);
create index ix_districts_state on districts (state_id);

create table talukas (
    id uuid primary key default gen_random_uuid(),
    district_id uuid not null references districts(id),
    code text not null,
    name text not null,
    name_local jsonb not null default '{}',
    centroid geography(Point,4326),
    is_active boolean not null default true,
    like tpl.audit including defaults,
    unique (district_id, code)
);
create index ix_talukas_district on talukas (district_id);

create table villages (
    id uuid primary key default gen_random_uuid(),
    taluka_id uuid not null references talukas(id),
    code text not null,
    name text not null,
    name_local jsonb not null default '{}',
    pincode text check (pincode is null or pincode ~ '^[0-9]{4,10}$'),
    centroid geography(Point,4326),
    is_active boolean not null default true,
    like tpl.audit including defaults,
    unique (taluka_id, code)
);
create index ix_villages_taluka on villages (taluka_id);
create index ix_villages_pincode on villages (pincode) where pincode is not null;
create index ix_villages_name_trgm on villages using gin (name gin_trgm_ops);

create table pincodes (
    pincode text primary key check (pincode ~ '^[0-9]{4,10}$'),
    country_id uuid not null references countries(id),
    state_id uuid references states(id),
    district_id uuid references districts(id)
);

-- Reusable address. Hierarchy consistency is enforced by trigger below.
create table addresses (
    id uuid primary key default gen_random_uuid(),
    country_id uuid not null references countries(id),
    state_id uuid references states(id),
    district_id uuid references districts(id),
    taluka_id uuid references talukas(id),
    village_id uuid references villages(id),
    city text,
    line1 text,
    line2 text,
    landmark text,
    pincode text check (pincode is null or pincode ~ '^[0-9]{4,10}$'),
    location geography(Point,4326),
    like tpl.audit_sd including defaults
);
create index ix_addresses_country on addresses (country_id);
create index ix_addresses_state on addresses (state_id);
create index ix_addresses_district on addresses (district_id);
create index ix_addresses_taluka on addresses (taluka_id);
create index ix_addresses_village on addresses (village_id);
create index ix_addresses_pincode on addresses (pincode) where pincode is not null;
create index ix_addresses_location on addresses using gist (location);

create or replace function check_address_hierarchy() returns trigger language plpgsql as $$
begin
    if new.village_id is not null then
        select t.id, t.district_id into new.taluka_id, new.district_id
          from villages v join talukas t on t.id = v.taluka_id where v.id = new.village_id;
    elsif new.taluka_id is not null then
        select district_id into new.district_id from talukas where id = new.taluka_id;
    end if;
    if new.district_id is not null then
        select state_id into new.state_id from districts where id = new.district_id;
    end if;
    if new.state_id is not null and not exists (select 1 from states where id = new.state_id and country_id = new.country_id) then
        raise exception 'Address state does not belong to the country' using errcode = 'check_violation';
    end if;
    return new;
end $$;
create trigger trg_addresses_hierarchy before insert or update on addresses
    for each row execute function check_address_hierarchy();

-- Verification (shared by all partner types)
create table verification_records (
    id uuid primary key default gen_random_uuid(),
    subject_type text not null check (subject_type in ('consultant','nursery','laboratory','shop','advertiser','brand')),
    subject_id uuid not null,
    status text not null default 'pending' check (status in ('pending','under_review','verified','rejected','suspended')),
    submitted_at timestamptz not null default now(),
    reviewed_by uuid references users(id),
    reviewed_at timestamptz,
    rejection_reason text,
    like tpl.audit including defaults,
    check (status not in ('verified','rejected') or reviewed_at is not null)
);
create index ix_verification_subject on verification_records (subject_type, subject_id, submitted_at desc);
create index ix_verification_queue on verification_records (status, submitted_at) where status in ('pending','under_review');

create table verification_documents (
    id uuid primary key default gen_random_uuid(),
    verification_record_id uuid not null references verification_records(id) on delete cascade,
    document_type text not null,
    document_number_encrypted bytea,
    file_object_id uuid not null references file_objects(id),
    expires_on date,
    like tpl.audit including defaults
);
create index ix_verification_documents_record on verification_documents (verification_record_id);

-- Profiles
create table farmer_profiles (
    id uuid primary key default gen_random_uuid(),
    user_id uuid not null unique references users(id),
    organization_id uuid references organizations(id),
    farmer_code text not null unique,
    full_name text not null,
    date_of_birth date check (date_of_birth is null or date_of_birth < current_date),
    gender text check (gender in ('male','female','other','undisclosed')),
    photo_file_id uuid references file_objects(id),
    address_id uuid references addresses(id),
    home_location geography(Point,4326),
    gov_id_type text check (gov_id_type in ('aadhaar','voter_id','farmer_registry','other')),
    gov_id_encrypted bytea,
    gov_id_blind_index bytea,
    gov_id_last4 text check (gov_id_last4 is null or gov_id_last4 ~ '^[0-9A-Za-z]{4}$'),
    onboarding_state text not null default 'language' check (onboarding_state in ('language','profile','location','land','crop','irrigation','soil','completed')),
    farmer_segment text check (farmer_segment in ('marginal','small','medium','large')),
    like tpl.audit_sd including defaults,
    check ((gov_id_encrypted is null) = (gov_id_type is null))
);
create index ix_farmer_profiles_address on farmer_profiles (address_id);
create index ix_farmer_profiles_org on farmer_profiles (organization_id) where organization_id is not null;
create index ix_farmer_profiles_name_trgm on farmer_profiles using gin (full_name gin_trgm_ops);
create unique index ux_farmer_profiles_govid on farmer_profiles (gov_id_blind_index) where gov_id_blind_index is not null and not is_deleted;
create index ix_farmer_profiles_location on farmer_profiles using gist (home_location);

create table consultant_profiles (
    id uuid primary key default gen_random_uuid(),
    user_id uuid not null unique references users(id),
    display_name text not null,
    photo_file_id uuid references file_objects(id),
    headline text,
    bio text,
    education text,
    experience_years int check (experience_years is null or experience_years between 0 and 70),
    address_id uuid references addresses(id),
    base_location geography(Point,4326),
    service_radius_km numeric(8,2) check (service_radius_km is null or service_radius_km >= 0),
    default_fee numeric(12,2) check (default_fee is null or default_fee >= 0),
    currency char(3) not null default 'INR',
    rating_average numeric(3,2) not null default 0 check (rating_average between 0 and 5),
    rating_count int not null default 0 check (rating_count >= 0),
    verification_status text not null default 'unverified' check (verification_status in ('unverified','pending','verified','rejected','suspended')),
    verified_at timestamptz,
    is_accepting_requests boolean not null default true,
    like tpl.audit_sd including defaults
);
create index ix_consultant_profiles_verified on consultant_profiles (verification_status) where not is_deleted;
create index ix_consultant_profiles_location on consultant_profiles using gist (base_location);
create index ix_consultant_profiles_name_trgm on consultant_profiles using gin (display_name gin_trgm_ops);

create table consultant_languages (
    consultant_id uuid not null references consultant_profiles(id) on delete cascade,
    language_code text not null references languages(code),
    primary key (consultant_id, language_code)
);

create table consultant_documents (
    id uuid primary key default gen_random_uuid(),
    consultant_id uuid not null references consultant_profiles(id) on delete cascade,
    document_type text not null check (document_type in ('degree','certificate','license','id_proof','other')),
    title text,
    file_object_id uuid not null references file_objects(id),
    like tpl.audit including defaults
);
create index ix_consultant_documents_consultant on consultant_documents (consultant_id);

create table nursery_profiles (
    id uuid primary key default gen_random_uuid(),
    user_id uuid not null references users(id),
    name text not null,
    owner_name text not null,
    mobile_number text check (mobile_number is null or is_valid_e164(mobile_number)),
    email citext,
    address_id uuid references addresses(id),
    location geography(Point,4326),
    profile_image_id uuid references file_objects(id),
    description text,
    license_number text,
    verification_status text not null default 'unverified' check (verification_status in ('unverified','pending','verified','rejected','suspended')),
    verified_at timestamptz,
    rating_average numeric(3,2) not null default 0 check (rating_average between 0 and 5),
    rating_count int not null default 0 check (rating_count >= 0),
    is_active boolean not null default true,
    like tpl.audit_sd including defaults
);
create index ix_nursery_profiles_user on nursery_profiles (user_id);
create index ix_nursery_profiles_location on nursery_profiles using gist (location);
create index ix_nursery_profiles_name_trgm on nursery_profiles using gin (name gin_trgm_ops);
create index ix_nursery_profiles_verified on nursery_profiles (verification_status) where not is_deleted;

create table laboratory_profiles (
    id uuid primary key default gen_random_uuid(),
    user_id uuid not null references users(id),
    name text not null,
    mobile_number text check (mobile_number is null or is_valid_e164(mobile_number)),
    email citext,
    address_id uuid references addresses(id),
    location geography(Point,4326),
    accreditation_number text,
    profile_image_id uuid references file_objects(id),
    description text,
    verification_status text not null default 'unverified' check (verification_status in ('unverified','pending','verified','rejected','suspended')),
    verified_at timestamptz,
    rating_average numeric(3,2) not null default 0 check (rating_average between 0 and 5),
    rating_count int not null default 0 check (rating_count >= 0),
    is_active boolean not null default true,
    like tpl.audit_sd including defaults
);
create index ix_laboratory_profiles_user on laboratory_profiles (user_id);
create index ix_laboratory_profiles_location on laboratory_profiles using gist (location);
create index ix_laboratory_profiles_name_trgm on laboratory_profiles using gin (name gin_trgm_ops);
create index ix_laboratory_profiles_verified on laboratory_profiles (verification_status) where not is_deleted;

create table shop_profiles (
    id uuid primary key default gen_random_uuid(),
    user_id uuid not null references users(id),
    shop_name text not null,
    owner_name text not null,
    mobile_number text check (mobile_number is null or is_valid_e164(mobile_number)),
    email citext,
    address_id uuid references addresses(id),
    location geography(Point,4326),
    license_number text,
    license_type text,
    license_valid_until date,
    gstin text check (gstin is null or gstin ~ '^[0-9A-Z]{15}$'),
    shop_image_id uuid references file_objects(id),
    verification_status text not null default 'unverified' check (verification_status in ('unverified','pending','verified','rejected','suspended')),
    verified_at timestamptz,
    rating_average numeric(3,2) not null default 0 check (rating_average between 0 and 5),
    rating_count int not null default 0 check (rating_count >= 0),
    is_active boolean not null default true,
    like tpl.audit_sd including defaults
);
create index ix_shop_profiles_user on shop_profiles (user_id);
create index ix_shop_profiles_location on shop_profiles using gist (location);
create index ix_shop_profiles_name_trgm on shop_profiles using gin (shop_name gin_trgm_ops);
create index ix_shop_profiles_verified on shop_profiles (verification_status) where not is_deleted;

create table brand_profiles (
    id uuid primary key default gen_random_uuid(),
    user_id uuid not null references users(id),
    brand_name text not null,
    legal_name text,
    website text,
    logo_file_id uuid references file_objects(id),
    description text,
    verification_status text not null default 'unverified' check (verification_status in ('unverified','pending','verified','rejected','suspended')),
    like tpl.audit_sd including defaults
);
create index ix_brand_profiles_user on brand_profiles (user_id);
create unique index ux_brand_profiles_name on brand_profiles (lower(brand_name)) where not is_deleted;

create table advertiser_profiles (
    id uuid primary key default gen_random_uuid(),
    user_id uuid not null references users(id),
    brand_profile_id uuid references brand_profiles(id),
    company_name text not null,
    contact_name text,
    contact_email citext,
    contact_mobile text check (contact_mobile is null or is_valid_e164(contact_mobile)),
    gstin text check (gstin is null or gstin ~ '^[0-9A-Z]{15}$'),
    billing_address_id uuid references addresses(id),
    verification_status text not null default 'unverified' check (verification_status in ('unverified','pending','verified','rejected','suspended')),
    is_active boolean not null default true,
    like tpl.audit_sd including defaults
);
create index ix_advertiser_profiles_user on advertiser_profiles (user_id);
create index ix_advertiser_profiles_brand on advertiser_profiles (brand_profile_id);
