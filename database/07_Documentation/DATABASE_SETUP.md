# Database setup

## Prerequisites

- Microsoft SQL Server 2019 or later (SQL Server 2022/2025 is suitable).
- `sqlcmd` with permission to create databases.
- SQLCMD mode enabled because the deployment uses `:r` and variables.

## Clean install

From the `database` directory:

```powershell
sqlcmd -S localhost -E -b -i .\DropAndRebuild.sql
sqlcmd -S localhost -E -b -i .\RunTests.sql
```

Use `Deploy.sql` rather than `DropAndRebuild.sql` when creating the database for the first time without dropping an existing database. Scripts are ordered and fail fast. Seed credentials are deliberately fake placeholders and are not suitable for authentication.

## Application security

Create the future API login/user, add it only to `procurement_app_executor`, and do not grant table DML. Auditors can be added to `procurement_auditor`. Production authentication remains external.

## Local validation note

On 2026-09-01 the local `MSSQLSERVER` service was running, but ODBC Driver 18 failed before login with `Encryption not supported on the client` / `No credentials are available in the security package`. Therefore the committed scripts have not been claimed as runtime-passing in this environment. Repair the local TLS/SSPI client configuration, then run both commands above.

