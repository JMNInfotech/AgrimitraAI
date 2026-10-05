-- V009: nursery, laboratory, shop, unified marketplace catalogue
create table manufacturers (
    id uuid primary key default gen_random_uuid(),
    name text not null,
    country_id uuid references countries(id),
    website text,
    is_active boolean not null default true,
    like tpl.audit_sd including defaults
);
create unique index ux_manufacturers_name on manufacturers (lower(name)) where not is_deleted;

create table product_categories (
    id uuid primary key default gen_random_uuid(),
    parent_id uuid references product_categories(id),
    code text not null unique,
    name text not null,
    name_local jsonb not null default '{}',
    domain text not null check (domain in ('nursery_plants','agri_products','pesticides','fertilizers','seeds','equipment','soil_tests','agri_services','consultant_services')),
    icon_key text,
    sort_order int not null default 0,
    is_active boolean not null default true,
    like tpl.audit including defaults
);
create index ix_product_categories_parent on product_categories (parent_id);
create index ix_product_categories_domain on product_categories (domain);

-- Nursery
create table nursery_products (
    id uuid primary key default gen_random_uuid(),
    nursery_id uuid not null references nursery_profiles(id),
    crop_id uuid not null references crops(id),
    category_id uuid references product_categories(id),
    name text not null,
    description text,
    unit text not null default 'piece' check (unit in ('piece','kg','bag','tray')),
    is_available boolean not null default true,
    like tpl.audit_sd including defaults
);
create index ix_nursery_products_nursery on nursery_products (nursery_id) where not is_deleted;
create index ix_nursery_products_crop on nursery_products (crop_id);
create index ix_nursery_products_category on nursery_products (category_id);
create index ix_nursery_products_name_trgm on nursery_products using gin (name gin_trgm_ops);

create table nursery_product_varieties (
    id uuid primary key default gen_random_uuid(),
    nursery_product_id uuid not null references nursery_products(id) on delete cascade,
    crop_variety_id uuid references crop_varieties(id),
    name text not null,
    description text,
    like tpl.audit_sd including defaults
);
create index ix_nursery_product_varieties_product on nursery_product_varieties (nursery_product_id);
create index ix_nursery_product_varieties_variety on nursery_product_varieties (crop_variety_id);

create table nursery_batches (
    id uuid primary key default gen_random_uuid(),
    nursery_id uuid not null references nursery_profiles(id),
    nursery_product_id uuid not null references nursery_products(id),
    product_variety_id uuid references nursery_product_varieties(id),
    batch_code text not null,
    plant_age_days int check (plant_age_days is null or plant_age_days >= 0),
    sown_on date,
    ready_on date,
    quantity_total int not null check (quantity_total > 0),
    quantity_available int not null check (quantity_available >= 0),
    unit_price numeric(12,2) not null check (unit_price >= 0),
    currency char(3) not null default 'INR',
    status text not null default 'growing' check (status in ('growing','ready','sold_out','discarded')),
    description text,
    like tpl.audit_sd including defaults,
    check (quantity_available <= quantity_total),
    check (ready_on is null or sown_on is null or ready_on >= sown_on),
    unique (nursery_id, batch_code)
);
create index ix_nursery_batches_product on nursery_batches (nursery_product_id, status) where not is_deleted;
create index ix_nursery_batches_variety on nursery_batches (product_variety_id);
create index ix_nursery_batches_ready on nursery_batches (ready_on) where status in ('growing','ready');

create table nursery_inventory (
    batch_id uuid primary key references nursery_batches(id) on delete cascade,
    quantity_on_hand int not null check (quantity_on_hand >= 0),
    quantity_reserved int not null default 0 check (quantity_reserved >= 0),
    quantity_damaged int not null default 0 check (quantity_damaged >= 0),
    low_stock_threshold int check (low_stock_threshold is null or low_stock_threshold >= 0),
    updated_at timestamptz not null default now(),
    updated_by uuid,
    check (quantity_reserved <= quantity_on_hand)
);

-- Laboratory
create table lab_test_types (
    id uuid primary key default gen_random_uuid(),
    code text not null unique,
    name text not null,
    name_local jsonb not null default '{}',
    category text not null check (category in ('soil','water','plant_tissue','pesticide_residue','fertilizer','seed','other')),
    sample_type text not null check (sample_type in ('soil','water','leaf','petiole','fruit','seed','fertilizer','other')),
    description text,
    is_active boolean not null default true,
    like tpl.audit including defaults
);

create table laboratory_services (
    id uuid primary key default gen_random_uuid(),
    laboratory_id uuid not null references laboratory_profiles(id),
    test_type_id uuid not null references lab_test_types(id),
    name text,
    price numeric(12,2) not null check (price >= 0),
    currency char(3) not null default 'INR',
    turnaround_days int not null default 7 check (turnaround_days > 0),
    sample_collection_available boolean not null default false,
    is_active boolean not null default true,
    like tpl.audit_sd including defaults
);
create unique index ux_lab_services_lab_test on laboratory_services (laboratory_id, test_type_id) where not is_deleted;
create index ix_lab_services_test on laboratory_services (test_type_id);

