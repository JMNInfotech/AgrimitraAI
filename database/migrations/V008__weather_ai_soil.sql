-- V008: weather, AI governance (models, datasets, scans, inference logs), soil
create table weather_locations (
    id uuid primary key default gen_random_uuid(),
    land_id uuid references lands(id) on delete cascade,
    village_id uuid references villages(id),
    name text,
    location geography(Point,4326) not null,
    grid_key text not null,
    provider text not null,
    provider_location_id text,
    time_zone_id text not null default 'Asia/Kolkata',
    is_active boolean not null default true,
    last_fetched_at timestamptz,
    like tpl.audit including defaults,
    unique (provider, grid_key)
);
create index ix_weather_locations_land on weather_locations (land_id);
create index ix_weather_locations_village on weather_locations (village_id);
create index ix_weather_locations_geo on weather_locations using gist (location);

-- Link table so many lands can share one weather grid cell
create table land_weather_locations (
    land_id uuid primary key references lands(id) on delete cascade,
    weather_location_id uuid not null references weather_locations(id),
    distance_meters numeric(10,1),
    like tpl.audit including defaults
);
create index ix_land_weather_location on land_weather_locations (weather_location_id);

create table weather_data (
    id uuid not null default gen_random_uuid(),
    weather_location_id uuid not null references weather_locations(id) on delete cascade,
    kind text not null check (kind in ('current','hourly_forecast','daily_forecast','historical')),
    observed_or_forecast_for timestamptz not null,
    fetched_at timestamptz not null default now(),
    temperature_c numeric(5,2),
    temperature_min_c numeric(5,2),
    temperature_max_c numeric(5,2),
    humidity_percent numeric(5,2) check (humidity_percent is null or humidity_percent between 0 and 100),
    rain_probability_percent numeric(5,2) check (rain_probability_percent is null or rain_probability_percent between 0 and 100),
    rainfall_mm numeric(7,2) check (rainfall_mm is null or rainfall_mm >= 0),
    wind_speed_kmh numeric(6,2) check (wind_speed_kmh is null or wind_speed_kmh >= 0),
    wind_direction_deg numeric(5,1) check (wind_direction_deg is null or wind_direction_deg between 0 and 360),
    pressure_hpa numeric(7,2),
    uv_index numeric(4,1),
    condition_code text,
    raw jsonb,
    primary key (id, observed_or_forecast_for)
) partition by range (observed_or_forecast_for);
create table weather_data_default partition of weather_data default;
create unique index ux_weather_data_key on weather_data (weather_location_id, kind, observed_or_forecast_for);
create index ix_weather_data_lookup on weather_data (weather_location_id, observed_or_forecast_for desc);

create table weather_alerts (
    id uuid primary key default gen_random_uuid(),
    weather_location_id uuid not null references weather_locations(id) on delete cascade,
    alert_type text not null check (alert_type in ('heavy_rain','storm','heat_wave','cold_wave','frost','hail','high_wind','high_humidity','drought','other')),
    severity text not null check (severity in ('advisory','watch','warning','severe')),
    title text not null,
    description text,
    starts_at timestamptz not null,
    ends_at timestamptz,
    source text not null,
    external_id text,
    status text not null default 'active' check (status in ('active','expired','cancelled')),
    like tpl.audit including defaults,
    check (ends_at is null or ends_at > starts_at),
    unique (source, external_id)
);
create index ix_weather_alerts_location on weather_alerts (weather_location_id, starts_at desc);
create index ix_weather_alerts_active on weather_alerts (starts_at) where status = 'active';

alter table schedule_conflicts add column weather_alert_id uuid references weather_alerts(id);
create index ix_conflicts_weather_alert on schedule_conflicts (weather_alert_id);
create index ix_conflicts_weather_data on schedule_conflicts (weather_data_id);

