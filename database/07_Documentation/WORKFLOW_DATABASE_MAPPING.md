# Workflow database mapping

1. Maker creates a draft with item snapshots and normalized dynamic specification values, then submits it to the manager pool.
2. Manager approval releases the manager assignment and creates the procurement-officer pool assignment.
3. A mapped Procurement Officer atomically claims the pool request. Approval evaluates every item against active, date-effective amount rules. Incompatible item routes are rejected.
4. The selected rule routes to Procurement Authority before the concern division when configured, directly to the concern division, or to the configured destination.
5. A Concern Official independently chooses verify-and-complete, forward to a Concern Authority, return, or not-recommend. No matrix column requires Concern Authority involvement.
6. Concern Authority approval resumes the rule's `DESTINATION_AFTER_CONCERN`. Procurement Authority uses an OR pool: the first valid transition locks and advances the single workflow instance.
7. Entering `READY_FOR_PROCUREMENT` atomically completes the workflow instance and creates tracker/current-status/history rows.
8. Procurement status changes append history. A work order may be assigned to several requisitions in one transaction and advances each tracker to `WORK_ORDER_ISSUED`.

Every material transition writes `APPROVAL_ACTION`, `WORKFLOW_HISTORY`, updates ownership, and changes current state within a transaction. A shared batch GUID groups independently processed bulk calls; no bulk tables exist.
