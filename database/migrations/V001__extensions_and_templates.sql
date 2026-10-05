-- V001: extensions, shared helpers, column templates
create extension if not exists postgis;
create extension if not exists citext;
create extension if not exists pg_trgm;
create extension if not exists vector;

-- Column templates (copied with LIKE ... INCLUDING DEFAULTS so every table has identical audit columns).
create schema if not exists tpl;

create table tpl.created_only (
    created_at timestamptz not null default now(),
    created_by uuid
);
create table tpl.audit (
    created_at timestamptz not null default now(),
    updated_at timestamptz not null default now(),
    created_by uuid,
    updated_by uuid
);
create table tpl.audit_sd (
    created_at timestamptz not null default now(),
    updated_at timestamptz not null default now(),
    created_by uuid,
    updated_by uuid,
    is_deleted boolean not null default false,
    deleted_at timestamptz,
    deleted_by uuid
);

create or replace function set_updated_at() returns trigger language plpgsql as $$
begin
    new.updated_at := now();
    return new;
end $$;

create or replace function forbid_update_delete() returns trigger language plpgsql as $$
begin
    raise exception 'Table % is append-only', tg_table_name using errcode = 'restrict_violation';
end $$;

-- Reusable validators
create or replace function is_valid_e164(p text) returns boolean language sql immutable as
$$ select p ~ '^\+[1-9][0-9]{7,14}$' $$;
