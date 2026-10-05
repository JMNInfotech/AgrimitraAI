# AGRIMITRA database

PostgreSQL 16 + PostGIS + pgvector. SQL files are the schema source of truth; EF Core maps them (DB-first) and its `Baseline` migration executes the same SQL.

| Path | Purpose |
|---|---|
| `migrations/V001…V013` | Versioned DDL (extensions, tables, constraints, indexes, triggers, partitions) |
| `seed/S001`, `S002` | Reference seed: 13 roles, 153 permissions + grants, languages, masters, crops/stages, placements, configs |
| `seed/dev/D*.sql` | Dev-only sample geography (never real data) |
| `verify.sql` | CI guard: every FK indexed, every table has a PK, `updated_at` triggers present |
| `tests/smoke.sql` | Constraint/trigger tests (rolled back) |
| `setup-local.sh` | psql-only runner recording versions in `schema_migrations` |

## Setup
```bash
# 1. PostgreSQL with PostGIS + pgvector (docker)
POSTGRES_PASSWORD=agri docker compose -f infra/docker/docker-compose.db.yml up -d

# 2a. Apply with psql
export DATABASE_URL=postgres://agri:agri@localhost:5432/agrimitra
./database/setup-local.sh --dev-seed          # omit --dev-seed in non-dev environments
psql "$DATABASE_URL" -f database/verify.sql && psql "$DATABASE_URL" -f database/tests/smoke.sql

# 2b. …or apply with EF Core (use ONE of 2a/2b per database)
export AGRIMITRA_DB="Host=localhost;Database=agrimitra;Username=agri;Password=agri"
dotnet ef database update --project backend/Agrimitra.Infrastructure

# 3. Mapping tests
AGRIMITRA_TEST_DB="$AGRIMITRA_DB" dotnet test backend/Agrimitra.sln
```

## Conventions
- UUID PKs (`gen_random_uuid()`), `timestamptz` UTC, enums as `text` + `CHECK` (additive changes only).
- Audit columns `created_at/updated_at/created_by/updated_by`; soft delete (`is_deleted/deleted_at/deleted_by`) on user-owned data (EF global query filter).
- `updated_at` maintained by trigger; append-only tables (audit, history, completions, inference/payment logs, ad events) reject UPDATE/DELETE.
- Partitioned monthly (12 months pre-created, default partition as safety net; call `create_monthly_partitions()` from a job): `audit_logs`, `login_audits`, `ai_inference_logs`, `payment_logs`, `weather_data`, `ad_impressions`, `ad_clicks`, `ad_conversions`. FKs *into* these tables are not possible (composite partition key), so references such as `inference_log_id` are plain UUIDs.
- Status values are lowercase `snake_case` (e.g. `in_progress`) except payment statuses, which are uppercase as specified (`PARTIALLY_REFUNDED`).
- Optimistic concurrency on key aggregates uses PostgreSQL `xmin` (mapped in `AgrimitraDbContext.Partial.cs`).
- Advertising tables hold only a rotating `viewer_hash` and coarse buckets; no farmer identifiers and no FKs to AI tables.
- Adding a migration: new `V0NN__name.sql` (never edit applied files), index every new FK (`verify.sql` fails otherwise), then `dotnet ef migrations add` to update the model snapshot.
