USE [JamunaBankProcurement];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @RequesterEmployeeId VARCHAR(30) = 'DEV-MAKER-01';
DECLARE @AuthorityEmployeeId VARCHAR(30) = 'DEV-CA-01';
DECLARE @OfficialEmployeeId VARCHAR(30) = 'DEV-CO-01';
DECLARE @RequesterOrgUnitId BIGINT;
DECLARE @ConcernDivisionId BIGINT;
DECLARE @ConcernOrgUnitId BIGINT;

SELECT @RequesterOrgUnitId = ORG_UNIT_ID
FROM dbo.APP_USER_CACHE
WHERE EMPLOYEE_ID = @RequesterEmployeeId AND IS_ACTIVE = 1;

SELECT TOP (1)
    @ConcernDivisionId = a.CONCERN_DIVISION_ID,
    @ConcernOrgUnitId = s.ORG_UNIT_ID
FROM dbo.CONCERN_DIVISION_AUTHORITY a
JOIN dbo.CONCERN_DIVISION_SETUP s ON s.CONCERN_DIVISION_ID = a.CONCERN_DIVISION_ID
WHERE a.EMPLOYEE_ID = @AuthorityEmployeeId
  AND a.IS_ACTIVE = 1
  AND s.IS_ACTIVE = 1
  AND a.EFFECTIVE_FROM <= CONVERT(DATE, SYSUTCDATETIME())
  AND (a.EFFECTIVE_TO IS NULL OR a.EFFECTIVE_TO >= CONVERT(DATE, SYSUTCDATETIME()))
ORDER BY a.AUTHORITY_LEVEL, a.CONCERN_AUTHORITY_ID;

IF @RequesterOrgUnitId IS NULL
BEGIN
    ;THROW 51001, 'The development Maker user was not found.', 1;
END;

IF @ConcernDivisionId IS NULL
BEGIN
    ;THROW 51002, 'DEV-CA-01 is not mapped to an active Concern Division Authority pool.', 1;
END;

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.CONCERN_DIVISION_OFFICIAL
    WHERE CONCERN_DIVISION_ID = @ConcernDivisionId
      AND EMPLOYEE_ID = @OfficialEmployeeId
      AND IS_ACTIVE = 1
)
BEGIN
    ;THROW 51003, 'DEV-CO-01 is not mapped as an active official for the authority division.', 1;
END;

DECLARE @Rules TABLE
(
    DestinationRow INT NOT NULL,
    WorkflowRuleId BIGINT NOT NULL,
    AssetId BIGINT NOT NULL,
    AssetName NVARCHAR(200) NOT NULL,
    RequisitionType VARCHAR(20) NOT NULL,
    Destination VARCHAR(30) NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL
);

;WITH CandidateRules AS
(
    SELECT
        r.WORKFLOW_RULE_ID,
        r.ASSET_ID,
        a.ASSET_NAME,
        r.REQ_TYPE,
        r.DESTINATION_AFTER_CONCERN,
        CONVERT(DECIMAL(18,2),
            CASE
                WHEN r.MIN_AMOUNT IS NOT NULL AND r.MIN_AMOUNT > 0 THEN r.MIN_AMOUNT
                WHEN r.MAX_AMOUNT IS NOT NULL AND r.MAX_AMOUNT < 100000 THEN r.MAX_AMOUNT
                ELSE 100000
            END) AS UNIT_PRICE,
        ROW_NUMBER() OVER
        (
            PARTITION BY r.DESTINATION_AFTER_CONCERN
            ORDER BY r.PRIORITY_ORDER, r.WORKFLOW_RULE_ID
        ) AS RN
    FROM dbo.WORKFLOW_RULE r
    JOIN dbo.ASSET_MASTER a ON a.ASSET_ID = r.ASSET_ID AND a.IS_ACTIVE = 1
    WHERE r.CONCERN_DIVISION_ID = @ConcernDivisionId
      AND r.IS_ACTIVE = 1
      AND r.EFFECTIVE_FROM <= CONVERT(DATE, SYSUTCDATETIME())
      AND (r.EFFECTIVE_TO IS NULL OR r.EFFECTIVE_TO >= CONVERT(DATE, SYSUTCDATETIME()))
)
INSERT @Rules
    (DestinationRow, WorkflowRuleId, AssetId, AssetName, RequisitionType, Destination, UnitPrice)
