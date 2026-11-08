# Stored procedure catalog

All command procedures return `RESULT_CODE`, `RESULT_MESSAGE`, `ENTITY_ID`, and `REFERENCE_NO`. Query procedures return result sets. Transactional commands use `XACT_ABORT`, explicit transactions, locking, and `TRY/CATCH` where multiple writes occur.

| Procedure(s) | Purpose / key parameters | Tables read or affected |
|---|---|---|
| `usp_Common_GetLookup`, `usp_Organization_GetAll`, `usp_DevAuth_GetUser` | Reference and development identity reads | lookup, organization, dev-user tables |
| `usp_Audit_GetByEntity` | Technical audit read by entity key | `AUDIT_LOG` |
| `usp_AssetCategory_GetAll`, `usp_Asset_GetAll`, `usp_Asset_GetById` | Asset-master queries and specification expansion | asset tables |
| `usp_Asset_Create`, `usp_Asset_Update` | Create/update/deactivate asset; actor and asset fields | `ASSET_MASTER` |
| `usp_AssetSpec_GetByAsset` | Definitions and option rows by asset | specification tables |
| `usp_AssetOfficerMap_Save` | Add effective officer mapping | `PROC_OFFICER_ASSET_MAP` |
| `usp_WorkflowRule_Evaluate`, `usp_WorkflowRule_GetByAsset` | Deterministic amount/date rule matching | `WORKFLOW_RULE` |
| `usp_WorkflowRule_Save` | Concurrency-safe create/update and overlap rejection | `WORKFLOW_RULE` |
| `usp_Requisition_Create` | Header plus item TVP and replacement details | requisition, item, replacement, audit |
| `usp_Requisition_GetById`, `usp_Requisition_Search` | Full detail and indexed operational search | requisition, item, spec, action tables |
| `usp_Requisition_Submit` | Validate mandatory specs; submit to manager | header, assignment, action, history |
| `usp_Manager_GetPending` | Manager queue scoped to manager organization | header and dev-role references |
| `usp_Manager_Approve/Return/Reject` | Manager state transitions | header, assignment, action, history |
| `usp_ProcurementOfficer_GetAssigned`, `usp_ProcurementOfficer_TakeForReview` | Eligible queue and atomic claim | mapping, assignment, action |
| `usp_ProcurementOfficer_Approve` | Evaluate all item rules and initialize workflow | rule, instance, assignment, action/history |
| `usp_ProcurementAuthority_GetPending/Approve/Reject` | OR-pool queue and authority transitions | authority, workflow, action/history |
| `usp_ConcernDivision_GetPending/VerifyComplete/ForwardToAuthority/Return/NotRecommend` | Concern Official operational choices | setup, workflow, assignment, action/history |
| `usp_ConcernAuthority_GetPending/Approve/Return/Reject` | Concern Authority decisions and matrix continuation | authority, workflow, action/history |
| `usp_Workflow_Advance`, `usp_Workflow_SimpleTerminal` | Internal transactional transition engines | workflow, assignment, action/history, tracker |
| `usp_ProcurementTracking_Search/UpdateStatus/GetHistory` | Tracker search, atomic status update, immutable history | tracking and status tables |
| `usp_WorkOrder_CreateAndAssign` | Create one work order for TVP requisitions and update status | work order, bridge, tracking/history |
| `usp_WorkOrder_GetByRequisition` | Work orders linked to a requisition | work-order tables |
| `usp_Comment_Add/GetByRequisition` | Add/read non-transition observations | `COMMENT_NOTE` |
| `usp_Attachment_Add/GetByEntity` | Validate polymorphic target and add/read metadata | attachment and target tables |

TVPs: `RequisitionItemTableType` carries item and replacement inputs; `WorkOrderRequisitionTableType` carries multi-requisition allocations.

