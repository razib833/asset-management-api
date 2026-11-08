# Index rationale

Filtered unique indexes enforce single active workflow and assignment ownership. Search indexes cover requisition number/status/date and organization, item asset/type joins, workflow rule matching, owner queues, employee mappings, procurement status/reference, and work-order number. Foreign-key child indexes avoid scans during joins and parent validation. No index is added solely because a column exists.
