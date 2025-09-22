dotnet ef dbcontext scaffold \
"$CONN_STRING" \
Npgsql.EntityFrameworkCore.PostgreSQL \
--output-dir ./Entities \
--context-dir . \
--context MyDbContext \
--no-onconfiguring \
--namespace efscaffold.Entities \
--context-namespace Infrastructure.Postgres.Scaffolding \
--schema library \
--force
