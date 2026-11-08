USE [JamunaBankProcurement];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @RuleId BIGINT;
DECLARE @AssetId BIGINT;
DECLARE @AssetName NVARCHAR(200);
DECLARE @RequisitionType VARCHAR(20);
DECLARE @RequesterOrgUnitId BIGINT;

SELECT TOP (1)
    @RuleId = r.WORKFLOW_RULE_ID,
    @AssetId = r.ASSET_ID,
    @AssetName = a.ASSET_NAME,
    @RequisitionType = r.REQ_TYPE
FROM dbo.WORKFLOW_RULE r
JOIN dbo.ASSET_MASTER a ON a.ASSET_ID = r.ASSET_ID
WHERE r.IS_ACTIVE = 1
  AND r.AUTHORITY_BEFORE_DIVISION = 1
  AND r.EFFECTIVE_FROM <= CONVERT(DATE, SYSUTCDATETIME())
  AND (r.EFFECTIVE_TO IS NULL OR r.EFFECTIVE_TO >= CONVERT(DATE, SYSUTCDATETIME()))
ORDER BY r.PRIORITY_ORDER, r.WORKFLOW_RULE_ID;

SELECT @RequesterOrgUnitId = ORG_UNIT_ID
FROM dbo.APP_USER_CACHE
WHERE EMPLOYEE_ID = 'DEV-MAKER-01';

IF @RuleId IS NULL
BEGIN
    ;THROW 51000, 'An active workflow rule with Procurement Authority before Concern Division is required for test requisitions.', 1;
END;

IF @RequesterOrgUnitId IS NULL
BEGIN
    ;THROW 51001, 'The development Maker user was not found.', 1;
END;

DECLARE @TestRows TABLE
(
    RowNumber INT NOT NULL,
    RequisitionNo VARCHAR(30) NOT NULL,
    Quantity DECIMAL(18,2) NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    Justification NVARCHAR(2000) NOT NULL
);

INSERT @TestRows (RowNumber, RequisitionNo, Quantity, UnitPrice, Justification)
VALUES
    (1, 'TEST-PA-2026-0001', 2, 425000, N'Development test request for Procurement Authority individual approval.'),
    (2, 'TEST-PA-2026-0002', 1, 650000, N'Development test request for Procurement Authority rejection.'),
    (3, 'TEST-PA-2026-0003', 3, 300000, N'Development test request for Procurement Authority bulk approval.');

DECLARE @RowNumber INT = 1;

WHILE @RowNumber <= 3
BEGIN
    DECLARE @RequisitionNo VARCHAR(30);
    DECLARE @Quantity DECIMAL(18,2);
    DECLARE @UnitPrice DECIMAL(18,2);
    DECLARE @Justification NVARCHAR(2000);
    DECLARE @RequisitionId BIGINT;

    SELECT
        @RequisitionNo = RequisitionNo,
        @Quantity = Quantity,
        @UnitPrice = UnitPrice,
        @Justification = Justification
    FROM @TestRows
    WHERE RowNumber = @RowNumber;

    IF NOT EXISTS (SELECT 1 FROM dbo.REQUISITION_HEADER WHERE REQUISITION_NO = @RequisitionNo)
    BEGIN
        BEGIN TRAN;

        INSERT dbo.REQUISITION_HEADER
            (REQUISITION_NO, REQUESTED_BY_EMPLOYEE_ID, REQUESTED_ORG_UNIT_ID, REQUESTED_DATE,
             JUSTIFICATION, TOTAL_ESTIMATED_AMOUNT, CURRENT_STATUS, CREATED_BY)
        VALUES
            (@RequisitionNo, 'DEV-MAKER-01', @RequesterOrgUnitId,
             DATEADD(MINUTE, -(@RowNumber * 30), SYSUTCDATETIME()),
             @Justification, @Quantity * @UnitPrice, 'IN_WORKFLOW', 'development-seed');

        SET @RequisitionId = SCOPE_IDENTITY();

        INSERT dbo.REQUISITION_ITEM
            (REQUISITION_ID, ASSET_ID, ITEM_NAME_SNAPSHOT, REQ_TYPE, QUANTITY,
             ESTIMATED_UNIT_PRICE, PURPOSE_JUSTIFICATION, ADDITIONAL_SPEC_REQUIREMENT, CREATED_BY)
        VALUES
            (@RequisitionId, @AssetId, @AssetName, @RequisitionType, @Quantity,
             @UnitPrice, @Justification, N'Procurement Authority workflow test data.', 'development-seed');

        INSERT dbo.WORKFLOW_INSTANCE
            (REQUISITION_ID, WORKFLOW_RULE_ID, CURRENT_STAGE, CURRENT_OWNER_TYPE,
             CURRENT_STATUS, STARTED_DATE)
        VALUES
            (@RequisitionId, @RuleId, 'PROCUREMENT_AUTHORITY', 'ROLE_POOL',
             'PENDING', DATEADD(HOUR, -2, SYSUTCDATETIME()));

        INSERT dbo.REQUISITION_ASSIGNMENT
            (REQUISITION_ID, ASSIGNMENT_TYPE, DUE_DATE, CREATED_BY)
        VALUES
            (@RequisitionId, 'PROCUREMENT_AUTHORITY_POOL', DATEADD(DAY, 5, SYSUTCDATETIME()), 'development-seed');

        INSERT dbo.APPROVAL_ACTION
            (REQUISITION_ID, ACTION_TYPE, ACTION_BY_EMPLOYEE_ID, ACTION_ROLE,
             FROM_STAGE, TO_STAGE, ACTION_DATETIME, REMARKS)
        VALUES
            (@RequisitionId, 'MANAGER_APPROVE', 'DEV-MGR-01', 'MANAGER',
             'MANAGER', 'PROCUREMENT_OFFICER', DATEADD(HOUR, -3, SYSUTCDATETIME()), N'Approved for procurement review.'),
            (@RequisitionId, 'PROCUREMENT_OFFICER_APPROVE', 'DEV-PO-01', 'PROCUREMENT_OFFICER',
             'PROCUREMENT_OFFICER', 'PROCUREMENT_AUTHORITY', DATEADD(HOUR, -2, SYSUTCDATETIME()), N'Reviewed and recommended for authority approval.');

        INSERT dbo.WORKFLOW_HISTORY
            (REQUISITION_ID, WORKFLOW_RULE_ID, FROM_STAGE, TO_STAGE, ACTION_TYPE,
             ACTION_BY_EMPLOYEE_ID, ACTION_DATETIME, REMARKS)
        VALUES
            (@RequisitionId, @RuleId, 'MANAGER', 'PROCUREMENT_OFFICER', 'MANAGER_APPROVE',
             'DEV-MGR-01', DATEADD(HOUR, -3, SYSUTCDATETIME()), N'Approved for procurement review.'),
            (@RequisitionId, @RuleId, 'PROCUREMENT_OFFICER', 'PROCUREMENT_AUTHORITY', 'PROCUREMENT_OFFICER_APPROVE',
             'DEV-PO-01', DATEADD(HOUR, -2, SYSUTCDATETIME()), N'Reviewed and recommended for authority approval.');

        COMMIT;
    END;

    SET @RowNumber += 1;
END;
GO
