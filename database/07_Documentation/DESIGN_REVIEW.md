# Database design review

## Decisions and improvements

| Requirement | Issue | Implemented improvement | Reason |
|---|---|---|---|
| Employee IDs are repeated as text | Production identity is external, but unrestricted text FKs would be weak | `APP_USER_CACHE` is a non-authentication reference/cache and employee columns reference its stable `EMPLOYEE_ID` | Preserves FK integrity without making this database an identity provider |
| Workflow rules contain amount ranges | SQL Server cannot enforce non-overlapping ranges with a CHECK constraint | `usp_WorkflowRule_Save` uses `UPDLOCK, HOLDLOCK` to reject overlapping active rules; evaluation uses priority and then specificity | Deterministic selection and concurrency-safe configuration |
| One current workflow instance | A plain unique constraint cannot filter completed rows | Filtered unique index on `WORKFLOW_INSTANCE(REQUISITION_ID)` where `COMPLETED_DATE IS NULL` | Allows history while guaranteeing one active instance |
| Assignment can represent pools and individuals | Exclusivity differs by assignment type | A filtered unique index prevents more than one current assignment row per requisition and type | Prevents duplicate claims while retaining released history |
| Concern Division action table was suggested for review | It would duplicate action and movement facts | Not created; `APPROVAL_ACTION` records decisions and `WORKFLOW_HISTORY` records movement | Avoids two transactional sources of truth |
| Generic attachments | A polymorphic FK cannot be expressed declaratively | Retained as an explicit metadata registry; `usp_Attachment_Add` validates supported entity types and entity existence | Supports extensibility while putting integrity checks at the only application write boundary |
| Dynamic specification history | Master labels/options can change | Snapshot name, type and option text are stored with requisition values | Historical readability is independent of later configuration |
| Approval history and audit log overlap | They serve different audiences | Approval/history are immutable business facts; `AUDIT_LOG` is a technical before/after trail | Keeps workflow reporting separate from technical traceability |
| Monetary total can drift | Header totals are derived from active items | Creation procedure calculates item totals and header total in one transaction; direct table writes are denied to app roles later | Consistency at the stored-procedure boundary |
| Workflow rules apply per asset while a requisition can contain several assets | A single header workflow cannot represent conflicting item routes | Submission rejects mixed items whose evaluated rules resolve to different route signatures | Phase 1 keeps header-level workflow deterministic; item-level workflow can be introduced later if business approves split routing |

## Explicit assumptions

- Manager is resolved as the active `MANAGER` role user for the requester's organization in development data. Production identity/manager resolution will be supplied by the User Management API, but the actor is still validated by database role membership.
- A requisition's active items must resolve to one compatible route. This is safer than silently choosing the highest route.
- `AUTHORITY_BEFORE_DIVISION=1` requires a concern division. `DESTINATION_AFTER_CONCERN` is `READY_FOR_PROCUREMENT` or `PROCUREMENT_AUTHORITY`.
- Concern Authority is always selected operationally by a Concern Official and is never required by `WORKFLOW_RULE`.
- Important transactional and history tables have no delete procedure. Application principals should receive EXECUTE only, not direct table DML.
- All timestamps use UTC via `SYSUTCDATETIME()`.

