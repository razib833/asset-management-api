# Validation report — 2026-09-01

## Static reconciliation

- Tables: 33
- Stored procedures: 48
- explicitly created nonclustered/filtered indexes: 30 (in addition to PK/UQ backing indexes)
- Foreign keys: 47
- Deployment/test/documentation files: 32
- Prohibited bulk tables: absent
- Redundant Concern Division action table: absent
- Workflow-matrix Concern Authority required field: absent
- Dynamic SQL in application procedures: absent

## Runtime status

Not passed in the current host. SQL Server `MSSQLSERVER` was running, but every local `sqlcmd` connection failed before login in ODBC Driver 18 with an encryption/SSPI error. Consequently no claim is made that the scripts compiled or tests passed. Run `DropAndRebuild.sql` followed by `RunTests.sql` after repairing the host client/security configuration.

## Test coverage supplied

Metadata tests verify prohibited objects, foreign-key baseline, filtered uniqueness, forbidden rule fields, dynamic SQL, and unresolved procedure dependencies. Transactional rollback tests cover duplicate codes, multiple officer/official/authority mappings, dynamic multi-select definitions, replacement-capable TVP creation, mandatory-spec rejection, workflow rule data, procurement statuses, and isolation of test data.

Additional API-phase integration tests should exercise every happy/negative transition, concurrent pool claims, mixed-route rejection, bulk partial-success orchestration, and optimistic rowversion conflicts.