create table lab_bookings (
    id uuid primary key default gen_random_uuid(),
    booking_number text not null unique,
    laboratory_id uuid not null references laboratory_profiles(id),
    farmer_profile_id uuid not null references farmer_profiles(id),
    land_id uuid references lands(id),
    crop_id uuid references crops(id),
    crop_cycle_id uuid references crop_cycles(id),
    service_id uuid not null references laboratory_services(id),
    sample_type text not null,
    sample_handover text not null default 'drop_off' check (sample_handover in ('drop_off','pickup','courier')),
    preferred_date date,
    price numeric(12,2) not null check (price >= 0),
    currency char(3) not null default 'INR',
    payment_id uuid,
    status text not null default 'submitted' check (status in ('submitted','payment_pending','confirmed','sample_received','testing','review_pending','report_ready','completed','cancelled','rejected')),
    notes text,
    like tpl.audit_sd including defaults
);
create index ix_lab_bookings_lab_status on lab_bookings (laboratory_id, status, created_at desc) where not is_deleted;
create index ix_lab_bookings_farmer on lab_bookings (farmer_profile_id, created_at desc);
create index ix_lab_bookings_service on lab_bookings (service_id);
create index ix_lab_bookings_land on lab_bookings (land_id);
create index ix_lab_bookings_crop on lab_bookings (crop_id);
create index ix_lab_bookings_cycle on lab_bookings (crop_cycle_id);
alter table soil_tests add constraint fk_soil_tests_booking foreign key (lab_booking_id) references lab_bookings(id);
create index ix_soil_tests_booking on soil_tests (lab_booking_id);

create table lab_booking_events (
    id uuid primary key default gen_random_uuid(),
    booking_id uuid not null references lab_bookings(id) on delete cascade,
    from_status text,
    to_status text not null,
    actor_user_id uuid references users(id),
    note text,
    created_at timestamptz not null default now()
);
create index ix_lab_booking_events_booking on lab_booking_events (booking_id, created_at);

create table lab_samples (
    id uuid primary key default gen_random_uuid(),
    booking_id uuid not null references lab_bookings(id),
    sample_code text not null,
    laboratory_id uuid not null references laboratory_profiles(id),
    soil_sample_id uuid references soil_samples(id),
    sample_type text not null,
    received_at timestamptz,
    received_by uuid references users(id),
    condition text check (condition in ('good','damaged','insufficient','contaminated')),
    storage_location text,
    status text not null default 'expected' check (status in ('expected','received','in_testing','tested','disposed','rejected')),
    like tpl.audit including defaults,
    unique (laboratory_id, sample_code)
);
create index ix_lab_samples_booking on lab_samples (booking_id);
create index ix_lab_samples_soil_sample on lab_samples (soil_sample_id);
create index ix_lab_samples_status on lab_samples (laboratory_id, status);

create table lab_results (
    id uuid primary key default gen_random_uuid(),
    sample_id uuid not null references lab_samples(id),
    test_type_id uuid not null references lab_test_types(id),
    status text not null default 'draft' check (status in ('draft','submitted','approved','rejected')),
    technician_id uuid references users(id),
    technician_submitted_at timestamptz,
    reviewer_id uuid references users(id),
    reviewed_at timestamptz,
    review_notes text,
    summary text,
    like tpl.audit including defaults,
    unique (sample_id, test_type_id),
    check (status not in ('approved','rejected') or (reviewer_id is not null and reviewed_at is not null)),
    check (reviewer_id is null or technician_id is null or reviewer_id <> technician_id)
);
create index ix_lab_results_test_type on lab_results (test_type_id);
create index ix_lab_results_review on lab_results (technician_submitted_at) where status = 'submitted';

create table lab_result_values (
    id uuid primary key default gen_random_uuid(),
    lab_result_id uuid not null references lab_results(id) on delete cascade,
    soil_parameter_id uuid references soil_parameters(id),
    parameter_name text not null,
    value_numeric numeric(14,4),
    value_text text,
    unit text,
    reference_range text,
    flag text check (flag in ('low','normal','high','critical')),
    like tpl.audit including defaults,
    check (value_numeric is not null or value_text is not null)
);
create index ix_lab_result_values_result on lab_result_values (lab_result_id);
create index ix_lab_result_values_param on lab_result_values (soil_parameter_id);