-- AI model registry
create table ai_models (
    id uuid primary key default gen_random_uuid(),
    code text not null unique,
    name text not null,
    task text not null check (task in ('disease_detection','pest_detection','crop_classification','soil_interpretation','ocr','recommendation','llm','speech_to_text','text_to_speech','embedding','scheduling_conflict')),
    crop_id uuid references crops(id),
    description text,
    is_active boolean not null default true,
    like tpl.audit including defaults
);
create index ix_ai_models_crop on ai_models (crop_id);
create index ix_ai_models_task on ai_models (task);

create table datasets (
    id uuid primary key default gen_random_uuid(),
    name text not null unique,
    task text not null,
    crop_id uuid references crops(id),
    description text,
    like tpl.audit_sd including defaults
);
create index ix_datasets_crop on datasets (crop_id);

create table dataset_versions (
    id uuid primary key default gen_random_uuid(),
    dataset_id uuid not null references datasets(id),
    version text not null,
    status text not null default 'draft' check (status in ('draft','in_review','approved','rejected','archived')),
    image_count int not null default 0 check (image_count >= 0),
    train_count int not null default 0,
    validation_count int not null default 0,
    test_count int not null default 0,
    split_seed int,
    manifest_file_id uuid references file_objects(id),
    approved_by uuid references users(id),
    approved_at timestamptz,
    like tpl.audit including defaults,
    unique (dataset_id, version),
    check (status <> 'approved' or approved_at is not null)
);

create table dataset_images (
    id uuid primary key default gen_random_uuid(),
    dataset_version_id uuid not null references dataset_versions(id) on delete cascade,
    file_object_id uuid not null references file_objects(id),
    crop_id uuid references crops(id),
    crop_disease_id uuid references crop_diseases(id),
    label text,
    source text not null check (source in ('curated','farmer_feedback','consultant_confirmed','public_dataset','field_collection')),
    split text check (split in ('train','validation','test')),
    annotation jsonb,
    labeled_by uuid references users(id),
    reviewed_by uuid references users(id),
    like tpl.audit including defaults,
    unique (dataset_version_id, file_object_id)
);
create index ix_dataset_images_version_split on dataset_images (dataset_version_id, split);
create index ix_dataset_images_crop on dataset_images (crop_id);
create index ix_dataset_images_disease on dataset_images (crop_disease_id);

create table ai_model_versions (
    id uuid primary key default gen_random_uuid(),
    model_id uuid not null references ai_models(id),
    version text not null,
    status text not null default 'draft' check (status in ('draft','training','staging','approved','production','deprecated')),
    dataset_version_id uuid references dataset_versions(id),
    accuracy numeric(6,5) check (accuracy is null or accuracy between 0 and 1),
    precision_score numeric(6,5) check (precision_score is null or precision_score between 0 and 1),
    recall_score numeric(6,5) check (recall_score is null or recall_score between 0 and 1),
    f1_score numeric(6,5) check (f1_score is null or f1_score between 0 and 1),
    confidence_threshold numeric(4,3) not null default 0.700 check (confidence_threshold between 0 and 1),
    artifact_uri text,
    artifact_sha256 text,
    environment text not null default 'dev' check (environment in ('dev','staging','production')),
    trained_at timestamptz,
    deployed_at timestamptz,
    approved_by uuid references users(id),
    approved_at timestamptz,
    like tpl.audit including defaults,
    unique (model_id, version),
    check (status not in ('approved','production') or approved_at is not null)
);
create index ix_model_versions_dataset on ai_model_versions (dataset_version_id);
create unique index ux_model_versions_production on ai_model_versions (model_id, environment) where status = 'production';

alter table schedule_change_proposals add constraint fk_proposals_model_version foreign key (model_version_id) references ai_model_versions(id);

