SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRANSACTION;

DECLARE @Categories TABLE
(
    CATEGORY_CODE VARCHAR(30) NOT NULL,
    CATEGORY_NAME NVARCHAR(150) NOT NULL,
    DESCRIPTION NVARCHAR(500) NULL
);

INSERT @Categories (CATEGORY_CODE, CATEGORY_NAME, DESCRIPTION)
VALUES
    ('IT',        N'IT Equipment',                N'Computers, printers, network devices and related technology equipment.'),
    ('OFFICE',    N'Office Equipment',            N'General office machines and operational equipment.'),
    ('FURNITURE', N'Furniture & Fixtures',        N'Office furniture, fixtures and workspace fittings.'),
    ('ELECTRICAL',N'Electrical & Electronics',    N'Electrical appliances and electronic equipment.'),
    ('SECURITY',  N'Security Equipment',          N'Physical security, surveillance and access-control equipment.'),
    ('VEHICLE',   N'Vehicles',                    N'Bank vehicles and related transport assets.'),
    ('FACILITY',  N'Facility & Maintenance',      N'Building, HVAC and facilities-maintenance equipment.');

UPDATE existing
SET existing.CATEGORY_NAME = source.CATEGORY_NAME,
    existing.DESCRIPTION = source.DESCRIPTION,
    existing.IS_ACTIVE = 1,
    existing.MODIFIED_BY = 'development-seed',
    existing.MODIFIED_DATE = SYSUTCDATETIME()
FROM dbo.ASSET_CATEGORY existing
JOIN @Categories source ON source.CATEGORY_CODE = existing.CATEGORY_CODE;

INSERT dbo.ASSET_CATEGORY (CATEGORY_CODE, CATEGORY_NAME, DESCRIPTION, CREATED_BY)
SELECT source.CATEGORY_CODE, source.CATEGORY_NAME, source.DESCRIPTION, 'development-seed'
FROM @Categories source
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.ASSET_CATEGORY existing
    WHERE existing.CATEGORY_CODE = source.CATEGORY_CODE
);

COMMIT TRANSACTION;

SELECT ASSET_CATEGORY_ID, CATEGORY_CODE, CATEGORY_NAME, IS_ACTIVE
FROM dbo.ASSET_CATEGORY
WHERE CATEGORY_CODE IN ('IT','OFFICE','FURNITURE','ELECTRICAL','SECURITY','VEHICLE','FACILITY')
ORDER BY CATEGORY_NAME;