create table lab_reports (
    id uuid primary key default gen_random_uuid(),
    report_number text not null unique,
    booking_id uuid not null references lab_bookings(id),
    laboratory_id uuid not null references laboratory_profiles(id),
    pdf_file_object_id uuid references file_objects(id),
    status text not null default 'draft' check (status in ('draft','issued','revoked')),
    issued_at timestamptz,
    issued_by uuid references users(id),
    soil_report_id uuid references soil_reports(id),
    like tpl.audit_sd including defaults,
    check (status <> 'issued' or (issued_at is not null and pdf_file_object_id is not null))
);
create index ix_lab_reports_booking on lab_reports (booking_id);
create index ix_lab_reports_lab on lab_reports (laboratory_id, issued_at desc);
create index ix_lab_reports_soil on lab_reports (soil_report_id);

-- Shop
create table shop_products (
    id uuid primary key default gen_random_uuid(),
    shop_id uuid not null references shop_profiles(id),
    category_id uuid not null references product_categories(id),
    manufacturer_id uuid references manufacturers(id),
    name text not null,
    brand text,
    active_ingredient text,
    formulation text,
    registration_number text,
    pack_size numeric(12,3) not null check (pack_size > 0),
    pack_unit text not null check (pack_unit in ('g','kg','ml','l','piece','packet','bag')),
    mrp numeric(12,2) check (mrp is null or mrp >= 0),
    price numeric(12,2) not null check (price >= 0),
    currency char(3) not null default 'INR',
    tax_percent numeric(5,2) not null default 0 check (tax_percent between 0 and 100),
    description text,
    image_file_id uuid references file_objects(id),
    is_restricted boolean not null default false,
    is_available boolean not null default true,
    search_vector tsvector generated always as (to_tsvector('simple', coalesce(name,'') || ' ' || coalesce(brand,'') || ' ' || coalesce(active_ingredient,''))) stored,
    like tpl.audit_sd including defaults,
    check (mrp is null or price <= mrp)
);
create index ix_shop_products_shop on shop_products (shop_id, is_available) where not is_deleted;
create index ix_shop_products_category on shop_products (category_id);
create index ix_shop_products_manufacturer on shop_products (manufacturer_id);
create index ix_shop_products_ingredient on shop_products (lower(active_ingredient));
create index ix_shop_products_search on shop_products using gin (search_vector);

create table product_batches (
    id uuid primary key default gen_random_uuid(),
    shop_product_id uuid not null references shop_products(id),
    batch_number text not null,
    manufactured_on date,
    expiry_date date not null,
    quantity_received int not null check (quantity_received > 0),
    quantity_available int not null check (quantity_available >= 0),
    cost_price numeric(12,2) check (cost_price is null or cost_price >= 0),
    status text not null default 'active' check (status in ('active','expired','recalled','sold_out')),
    like tpl.audit_sd including defaults,
    unique (shop_product_id, batch_number),
    check (quantity_available <= quantity_received),
    check (manufactured_on is null or expiry_date > manufactured_on)
);
create index ix_product_batches_expiry on product_batches (expiry_date) where status = 'active';

create table product_inventory (
    shop_product_id uuid primary key references shop_products(id) on delete cascade,
    quantity_on_hand int not null default 0 check (quantity_on_hand >= 0),
    quantity_reserved int not null default 0 check (quantity_reserved >= 0),
    low_stock_threshold int check (low_stock_threshold is null or low_stock_threshold >= 0),
    updated_at timestamptz not null default now(),
    updated_by uuid,
    check (quantity_reserved <= quantity_on_hand)
);

