# Table dictionary

Notation: `PK` primary key, `FK` foreign key, `UQ` unique, `NN` not null. Audit columns use UTC `DATETIME2(3)` defaults. Full constraint names and exact defaults are authoritative in `01_Tables`.

## Organization and development identity

- `ORG_UNIT`: `ORG_UNIT_ID BIGINT PK identity`; `PARENT_ORG_UNIT_ID BIGINT FK nullable`; `ORG_UNIT_CODE VARCHAR(30) UQ NN`; `ORG_UNIT_NAME NVARCHAR(200) NN`; `ORG_UNIT_TYPE VARCHAR(20) NN` branch/division; `IS_ACTIVE BIT NN default 1`; created/modified actor and date columns.
- `ROLE_MASTER`: identity `ROLE_ID`; unique `ROLE_CODE`; name, description, active flag, created/modified audit fields.
- `APP_USER_CACHE`: `EMPLOYEE_ID VARCHAR(30) PK`; display name, optional email/designation; optional organization FK; active/refreshed/created/modified dates. External identity reference only.
- `DEV_USER`: identity key; employee FK and username are unique; fake development password hash; active and audit fields.
- `DEV_USER_ROLE`: composite `(DEV_USER_ID,ROLE_ID)` PK; optional org scope FK; active and created fields.
- `COMMON_LOOKUP`: identity key; category+code UQ; name, description, nonnegative display order, active flag.

## Configuration

- `ASSET_CATEGORY`: identity key; unique code; name, description, active and audit fields.
- `ASSET_MASTER`: identity key; category FK; unique code; name/description/UOM; optional warranty months, useful-life years, capitalization amount; criticality; inventory/depreciable/active flags; audit fields.
- `ASSET_SPEC_DEFINITION`: identity key; asset FK; unique asset+name; type, mandatory flag, order; nullable typed defaults; active and audit fields.
- `ASSET_SPEC_OPTION`: identity key; definition FK; unique definition+value; order, active and audit fields.
- `PROC_OFFICER_ASSET_MAP`: identity key; asset and employee FKs; primary/active flags; effective date range and audit fields.
- `CONCERN_DIVISION_SETUP`: identity key; unique organization FK and setup code; active/audit fields.
- `CONCERN_DIVISION_OFFICIAL`: identity key; setup and employee FKs; active, effective range, audit fields.
- `CONCERN_DIVISION_AUTHORITY`: identity key; setup and employee FKs; positive authority level; active/effective/audit fields.
- `PROC_AUTHORITY`: identity key; employee FK; authority type; active/effective/audit fields.
- `WORKFLOW_RULE`: identity key; unique code; name; asset FK; request type; nullable min/max amounts; optional concern setup FK; authority-before flag; post-concern destination; priority; active/effective/audit fields; rowversion. It deliberately has no Concern Authority required field.

## Requisition and workflow

- `REQUISITION_HEADER`: identity key; unique number; requester employee and organization FKs; requested date, justification, total amount, constrained current status, active/audit fields and rowversion.
- `REQUISITION_ITEM`: identity key; requisition and asset FKs; name snapshot; request type; positive quantity/nonnegative unit price; persisted computed total; purpose/additional specifications; active/audit fields.
- `REQUISITION_REPLACEMENT_INFO`: identity key; unique item FK (1:1); optional tag/serial; mandatory current condition; extra info and audit fields.
- `REQUISITION_ITEM_SPEC`: identity key; item and definition FKs, unique together; name/type snapshots; nullable typed values; mandatory/order snapshots and audit fields. At most one scalar value is allowed.
- `REQUISITION_ITEM_SPEC_OPTION`: identity key; requisition-spec and master-option FKs, unique together; selected text snapshot and created fields. Multiple rows support multi-select.
- `WORKFLOW_INSTANCE`: identity key; requisition/rule FKs; stage; owner type plus optional employee/org FKs; status; start/complete/create/modify dates and rowversion. Filtered uniqueness permits one open instance.
- `REQUISITION_ASSIGNMENT`: identity key; requisition FK; assignment type; optional employee/org targets; assigned/released/due dates; current flag and creator. Filtered uniqueness prevents conflicting current type ownership.
- `APPROVAL_ACTION`: identity key; requisition and optional item FKs; constrained action; actor employee FK/role; from/to stage; action date, remarks, optional batch GUID, created date. Immutable to application role.
- `WORKFLOW_HISTORY`: identity key; requisition and optional rule FKs; from/to stages; action and actor FK; action date, remarks, optional batch GUID. Immutable movement history.

## Procurement and supporting records

- `PROCUREMENT_STATUS_MASTER`: identity key; unique code; name/description/order/active fields.
- `PROCUREMENT_TRACKING`: identity key; unique requisition FK; current status and optional responsible employee FKs; reference, expected date, remarks, timestamps and rowversion.
- `PROCUREMENT_STATUS_HISTORY`: identity key; tracker/status/employee FKs; status date, reference, remarks, optional batch GUID, created date.
- `WORK_ORDER_HEADER`: identity key; unique number; order date; supplier; optional contract; nonnegative value; optional delivery date/remarks; audit fields and rowversion.
- `WORK_ORDER_REQUISITION`: composite work-order+requisition PK/FKs; optional nonnegative allocation and remarks.
- `COMMENT_NOTE`: identity key; requisition and optional item FKs; type/text; creator employee FK/date; active flag.
- `ATTACHMENT`: identity key; constrained entity type plus entity ID; stored/original names; storage key; extension/MIME/size/document type; uploader/date; active flag. Target integrity is validated by its add procedure.
- `AUDIT_LOG`: identity key; entity name/ID; action; optional valid JSON old/new values; optional employee FK; timestamp, IP and correlation GUID. Immutable to application role.