create table disease_scans (
    id uuid primary key default gen_random_uuid(),
    farmer_profile_id uuid not null references farmer_profiles(id),
    user_id uuid not null references users(id),
    land_id uuid references lands(id),
    crop_id uuid references crops(id),
    crop_cycle_id uuid references crop_cycles(id),
    location geography(Point,4326),
    captured_at timestamptz not null default now(),
    status text not null default 'queued' check (status in ('queued','processing','completed','needs_expert_review','failed')),
    client_mutation_id uuid unique,
    like tpl.audit_sd including defaults
);
create index ix_scans_farmer on disease_scans (farmer_profile_id, created_at desc) where not is_deleted;
create index ix_scans_cycle on disease_scans (crop_cycle_id);
create index ix_scans_land on disease_scans (land_id);
create index ix_scans_crop on disease_scans (crop_id);
create index ix_scans_status on disease_scans (status) where status in ('queued','processing');

create table disease_scan_images (
    id uuid primary key default gen_random_uuid(),
    disease_scan_id uuid not null references disease_scans(id) on delete cascade,
    file_object_id uuid not null references file_objects(id),
    sort_order int not null default 0,
    like tpl.audit including defaults
);
create index ix_scan_images_scan on disease_scan_images (disease_scan_id);

create table ai_inference_logs (
    id uuid not null default gen_random_uuid(),
    model_version_id uuid references ai_model_versions(id),
    kind text not null check (kind in ('disease_scan','crop_classification','soil_report','recommendation','copilot','voice','schedule_conflict','embedding')),
    user_id uuid references users(id),
    crop_id uuid references crops(id),
    coarse_location geography(Point,4326),
    input_metadata jsonb not null default '{}',
    output jsonb,
    confidence numeric(6,5) check (confidence is null or confidence between 0 and 1),
    status text not null default 'success' check (status in ('success','failed','timeout','blocked')),
    error_code text,
    latency_ms int check (latency_ms is null or latency_ms >= 0),
    correlation_id text,
    created_at timestamptz not null default now(),
    primary key (id, created_at)
) partition by range (created_at);
create table ai_inference_logs_default partition of ai_inference_logs default;
create index ix_inference_user on ai_inference_logs (user_id, created_at desc);
create index ix_inference_model on ai_inference_logs (model_version_id, created_at desc);
create index ix_inference_kind on ai_inference_logs (kind, created_at desc);
create index ix_inference_crop on ai_inference_logs (crop_id);

create table disease_results (
    id uuid primary key default gen_random_uuid(),
    disease_scan_id uuid not null references disease_scans(id) on delete cascade,
    inference_log_id uuid,
    inference_log_created_at timestamptz,
    model_version_id uuid references ai_model_versions(id),
    dataset_version_id uuid references dataset_versions(id),
    detected_crop_id uuid references crops(id),
    crop_disease_id uuid references crop_diseases(id),
    confidence numeric(6,5) not null check (confidence between 0 and 1),
    severity text check (severity in ('low','moderate','high','severe')),
    affected_area_percent numeric(5,2) check (affected_area_percent is null or affected_area_percent between 0 and 100),
    needs_expert_review boolean not null default false,
    explanation_file_id uuid references file_objects(id),
    recommended_action text,
    prevention text,
    disclaimer text not null,
    rank smallint not null default 1 check (rank > 0),
    like tpl.audit including defaults,
    unique (disease_scan_id, rank)
);
create index ix_disease_results_disease on disease_results (crop_disease_id);
create index ix_disease_results_model on disease_results (model_version_id);
create index ix_disease_results_review on disease_results (created_at) where needs_expert_review;
alter table crop_disease_history add constraint fk_disease_history_scan foreign key (disease_scan_id) references disease_scans(id);
alter table crop_disease_history add constraint fk_disease_history_consultation foreign key (consultation_id) references consultations(id);
alter table consultation_requests add constraint fk_requests_scan foreign key (disease_scan_id) references disease_scans(id);
create index ix_disease_history_scan on crop_disease_history (disease_scan_id);
create index ix_disease_history_consultation on crop_disease_history (consultation_id);
create index ix_requests_scan on consultation_requests (disease_scan_id);