-- Marketplace catalogue: one row per sellable thing, exactly one seller and one source
create table products (
    id uuid primary key default gen_random_uuid(),
    seller_type text not null check (seller_type in ('nursery','shop','laboratory','consultant')),
    nursery_id uuid references nursery_profiles(id),
    shop_id uuid references shop_profiles(id),
    laboratory_id uuid references laboratory_profiles(id),
    consultant_id uuid references consultant_profiles(id),
    nursery_batch_id uuid references nursery_batches(id),
    shop_product_id uuid references shop_products(id),
    laboratory_service_id uuid references laboratory_services(id),
    consultation_service_id uuid references consultation_services(id),
    kind text not null check (kind in ('physical','service')),
    category_id uuid not null references product_categories(id),
    crop_id uuid references crops(id),
    title text not null,
    description text,
    price numeric(12,2) not null check (price >= 0),
    currency char(3) not null default 'INR',
    unit text,
    is_available boolean not null default true,
    is_verified_seller boolean not null default false,
    rating_average numeric(3,2) not null default 0 check (rating_average between 0 and 5),
    rating_count int not null default 0 check (rating_count >= 0),
    status text not null default 'active' check (status in ('draft','active','paused','archived','blocked')),
    search_vector tsvector generated always as (to_tsvector('simple', coalesce(title,'') || ' ' || coalesce(description,''))) stored,
    like tpl.audit_sd including defaults,
    constraint ck_products_one_seller check (
        (seller_type = 'nursery' and nursery_id is not null and shop_id is null and laboratory_id is null and consultant_id is null) or
        (seller_type = 'shop' and shop_id is not null and nursery_id is null and laboratory_id is null and consultant_id is null) or
        (seller_type = 'laboratory' and laboratory_id is not null and nursery_id is null and shop_id is null and consultant_id is null) or
        (seller_type = 'consultant' and consultant_id is not null and nursery_id is null and shop_id is null and laboratory_id is null)),
    constraint ck_products_one_source check (
        (nursery_batch_id is not null)::int + (shop_product_id is not null)::int +
        (laboratory_service_id is not null)::int + (consultation_service_id is not null)::int = 1)
);
create unique index ux_products_nursery_batch on products (nursery_batch_id) where nursery_batch_id is not null and not is_deleted;
create unique index ux_products_shop_product on products (shop_product_id) where shop_product_id is not null and not is_deleted;
create unique index ux_products_lab_service on products (laboratory_service_id) where laboratory_service_id is not null and not is_deleted;
create unique index ux_products_consult_service on products (consultation_service_id) where consultation_service_id is not null and not is_deleted;
create index ix_products_nursery on products (nursery_id);
create index ix_products_shop on products (shop_id);
create index ix_products_laboratory on products (laboratory_id);
create index ix_products_consultant on products (consultant_id);
create index ix_products_category_price on products (category_id, price) where status = 'active' and not is_deleted;
create index ix_products_crop on products (crop_id);
create index ix_products_seller_type on products (seller_type, status);
create index ix_products_search on products using gin (search_vector);
create index ix_products_title_trgm on products using gin (title gin_trgm_ops);

create table product_images (
    id uuid primary key default gen_random_uuid(),
    product_id uuid not null references products(id) on delete cascade,
    file_object_id uuid not null references file_objects(id),
    sort_order int not null default 0,
    is_primary boolean not null default false,
    alt_text text,
    like tpl.audit including defaults
);
create index ix_product_images_product on product_images (product_id, sort_order);
create unique index ux_product_images_primary on product_images (product_id) where is_primary;

create table product_locations (
    id uuid primary key default gen_random_uuid(),
    product_id uuid not null references products(id) on delete cascade,
    address_id uuid references addresses(id),
    location geography(Point,4326) not null,
    service_radius_km numeric(8,2) check (service_radius_km is null or service_radius_km >= 0),
    like tpl.audit including defaults
);
create index ix_product_locations_product on product_locations (product_id);
create index ix_product_locations_geo on product_locations using gist (location);

-- Reviews: only for completed transactions, enforced by requiring a verified reference
create table reviews (
    id uuid primary key default gen_random_uuid(),
    reviewer_user_id uuid not null references users(id),
    target_type text not null check (target_type in ('product','nursery','laboratory','shop','consultant')),
    product_id uuid references products(id),
    nursery_id uuid references nursery_profiles(id),
    laboratory_id uuid references laboratory_profiles(id),
    shop_id uuid references shop_profiles(id),
    consultant_id uuid references consultant_profiles(id),
    order_item_id uuid,
    lab_booking_id uuid references lab_bookings(id),
    consultation_id uuid references consultations(id),
    rating smallint not null check (rating between 1 and 5),
    title text,
    comment text,
    status text not null default 'pending' check (status in ('pending','approved','rejected','hidden')),
    moderated_by uuid references users(id),
    moderated_at timestamptz,
    moderation_reason text,
    seller_reply text,
    like tpl.audit_sd including defaults,
    constraint ck_reviews_one_target check (
        (product_id is not null)::int + (nursery_id is not null)::int + (laboratory_id is not null)::int +
        (shop_id is not null)::int + (consultant_id is not null)::int = 1),
    constraint ck_reviews_verified_ref check (
        (order_item_id is not null)::int + (lab_booking_id is not null)::int + (consultation_id is not null)::int = 1)
);
create index ix_reviews_product on reviews (product_id, status);
create index ix_reviews_nursery on reviews (nursery_id, status);
create index ix_reviews_laboratory on reviews (laboratory_id, status);
create index ix_reviews_shop on reviews (shop_id, status);
create index ix_reviews_consultant on reviews (consultant_id, status);
create index ix_reviews_reviewer on reviews (reviewer_user_id);
create index ix_reviews_moderation on reviews (created_at) where status = 'pending';
create unique index ux_reviews_order_item on reviews (reviewer_user_id, order_item_id) where order_item_id is not null and not is_deleted;
create unique index ux_reviews_lab_booking on reviews (reviewer_user_id, lab_booking_id) where lab_booking_id is not null and not is_deleted;
create unique index ux_reviews_consultation on reviews (reviewer_user_id, consultation_id) where consultation_id is not null and not is_deleted;