SELECT
    ROW_NUMBER() OVER (ORDER BY DESTINATION_AFTER_CONCERN),
    WORKFLOW_RULE_ID,
    ASSET_ID,
    ASSET_NAME,
    REQ_TYPE,
    DESTINATION_AFTER_CONCERN,
    UNIT_PRICE
FROM CandidateRules
WHERE RN = 1;

IF NOT EXISTS (SELECT 1 FROM @Rules)
BEGIN
    ;THROW 51004, 'No active workflow rule is available for the Concern Authority test division.', 1;
END;

DECLARE @TestRows TABLE
(
    RowNumber INT IDENTITY(1,1) NOT NULL,
    RequisitionNo VARCHAR(30) NOT NULL,
    RuleDestinationRow INT NOT NULL,
    Quantity DECIMAL(18,2) NOT NULL,
    Justification NVARCHAR(2000) NOT NULL
);

INSERT @TestRows (RequisitionNo, RuleDestinationRow, Quantity, Justification)
VALUES
    ('TEST-CA-2026-0001', 1, 1, N'Concern Division Authority individual approval test request.'),
    ('TEST-CA-2026-0002', 1, 2, N'Concern Division Authority return or reject test request.'),
    ('TEST-CA-2026-0003', 1, 1, N'Concern Division Authority bulk approval test request.'),
    ('TEST-CA-2026-0004', 2, 1, N'Concern Division Authority mixed-destination bulk test request.'),
    ('TEST-CA-2026-0005', 2, 2, N'Concern Division Authority second mixed-destination bulk test request.');

DECLARE @RowNumber INT = 1;
DECLARE @RowCount INT = (SELECT COUNT(*) FROM @TestRows);

