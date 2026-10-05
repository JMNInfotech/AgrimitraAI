-- Fails (raises) when schema conventions are violated. Run in CI after migrations.
do $$
declare n int; msg text;
begin
    -- FK without supporting index
    select count(*), string_agg(distinct c.conrelid::regclass::text, ', ') into n, msg
      from pg_constraint c join pg_class cl on cl.oid = c.conrelid and not cl.relispartition
     where c.contype = 'f' and c.connamespace = 'public'::regnamespace
       and not exists (select 1 from pg_index i where i.indrelid = c.conrelid and i.indisvalid
                        and (i.indkey::int2[])[0:cardinality(c.conkey)-1] = c.conkey);
    if n > 0 then raise exception 'FK without index on: %', msg; end if;

    -- table without primary key (partitions excluded)
    select count(*), string_agg(c.relname, ', ') into n, msg
      from pg_class c where c.relnamespace = 'public'::regnamespace and c.relkind in ('r','p') and not c.relispartition
       and c.relname <> 'schema_migrations'
       and not exists (select 1 from pg_constraint k where k.conrelid = c.oid and k.contype = 'p');
    if n > 0 then raise exception 'Tables without primary key: %', msg; end if;

    -- mutable tables missing updated_at trigger
    select count(*), string_agg(c.relname, ', ') into n, msg
      from pg_class c join pg_attribute a on a.attrelid = c.oid and a.attname = 'updated_at'
     where c.relnamespace = 'public'::regnamespace and c.relkind in ('r','p') and not c.relispartition
       and not exists (select 1 from pg_trigger t where t.tgrelid = c.oid and t.tgname = 'trg_' || c.relname || '_updated_at');
    if n > 0 then raise exception 'Tables missing updated_at trigger: %', msg; end if;
    raise notice 'schema verification passed';
end $$;
