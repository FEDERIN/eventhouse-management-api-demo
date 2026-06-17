#!/bin/bash
set -e

create_db_if_not_exists() {
  local db=$1
  if [ -z "$db" ]; then
    echo "Warning: Database name is empty, skipping..."
    return
  fi

  echo "  Checking/Creating database: $db"
  psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    SELECT 'CREATE DATABASE $db'
    WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = '$db')\gexec
EOSQL
}

create_db_if_not_exists "${MANAGEMENT_DB}"
create_db_if_not_exists "${IDEMPOTENCY_DB}"
