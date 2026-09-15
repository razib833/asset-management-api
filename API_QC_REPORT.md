# API Quality Control Report

## Scope

Step 20 reviewed the ASP.NET Core API, its repository/data-access layer, database deployment scripts, workflow procedures, security boundaries, bulk operations, documentation, and tests. No new business capability was added. Changes were limited to confirmed quality and security defects.

## Critical Issues

No unresolved critical issue was found in the C# data-access path or the reviewed workflow design.

## High Issues

### Fixed

- The shared `usp_Workflow_Advance` procedure validated the active stage and actor mapping but did not also require the requisition header to remain `IN_WORKFLOW`. A replacement procedure now locks both workflow and header rows, validates header state, stage, actor mapping, ownership/current assignment, destination organization, and performs the transition atomically.
- Bulk authority and tracking services caught every exception, including cancellation, and returned raw exception messages. Cancellation now propagates and unexpected exception details are no longer disclosed to clients.

### Remaining

- Production Jamuna Bank authentication is intentionally incomplete. `JamunaBankAuthenticationProvider` throws `NotSupportedException` until the official User Management API contract is configured. Production deployment is blocked until this integration is implemented and tested.
- SQL Server deployment and database integration tests could not be executed in this environment because Windows authentication cannot establish an SSPI context. The SQL scripts were statically reviewed, but their compilation and transactional behavior must be verified against a disposable SQL Server database before release.
- Generic attachment, comment, approval-history, workflow-history, and procurement-history reads require authentication but do not enforce per-requisition object-level access. If these records are not intended to be visible to every authenticated employee, authorization must be added in stored procedures using the trusted employee identity and relevant workflow/organization mappings.

## Medium Issues

### Fixed

- The OpenAPI document listed only a small subset of routes and did not describe bearer authentication. It now enumerates all mapped `api/*` endpoints and publishes a JWT bearer security scheme with anonymous-route exceptions.
- The anonymous database-health response disclosed the active organization-unit count. It now returns only health status while still exercising stored-procedure connectivity.

### Remaining

- The test project is a custom console test harness, not a standard `Microsoft.NET.Test.Sdk` project. `dotnet test` exits successfully but discovers/runs no tests. The harness itself runs 60 tests successfully. Migrate it to a standard test framework when NuGet restore is available.
- No database-backed integration tests currently cover stored-procedure contracts, result-set shapes, transaction rollback, locking, duplicate actions, or concurrent OR-pool approvals.
- Development authentication contains plaintext, development-only sample passwords in `appsettings.Development.json`. They are non-production identities, but local secrets or user-secrets would reduce accidental reuse risk.
- OpenAPI now covers all routes and authentication, but it does not yet emit detailed request/response schemas, parameter descriptions, or operation-specific status codes. Swagger UI also loads its assets from a public CDN.
- Bulk operations execute requisitions sequentially and correctly isolate each transaction, but unexpected per-item failures are converted to generic results without application-side structured logging. Database-side history exists only for successful transactional actions.

## Low Issues

- Several source files use very dense one-line formatting, which reduces maintainability and reviewability.
- Requisition detail result-set mapping is duplicated in `RequisitionMakerRepository` and `RequisitionDetailDataSetMapper`.
- API error mapping is implemented independently in many controllers, producing small differences in status-code and message behavior.
- The database project retains earlier procedure versions that are deliberately overridden later in `Deploy.sql`. Deployment order is correct, but consolidating superseded scripts would reduce review ambiguity.
- NuGet vulnerability metadata could not be downloaded, producing `NU1900`; package vulnerability status was therefore not verified during this review.

## Fixed Issues

- Added header-state, active assignment, actor mapping, destination, and locking checks to the shared workflow transition.
- Preserved atomic assignment closure, workflow updates, approval/history records, and procurement-tracking creation.
- Prevented cancellation swallowing in all three bulk service implementations.
- Prevented raw infrastructure exception messages from being returned by bulk endpoints.
- Replaced the incomplete static OpenAPI path list with endpoint-driven route coverage and bearer security metadata.
- Removed organization counts from the anonymous database-health payload.

## Remaining Issues

Release blockers are the production authentication integration and database deployment/integration verification. Object-level access for generic supporting records requires a business visibility decision and should be resolved before broad production use. Standard test discovery and richer OpenAPI schemas remain important quality work.

## Architecture Compliance