-- AI recommendations are suggestions only; never prescriptions or scheduled activities.
create table ai_recommendations (
    id uuid primary key default gen_random_uuid(),
    farmer_profile_id uuid not null references farmer_profiles(id),
    land_id uuid references lands(id),
    crop_cycle_id uuid references crop_cycles(id),
    disease_result_id uuid references disease_results(id),
    model_version_id uuid references ai_model_versions(id),
    kind text not null check (kind in ('disease','irrigation','fertilizer','pest','weather','soil','general','schedule')),
    title text not null,
    content text not null,
    language_code text not null references languages(code),
    sources jsonb not null default '[]',
    risk_level text not null default 'low' check (risk_level in ('low','medium','high')),
    escalation_required boolean not null default false,
    escalated_consultation_id uuid references consultations(id),
    confidence numeric(6,5) check (confidence is null or confidence between 0 and 1),
    status text not null default 'shown' check (status in ('shown','dismissed','accepted','expired')),
    disclaimer text not null,
    expires_at timestamptz,
    like tpl.audit including defaults
);
create index ix_recommendations_farmer on ai_recommendations (farmer_profile_id, created_at desc);
create index ix_recommendations_land on ai_recommendations (land_id);
create index ix_recommendations_cycle on ai_recommendations (crop_cycle_id);
create index ix_recommendations_result on ai_recommendations (disease_result_id);
create index ix_recommendations_model on ai_recommendations (model_version_id);
create index ix_recommendations_escalation on ai_recommendations (created_at) where escalation_required;
alter table crop_calendar_activities add constraint fk_calendar_activities_ai_rec foreign key (ai_recommendation_id) references ai_recommendations(id);
create index ix_calendar_activities_ai_rec on crop_calendar_activities (ai_recommendation_id) where ai_recommendation_id is not null;

create table ai_feedback (
    id uuid primary key default gen_random_uuid(),
    user_id uuid not null references users(id),
    disease_result_id uuid references disease_results(id),
    recommendation_id uuid references ai_recommendations(id),
    inference_log_id uuid,
    feedback_type text not null check (feedback_type in ('farmer_rating','consultant_confirmation','consultant_correction')),
    is_helpful boolean,
    corrected_crop_disease_id uuid references crop_diseases(id),
    comment text,
    candidate_status text not null default 'none' check (candidate_status in ('none','proposed','approved','rejected')),
    candidate_reviewed_by uuid references users(id),
    candidate_reviewed_at timestamptz,
    like tpl.audit including defaults,
    check (disease_result_id is not null or recommendation_id is not null or inference_log_id is not null)
);
create index ix_feedback_result on ai_feedback (disease_result_id);
create index ix_feedback_recommendation on ai_feedback (recommendation_id);
create index ix_feedback_user on ai_feedback (user_id);
create index ix_feedback_candidates on ai_feedback (created_at) where candidate_status = 'proposed';

create table ai_alerts (
    id uuid primary key default gen_random_uuid(),
    farmer_profile_id uuid not null references farmer_profiles(id),
    land_id uuid references lands(id),
    crop_cycle_id uuid references crop_cycles(id),
    alert_type text not null check (alert_type in ('rain_risk','disease_risk','high_humidity','irrigation_reminder','crop_stage_activity','lab_report_ready','consultant_followup','machinery_service','crop_harvest','upcoming_activity','missed_activity','schedule_conflict')),
    severity text not null default 'info' check (severity in ('info','warning','critical')),
    title text not null,
    message text not null,
    rule_code text,
    weather_alert_id uuid references weather_alerts(id),
    occurrence_id uuid references crop_activity_occurrences(id),
    status text not null default 'new' check (status in ('new','delivered','read','dismissed','expired')),
    dedupe_key text,
    expires_at timestamptz,
    like tpl.audit including defaults
);
create index ix_ai_alerts_farmer on ai_alerts (farmer_profile_id, created_at desc);
create index ix_ai_alerts_land on ai_alerts (land_id);
create index ix_ai_alerts_cycle on ai_alerts (crop_cycle_id);
create index ix_ai_alerts_occurrence on ai_alerts (occurrence_id);
create unique index ux_ai_alerts_dedupe on ai_alerts (farmer_profile_id, dedupe_key) where dedupe_key is not null and status in ('new','delivered');