WHILE @RowNumber <= @RowCount
BEGIN
    DECLARE @RequisitionNo VARCHAR(30);
    DECLARE @RuleDestinationRow INT;
    DECLARE @Quantity DECIMAL(18,2);
    DECLARE @Justification NVARCHAR(2000);
    DECLARE @RuleId BIGINT;
    DECLARE @AssetId BIGINT;
    DECLARE @AssetName NVARCHAR(200);
    DECLARE @RequisitionType VARCHAR(20);
    DECLARE @UnitPrice DECIMAL(18,2);
    DECLARE @RequisitionId BIGINT;

    SELECT
        @RequisitionNo = RequisitionNo,
        @RuleDestinationRow = RuleDestinationRow,
        @Quantity = Quantity,
        @Justification = Justification
    FROM @TestRows
    WHERE RowNumber = @RowNumber;

    IF NOT EXISTS (SELECT 1 FROM @Rules WHERE DestinationRow = @RuleDestinationRow)
        SET @RuleDestinationRow = 1;

    SELECT
        @RuleId = WorkflowRuleId,
        @AssetId = AssetId,
        @AssetName = AssetName,
        @RequisitionType = RequisitionType,
        @UnitPrice = UnitPrice
    FROM @Rules
    WHERE DestinationRow = @RuleDestinationRow;

    IF NOT EXISTS (SELECT 1 FROM dbo.REQUISITION_HEADER WHERE REQUISITION_NO = @RequisitionNo)
    BEGIN
        BEGIN TRAN;

        INSERT dbo.REQUISITION_HEADER
            (REQUISITION_NO, REQUESTED_BY_EMPLOYEE_ID, REQUESTED_ORG_UNIT_ID, REQUESTED_DATE,
             JUSTIFICATION, TOTAL_ESTIMATED_AMOUNT, CURRENT_STATUS, CREATED_BY)
        VALUES
            (@RequisitionNo, @RequesterEmployeeId, @RequesterOrgUnitId,
             DATEADD(MINUTE, -(@RowNumber * 20), SYSUTCDATETIME()),
             @Justification, @Quantity * @UnitPrice, 'IN_WORKFLOW', 'development-seed');

        SET @RequisitionId = SCOPE_IDENTITY();

        INSERT dbo.REQUISITION_ITEM
            (REQUISITION_ID, ASSET_ID, ITEM_NAME_SNAPSHOT, REQ_TYPE, QUANTITY,
             ESTIMATED_UNIT_PRICE, PURPOSE_JUSTIFICATION, ADDITIONAL_SPEC_REQUIREMENT, CREATED_BY)
        VALUES
            (@RequisitionId, @AssetId, @AssetName, @RequisitionType, @Quantity,
             @UnitPrice, @Justification, N'Concern Authority approval workflow test data.', 'development-seed');

        INSERT dbo.WORKFLOW_INSTANCE
            (REQUISITION_ID, WORKFLOW_RULE_ID, CURRENT_STAGE, CURRENT_OWNER_TYPE,
             CURRENT_OWNER_ORG_UNIT_ID, CURRENT_STATUS, STARTED_DATE)
        VALUES
            (@RequisitionId, @RuleId, 'CONCERN_AUTHORITY', 'ORG_UNIT',
             @ConcernOrgUnitId, 'PENDING', DATEADD(HOUR, -1, SYSUTCDATETIME()));

        INSERT dbo.REQUISITION_ASSIGNMENT
            (REQUISITION_ID, ASSIGNMENT_TYPE, ASSIGNED_TO_ORG_UNIT_ID,
             ASSIGNED_DATE, DUE_DATE, CREATED_BY)
        VALUES
            (@RequisitionId, 'CONCERN_AUTHORITY', @ConcernOrgUnitId,
             DATEADD(HOUR, -1, SYSUTCDATETIME()), DATEADD(DAY, 5, SYSUTCDATETIME()), 'development-seed');

        INSERT dbo.APPROVAL_ACTION
            (REQUISITION_ID, ACTION_TYPE, ACTION_BY_EMPLOYEE_ID, ACTION_ROLE,
             FROM_STAGE, TO_STAGE, ACTION_DATETIME, REMARKS)
        VALUES
            (@RequisitionId, 'MANAGER_APPROVE', 'DEV-MGR-01', 'MANAGER',
             'MANAGER', 'PROCUREMENT_OFFICER', DATEADD(HOUR, -4, SYSUTCDATETIME()), N'Approved by the branch manager.'),
            (@RequisitionId, 'PROCUREMENT_OFFICER_APPROVE', 'DEV-PO-01', 'PROCUREMENT_OFFICER',
             'PROCUREMENT_OFFICER', 'CONCERN_DIVISION', DATEADD(HOUR, -3, SYSUTCDATETIME()), N'Reviewed and forwarded to the concern division.'),
            (@RequisitionId, 'CONCERN_FORWARD_TO_AUTHORITY', @OfficialEmployeeId, 'CONCERN_OFFICIAL',
             'CONCERN_DIVISION', 'CONCERN_AUTHORITY', DATEADD(HOUR, -1, SYSUTCDATETIME()), N'Technical requirements verified. Forwarded for division authority approval.');

        INSERT dbo.WORKFLOW_HISTORY
            (REQUISITION_ID, WORKFLOW_RULE_ID, FROM_STAGE, TO_STAGE, ACTION_TYPE,
             ACTION_BY_EMPLOYEE_ID, ACTION_DATETIME, REMARKS)
        VALUES
            (@RequisitionId, @RuleId, 'MANAGER', 'PROCUREMENT_OFFICER', 'MANAGER_APPROVE',
             'DEV-MGR-01', DATEADD(HOUR, -4, SYSUTCDATETIME()), N'Approved by the branch manager.'),
            (@RequisitionId, @RuleId, 'PROCUREMENT_OFFICER', 'CONCERN_DIVISION', 'PROCUREMENT_OFFICER_APPROVE',
             'DEV-PO-01', DATEADD(HOUR, -3, SYSUTCDATETIME()), N'Reviewed and forwarded to the concern division.'),
            (@RequisitionId, @RuleId, 'CONCERN_DIVISION', 'CONCERN_AUTHORITY', 'CONCERN_FORWARD_TO_AUTHORITY',
             @OfficialEmployeeId, DATEADD(HOUR, -1, SYSUTCDATETIME()), N'Technical requirements verified. Forwarded for division authority approval.');

        COMMIT;
    END;

    SET @RowNumber += 1;
END;

SELECT
    h.REQUISITION_NO,
    a.ASSET_NAME,
    r.DESTINATION_AFTER_CONCERN AS NEXT_DESTINATION,
    h.TOTAL_ESTIMATED_AMOUNT
FROM dbo.REQUISITION_HEADER h
JOIN dbo.WORKFLOW_INSTANCE w ON w.REQUISITION_ID = h.REQUISITION_ID
JOIN dbo.WORKFLOW_RULE r ON r.WORKFLOW_RULE_ID = w.WORKFLOW_RULE_ID
JOIN dbo.ASSET_MASTER a ON a.ASSET_ID = r.ASSET_ID
WHERE h.REQUISITION_NO LIKE 'TEST-CA-2026-%'
ORDER BY h.REQUISITION_NO;
GO