| Area | Result | Notes |
|---|---|---|
| Controller → Service → Repository | Compliant | Business modules follow the layered pattern. |
| DTO boundary | Compliant | Controllers return DTOs/result contracts rather than database entities. |
| Current-user context | Compliant | Workflow actors, requester, creator, modifier, approver, and tracker identities originate from `ICurrentUserService`. |
| Client identity trust | Compliant | Workflow action payloads do not accept acting employee IDs or roles. Employee IDs in administration mapping requests are configuration targets, not acting identities. |
| Authentication | Partial | JWT validation is sound; development provider works; production provider is not implemented. |
| Authorization | Partial | Role policies protect workflow/configuration/audit endpoints; generic supporting reads lack object-level authorization. |
| Workflow validation | Compliant after fix | Current header state, workflow stage, assignment and mapped actor are validated in transactional procedures. |
| Transactions | Compliant | Workflow transitions, requisition submission, tracking updates, and Work Order creation use SQL transactions with `XACT_ABORT`. |
| Error handling | Compliant with noted consistency debt | Global middleware hides unhandled exception details; stored procedures return stable result codes. |
| Concurrency | Compliant for reviewed transitions | `UPDLOCK,HOLDLOCK`, current assignments, and state checks prevent duplicate/OR-pool actions. |
| Auditability | Compliant | Approval, workflow, procurement status, batch reference, attachment/comment actor, and Work Order audit records are present. |

Business-rule verification:

- No reviewed database business rule depends only on React; server services and stored procedures validate inputs and transitions.
- Every deployed workflow transition validates current state either directly or through the corrected `usp_Workflow_Advance`.
- Concern Division Authority remains optional and is reached only through the Concern Official's explicit forward action.
- Concern completion and Concern Authority approval use the matrix-defined `DESTINATION_AFTER_CONCERN` (`READY_FOR_PROCUREMENT` or `PROCUREMENT_AUTHORITY`).
- Procurement Officer rule evaluation remains stored-procedure driven.
- Work Orders support multiple requisitions through `WorkOrderRequisitionTableType` and `WORK_ORDER_REQUISITION`.
- Procurement Tracking records manual-process statuses and creates one status-history row per updated requisition.
- Bulk Procurement Authority and Concern Authority approvals generate one shared `BATCH_REFERENCE_ID` and process each requisition independently.
- No `BULK_ACTION` table was introduced.

## Stored Procedure Compliance

The entire application C# source under `src/JamunaBank.Procurement.API` was searched case-insensitively for:

- `SELECT`
- `INSERT`
- `UPDATE`
- `DELETE`
- `FromSql`
- `ExecuteSql`
- `DbSet`
- `DbContext`
- `SqlQuery`
- `CommandType.Text`

Results:

- `FromSql`, `ExecuteSql`, `DbSet`, `DbContext`, `SqlQuery`, and `CommandType.Text`: **0 matches**.
- `INSERT`: **0 matches**.
- `SELECT`, `UPDATE`, and `DELETE` matches were C# LINQ method names, DTO/controller/service names such as `UpdateAsync`, and HTTP attributes such as `HttpDelete`; none were SQL command text.
- The only `SqlCommand.CommandText` assignment is in `StoredProcedureExecutor.CreateCommand`, which immediately sets `CommandType = CommandType.StoredProcedure`.
- All **69 unique stored-procedure names** referenced by C# were found in the database SQL scripts.
- No raw SQL string literal or alternate ADO.NET command construction was found.

Conclusion: **no stored-procedure-only violation was found**. Every reviewed application database operation flows through `IStoredProcedureExecutor` and uses `CommandType.StoredProcedure`.

## Test Results

- `dotnet build JamunaBank.Procurement.slnx --no-restore --configuration Debug`: **succeeded**, 0 errors, 2 `NU1900` warnings.
- `dotnet test JamunaBank.Procurement.slnx --no-restore --configuration Debug`: **exit code 0**, but no tests were discovered because the test project is an executable console harness.
- Executable unit-test harness: **60 passed, 0 failed**.
- SQL/database integration tests: **not run**, blocked by the environment's SQL Server SSPI authentication failure.

## Final Assessment

The API is architecturally consistent and fully compliant with the stored-procedure-only database rule. The reviewed workflow, bulk, tracking, and Work Order behavior is represented in server-side procedures rather than React. It is suitable for continued development, but it is not production-ready until the official authentication provider, object-level supporting-data authorization, and SQL-backed integration verification are completed.
