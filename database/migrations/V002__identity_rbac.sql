-- V002: identity, RBAC, sessions, files, consent
create table languages (
    code text primary key check (code ~ '^[a-z]{2,3}$'),
    name text not null,
    native_name text not null,
    is_active boolean not null default true,
    is_rtl boolean not null default false,
    sort_order int not null default 0
);

create table organizations (
    id uuid primary key default gen_random_uuid(),
    name text not null,
    kind text not null default 'company' check (kind in ('company','cooperative','fpo','enterprise_farm','government')),
    is_active boolean not null default true,
    like tpl.audit_sd including defaults
);

create table users (
    id uuid primary key default gen_random_uuid(),
    organization_id uuid references organizations(id),
    mobile_number text check (mobile_number is null or is_valid_e164(mobile_number)),
    mobile_verified_at timestamptz,
    email citext check (email is null or email ~ '^[^@\s]+@[^@\s]+\.[^@\s]+$'),
    email_verified_at timestamptz,
    password_hash text,
    password_algorithm text check (password_algorithm is null or password_algorithm in ('argon2id')),
    password_changed_at timestamptz,
    preferred_language text not null default 'en' references languages(code),
    time_zone_id text not null default 'Asia/Kolkata',
    status text not null default 'pending_verification'
        check (status in ('pending_verification','active','locked','suspended','deleted')),
    failed_login_count int not null default 0 check (failed_login_count >= 0),
    lockout_end_at timestamptz,
    last_login_at timestamptz,
    mfa_enabled boolean not null default false,
    mfa_secret_encrypted bytea,
    security_stamp uuid not null default gen_random_uuid(),
    like tpl.audit_sd including defaults,
    constraint ck_users_identifier check (mobile_number is not null or email is not null)
);
create unique index ux_users_mobile on users (mobile_number) where mobile_number is not null and not is_deleted;
create unique index ux_users_email on users (email) where email is not null and not is_deleted;
create index ix_users_status on users (status) where not is_deleted;
create index ix_users_organization on users (organization_id) where organization_id is not null;

create table roles (
    id uuid primary key default gen_random_uuid(),
    name text not null unique,
    description text,
    is_system boolean not null default false,
    requires_mfa boolean not null default false,
    like tpl.audit including defaults
);

create table permissions (
    id uuid primary key default gen_random_uuid(),
    code text not null unique check (code ~ '^[a-z_]+(\.[a-z_]+)+$'),
    module text not null,
    description text,
    like tpl.audit including defaults
);
create index ix_permissions_module on permissions (module);

create table user_roles (
    user_id uuid not null references users(id) on delete cascade,
    role_id uuid not null references roles(id) on delete restrict,
    granted_by uuid references users(id),
    granted_at timestamptz not null default now(),
    expires_at timestamptz,
    primary key (user_id, role_id)
);
create index ix_user_roles_role on user_roles (role_id);

create table role_permissions (
    role_id uuid not null references roles(id) on delete cascade,
    permission_id uuid not null references permissions(id) on delete cascade,
    granted_at timestamptz not null default now(),
    primary key (role_id, permission_id)
);
create index ix_role_permissions_permission on role_permissions (permission_id);

create table user_devices (
    id uuid primary key default gen_random_uuid(),
    user_id uuid not null references users(id) on delete cascade,
    device_identifier text not null,
    platform text not null check (platform in ('android','ios','web','other')),
    device_name text,
    app_version text,
    push_token text,
    last_seen_at timestamptz,
    is_trusted boolean not null default false,
    like tpl.audit including defaults,
    unique (user_id, device_identifier)
);
create index ix_user_devices_push on user_devices (push_token) where push_token is not null;

create table user_sessions (
    id uuid primary key default gen_random_uuid(),
    user_id uuid not null references users(id) on delete cascade,
    device_id uuid references user_devices(id) on delete set null,
    active_role_id uuid references roles(id),
    ip_address inet,
    user_agent text,
    started_at timestamptz not null default now(),
    last_seen_at timestamptz not null default now(),
    expires_at timestamptz not null,
    revoked_at timestamptz,
    revoked_reason text,
    check (expires_at > started_at)
);
create index ix_user_sessions_user_active on user_sessions (user_id, expires_at) where revoked_at is null;

