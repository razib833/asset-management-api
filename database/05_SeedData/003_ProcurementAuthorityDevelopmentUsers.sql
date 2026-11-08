USE [JamunaBankProcurement];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @ProcurementOrgUnitId BIGINT =
(
    SELECT TOP (1) ORG_UNIT_ID
    FROM dbo.ORG_UNIT
    WHERE ORG_UNIT_CODE = 'PROC'
      AND IS_ACTIVE = 1
);

IF @ProcurementOrgUnitId IS NULL
BEGIN
    ;THROW 51000, 'The active Procurement Division organization unit was not found.', 1;
END;

IF NOT EXISTS (SELECT 1 FROM dbo.APP_USER_CACHE WHERE EMPLOYEE_ID = 'DEV-PA-01')
BEGIN
    INSERT dbo.APP_USER_CACHE
        (EMPLOYEE_ID, DISPLAY_NAME, EMAIL, DESIGNATION, ORG_UNIT_ID)
    VALUES
        ('DEV-PA-01', 'Procurement Authority A', 'pa1@example.invalid', 'Head of Procurement', @ProcurementOrgUnitId);
END;

IF NOT EXISTS (SELECT 1 FROM dbo.APP_USER_CACHE WHERE EMPLOYEE_ID = 'DEV-PA-02')
BEGIN
    INSERT dbo.APP_USER_CACHE
        (EMPLOYEE_ID, DISPLAY_NAME, EMAIL, DESIGNATION, ORG_UNIT_ID)
    VALUES
        ('DEV-PA-02', 'Procurement Authority B', 'pa2@example.invalid', 'Operations Head - Procurement', @ProcurementOrgUnitId);
END;

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.PROC_AUTHORITY
    WHERE EMPLOYEE_ID = 'DEV-PA-01'
      AND AUTHORITY_TYPE = 'HEAD'
      AND IS_ACTIVE = 1
)
BEGIN
    INSERT dbo.PROC_AUTHORITY
        (EMPLOYEE_ID, AUTHORITY_TYPE, EFFECTIVE_FROM, CREATED_BY)
    VALUES
        ('DEV-PA-01', 'HEAD', '2026-01-01', 'development-seed');
END;

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.PROC_AUTHORITY
    WHERE EMPLOYEE_ID = 'DEV-PA-02'
      AND AUTHORITY_TYPE = 'OPERATIONS_HEAD'
      AND IS_ACTIVE = 1
)
BEGIN
    INSERT dbo.PROC_AUTHORITY
        (EMPLOYEE_ID, AUTHORITY_TYPE, EFFECTIVE_FROM, CREATED_BY)
    VALUES
        ('DEV-PA-02', 'OPERATIONS_HEAD', '2026-01-01', 'development-seed');
END;
GO
