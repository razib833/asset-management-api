# Jamuna Bank Procurement database source export

This directory is a deployable SQL source package, not a backup of the current SQL Server data. It contains schema, constraints, indexes, stored procedures, reference/development seed data, test scripts, and documentation.

Run `Deploy.sql` from this directory with SQLCMD mode enabled. The database name is `JamunaBankProcurement`. Development test requisition seed scripts `05_SeedData/003`–`005` are intentionally not included in `Deploy.sql`; apply them only in a local test environment.

For a live-data export, create and verify a SQL Server `.bak` using an account with database backup permissions. Do not commit a live backup or production records to a public Git repository.