create table refresh_tokens (
    id uuid primary key default gen_random_uuid(),
    session_id uuid not null references user_sessions(id) on delete cascade,
    user_id uuid not null references users(id) on delete cascade,
    token_hash text not null unique,
    family_id uuid not null,
    replaced_by_id uuid references refresh_tokens(id),
    created_at timestamptz not null default now(),
    expires_at timestamptz not null,
    used_at timestamptz,
    revoked_at timestamptz,
    created_by_ip inet,
    check (expires_at > created_at)
);
create index ix_refresh_tokens_session on refresh_tokens (session_id);
create index ix_refresh_tokens_user on refresh_tokens (user_id);
create index ix_refresh_tokens_family on refresh_tokens (family_id);

create table otp_verifications (
    id uuid primary key default gen_random_uuid(),
    user_id uuid references users(id) on delete cascade,
    purpose text not null check (purpose in ('register','login','reset_password','change_mobile','change_email','verify_contact')),
    channel text not null check (channel in ('sms','whatsapp','email')),
    target text not null,
    code_hash text not null,
    attempts int not null default 0 check (attempts >= 0),
    max_attempts int not null default 5 check (max_attempts > 0),
    created_at timestamptz not null default now(),
    expires_at timestamptz not null,
    verified_at timestamptz,
    consumed_at timestamptz,
    ip_address inet,
    check (expires_at > created_at)
);
create index ix_otp_target_purpose on otp_verifications (target, purpose, created_at desc);
create index ix_otp_user on otp_verifications (user_id) where user_id is not null;

create table login_audits (
    id uuid not null default gen_random_uuid(),
    user_id uuid references users(id) on delete set null,
    identifier text,
    event text not null check (event in ('login_success','login_failed','logout','logout_all','refresh','mfa_challenge','mfa_failed','lockout','password_changed','password_reset')),
    failure_reason text,
    ip_address inet,
    user_agent text,
    device_identifier text,
    session_id uuid,
    created_at timestamptz not null default now(),
    primary key (id, created_at)
) partition by range (created_at);
create table login_audits_default partition of login_audits default;
create index ix_login_audits_user on login_audits (user_id, created_at desc);
create index ix_login_audits_ip on login_audits (ip_address, created_at desc);

create table consent_records (
    id uuid primary key default gen_random_uuid(),
    user_id uuid not null references users(id) on delete cascade,
    consent_type text not null check (consent_type in ('terms','privacy','data_sharing_consultant','analytics','ad_targeting_buckets','marketing','location','ai_training_candidate')),
    is_granted boolean not null,
    policy_version text not null,
    source text,
    ip_address inet,
    created_at timestamptz not null default now()
);
create index ix_consent_user_type on consent_records (user_id, consent_type, created_at desc);

-- Central object-storage metadata. No binaries in PostgreSQL.
create table file_objects (
    id uuid primary key default gen_random_uuid(),
    owner_user_id uuid references users(id) on delete set null,
    bucket text not null,
    object_key text not null,
    original_name text,
    mime_type text not null,
    size_bytes bigint not null check (size_bytes >= 0),
    sha256 text check (sha256 is null or sha256 ~ '^[0-9a-f]{64}$'),
    purpose text not null default 'general',
    scan_status text not null default 'pending' check (scan_status in ('pending','clean','infected','failed','skipped')),
    status text not null default 'uploaded' check (status in ('upload_pending','uploaded','available','quarantined','deleted')),
    width int check (width is null or width > 0),
    height int check (height is null or height > 0),
    duration_seconds numeric(10,2) check (duration_seconds is null or duration_seconds >= 0),
    like tpl.audit_sd including defaults,
    unique (bucket, object_key)
);
create index ix_file_objects_owner on file_objects (owner_user_id);
create index ix_file_objects_scan on file_objects (scan_status) where scan_status = 'pending';
