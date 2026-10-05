-- V013: FK-supporting indexes, updated_at triggers, append-only guards, partition management

-- 1. Every foreign key gets a supporting index (skips FKs already covered by a leading-column index and partitions).
do $$
declare r record; idx text;
begin
    for r in
        select c.conrelid::regclass as tbl, c.conrelid, c.conkey,
               string_agg(a.attname, ',' order by k.ord) as cols,
               string_agg(quote_ident(a.attname), ',' order by k.ord) as qcols
          from pg_constraint c
          join pg_class cl on cl.oid = c.conrelid and not cl.relispartition
          cross join lateral unnest(c.conkey) with ordinality k(attnum, ord)
          join pg_attribute a on a.attrelid = c.conrelid and a.attnum = k.attnum
         where c.contype = 'f' and c.connamespace = 'public'::regnamespace
           and not exists (
               select 1 from pg_index i
                where i.indrelid = c.conrelid and i.indisvalid
                  and (i.indkey::int2[])[0:cardinality(c.conkey)-1] = c.conkey)
         group by c.oid, c.conrelid, c.conkey
    loop
        idx := left('ix_' || r.conrelid::regclass::text || '_' || replace(r.cols, ',', '_'), 55)
               || '_' || substr(md5(r.conrelid::regclass::text || r.cols), 1, 6);
        execute format('create index if not exists %I on %s (%s)', idx, r.tbl, r.qcols);
    end loop;
end $$;

-- 2. updated_at maintenance
do $$
declare r record;
begin
    for r in
        select c.relname from pg_class c
          join pg_attribute a on a.attrelid = c.oid and a.attname = 'updated_at' and not a.attisdropped
         where c.relnamespace = 'public'::regnamespace and c.relkind in ('r','p') and not c.relispartition
    loop
        execute format('create trigger trg_%1$s_updated_at before update on %1$I for each row execute function set_updated_at()', r.relname);
    end loop;
end $$;

-- 3. Append-only tables
do $$
declare t text;
begin
    foreach t in array array[
        'audit_logs','login_audits','ai_inference_logs','crop_activity_history','crop_activity_completions',
        'consultant_schedule_history','order_status_history','lab_booking_events','payment_logs',
        'payment_transactions','consent_records','ad_budget_ledger','ad_impressions','ad_clicks','ad_conversions']
    loop
        execute format('create trigger trg_%1$s_append_only before update or delete on %1$I for each row execute function forbid_update_delete()', t);
    end loop;
end $$;

-- 4. Immutable prescriptions once issued (edits create a new version row)
create or replace function guard_issued_prescription() returns trigger language plpgsql as $$
begin
    if old.status = 'issued' and (new.diagnosis is distinct from old.diagnosis or new.advisory is distinct from old.advisory
        or new.precautions is distinct from old.precautions or new.farmer_profile_id <> old.farmer_profile_id
        or new.version <> old.version) then
        raise exception 'Issued prescription % is immutable; create a new version', old.id using errcode = 'restrict_violation';
    end if;
    return new;
end $$;
create trigger trg_prescriptions_immutable before update on prescriptions
    for each row execute function guard_issued_prescription();

create or replace function guard_issued_prescription_items() returns trigger language plpgsql as $$
declare v_status text;
begin
    select status into v_status from prescriptions where id = coalesce(new.prescription_id, old.prescription_id);
    if v_status = 'issued' then
        raise exception 'Items of an issued prescription are immutable' using errcode = 'restrict_violation';
    end if;
    return coalesce(new, old);
end $$;
create trigger trg_prescription_items_immutable before insert or update or delete on prescription_items
    for each row execute function guard_issued_prescription_items();

-- 5. Monthly partition management for high-volume tables
create or replace function create_monthly_partitions(p_parent regclass, p_from date, p_months int)
returns int language plpgsql as $$
declare i int; d date; n int := 0; part text;
begin
    for i in 0 .. p_months - 1 loop
        d := (date_trunc('month', p_from) + make_interval(months => i))::date;
        part := format('%s_%s', p_parent::text, to_char(d, 'YYYYMM'));
        if to_regclass(part) is null then
            execute format('create table %I partition of %s for values from (%L) to (%L)',
                part, p_parent, d::timestamptz, (d + interval '1 month')::timestamptz);
            n := n + 1;
        end if;
    end loop;
    return n;
end $$;

do $$
declare t text;
begin
    foreach t in array array['audit_logs','login_audits','ai_inference_logs','payment_logs','weather_data','ad_impressions','ad_clicks','ad_conversions']
    loop
        perform create_monthly_partitions(t::regclass, date_trunc('month', now())::date, 12);
    end loop;
end $$;
