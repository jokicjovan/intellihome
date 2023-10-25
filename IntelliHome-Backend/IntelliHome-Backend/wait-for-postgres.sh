#!/bin/bash

host="postgres"
port="5432"

apt-get update
apt-get install -y postgresql-client

until psql "postgresql://example_user:example_password@$host:$port/example_db" -c '\l'; do
  >&2 echo "Postgres is unavailable - sleeping"
  sleep 1
done

>&2 echo "Postgres is up - executing command"

dotnet restore
dotnet run
