#!/usr/bin/env bash
# Applies all migrations then seed files, in order, recording them in schema_migrations.
# Usage: DATABASE_URL=postgres://user:pass@host:5432/agrimitra ./database/setup-local.sh [--dev-seed]
set -euo pipefail
shopt -s nullglob
cd "$(dirname "$0")"
: "${DATABASE_URL:?DATABASE_URL is required}"
psql() { command psql "$DATABASE_URL" -v ON_ERROR_STOP=1 -q "$@"; }

psql -c "create table if not exists schema_migrations (version text primary key, applied_at timestamptz not null default now());"
files=(migrations/V*.sql seed/S*.sql)
[[ "${1:-}" == "--dev-seed" ]] && files+=(seed/dev/D*.sql)
for f in "${files[@]}"; do
  v=$(basename "$f" .sql)
  if [[ "$(psql -tA -c "select 1 from schema_migrations where version='$v'")" == "1" ]]; then continue; fi
  echo "applying $v"
  psql --single-transaction -f "$f" -c "insert into schema_migrations(version) values ('$v')"
done
echo "database up to date"