-- Soil
create table soil_parameters (
    id uuid primary key default gen_random_uuid(),
    code text not null unique,
    name text not null,
    name_local jsonb not null default '{}',
    category text not null check (category in ('reaction','macro','secondary','micro','organic','salinity','physical','other')),
    default_unit text not null,
    min_plausible numeric(14,4),
    max_plausible numeric(14,4),
    sort_order int not null default 0,
    like tpl.audit including defaults,
    check (min_plausible is null or max_plausible is null or min_plausible <= max_plausible)
);

create table soil_samples (
    id uuid primary key default gen_random_uuid(),
    farmer_profile_id uuid not null references farmer_profiles(id),
    land_id uuid not null references lands(id),
    crop_cycle_id uuid references crop_cycles(id),
    sample_code text not null unique,
    collected_on date not null,
    depth_cm numeric(6,1) check (depth_cm is null or depth_cm > 0),
    location geography(Point,4326),
    collected_by text,
    notes text,
    like tpl.audit_sd including defaults
);
create index ix_soil_samples_land on soil_samples (land_id, collected_on desc) where not is_deleted;
create index ix_soil_samples_farmer on soil_samples (farmer_profile_id);
create index ix_soil_samples_cycle on soil_samples (crop_cycle_id);

create table soil_tests (
    id uuid primary key default gen_random_uuid(),
    soil_sample_id uuid not null references soil_samples(id),
    lab_booking_id uuid,
    source text not null check (source in ('laboratory','uploaded_report','farmer_entry','field_kit')),
    tested_on date,
    status text not null default 'pending' check (status in ('pending','in_progress','completed','rejected')),
    like tpl.audit_sd including defaults
);
create index ix_soil_tests_sample on soil_tests (soil_sample_id);

create table soil_test_parameters (
    soil_test_id uuid not null references soil_tests(id) on delete cascade,
    soil_parameter_id uuid not null references soil_parameters(id),
    primary key (soil_test_id, soil_parameter_id)
);
create index ix_soil_test_parameters_param on soil_test_parameters (soil_parameter_id);

create table soil_reports (
    id uuid primary key default gen_random_uuid(),
    soil_test_id uuid not null references soil_tests(id),
    farmer_profile_id uuid not null references farmer_profiles(id),
    land_id uuid not null references lands(id),
    file_object_id uuid references file_objects(id),
    ocr_status text not null default 'not_required' check (ocr_status in ('not_required','pending','processing','extracted','needs_review','failed')),
    ocr_confidence numeric(6,5) check (ocr_confidence is null or ocr_confidence between 0 and 1),
    validated_by uuid references users(id),
    validated_at timestamptz,
    interpretation text,
    interpretation_language text references languages(code),
    inference_log_id uuid,
    report_date date,
    like tpl.audit_sd including defaults
);
create index ix_soil_reports_test on soil_reports (soil_test_id);
create index ix_soil_reports_farmer on soil_reports (farmer_profile_id, report_date desc) where not is_deleted;
create index ix_soil_reports_land on soil_reports (land_id);
create index ix_soil_reports_ocr on soil_reports (ocr_status) where ocr_status in ('pending','processing','needs_review');

create table soil_measurements (
    id uuid primary key default gen_random_uuid(),
    soil_test_id uuid not null references soil_tests(id) on delete cascade,
    soil_report_id uuid references soil_reports(id),
    soil_parameter_id uuid not null references soil_parameters(id),
    value numeric(14,4) not null,
    unit text not null,
    rating text check (rating in ('very_low','low','medium','high','very_high','optimal','deficient','excess')),
    is_extracted boolean not null default false,
    like tpl.audit including defaults,
    unique (soil_test_id, soil_parameter_id)
);
create index ix_soil_measurements_param on soil_measurements (soil_parameter_id);
create index ix_soil_measurements_report on soil_measurements (soil_report_id);
