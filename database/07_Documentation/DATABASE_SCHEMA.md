# Database schema

The design contains 33 normalized tables. All business writes are exposed through stored procedures; history tables are append-only for application usage.

## Main relationships

- `ORG_UNIT` self-references for organization hierarchy and has many cached users and requisitions.
- `ASSET_CATEGORY` 1—many `ASSET_MASTER`; asset 1—many specification definitions; definition 1—many options.
- Asset 1—many procurement-officer mappings. Concern setup belongs to one division and has many officials and many authorities.
- Requisition header 1—many items; an item has zero/one replacement record, many specification values, and each specification value has many selected option rows.
- Workflow rule belongs to an asset and optional concern division. Requisition has at most one open workflow instance, many assignments, actions, and movement-history rows.
- Requisition has zero/one procurement tracker; tracker has many immutable status-history rows.
- Work order and requisition are many-to-many through `WORK_ORDER_REQUISITION`.
- Comments attach to a requisition and optionally an item. Attachment metadata uses a validated polymorphic entity reference.

## Keys and cardinality safeguards

Every entity uses a `BIGINT IDENTITY` PK except stable employee cache keys and associative composite keys. Business codes and numbers are unique. Filtered unique indexes enforce one open workflow instance and one current assignment per type. The work-order bridge has composite PK `(WORK_ORDER_ID, REQUISITION_ID)`. Requisition-item specifications and selected options are unique within their parents.
