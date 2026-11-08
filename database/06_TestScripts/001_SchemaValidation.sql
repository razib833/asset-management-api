USE [JamunaBankProcurement];
GO
SET NOCOUNT ON;
DECLARE @failures TABLE(TEST_NAME NVARCHAR(200),DETAIL NVARCHAR(2000));
IF EXISTS(SELECT 1 FROM sys.tables WHERE name IN('BULK_ACTION_BATCH','BULK_ACTION_ITEM')) INSERT @failures VALUES('No bulk tables','Prohibited bulk table exists');
IF EXISTS(SELECT 1 FROM sys.tables WHERE name='CONCERN_DIVISION_ACTION') INSERT @failures VALUES('No redundant concern action table','Redundant table exists');
IF (SELECT COUNT(*) FROM sys.foreign_keys)<30 INSERT @failures VALUES('Foreign keys','Expected at least 30 foreign keys');
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name='UX_WFI_ACTIVE_REQ' AND has_filter=1 AND is_unique=1) INSERT @failures VALUES('Current workflow uniqueness','Filtered unique index missing');
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name='UX_ASSIGN_CURRENT_TYPE' AND has_filter=1 AND is_unique=1) INSERT @failures VALUES('Current assignment uniqueness','Filtered unique index missing');
IF EXISTS(SELECT 1 FROM sys.columns c JOIN sys.tables t ON t.object_id=c.object_id WHERE t.name='WORKFLOW_RULE' AND c.name LIKE '%CONCERN%AUTHORITY%REQUIRED%') INSERT @failures VALUES('Concern authority not matrix-mandated','Prohibited rule field exists');
IF EXISTS(SELECT 1 FROM sys.procedures p JOIN sys.sql_modules m ON m.object_id=p.object_id WHERE p.name LIKE 'usp[_]%' AND m.definition LIKE '%EXEC(%') INSERT @failures VALUES('No dynamic SQL','Dynamic EXEC found');
IF EXISTS(SELECT 1 FROM sys.sql_expression_dependencies d JOIN sys.procedures p ON p.object_id=d.referencing_id WHERE d.referenced_id IS NULL AND d.referenced_entity_name IS NOT NULL AND d.is_ambiguous=0) INSERT @failures SELECT 'Procedure dependency',CONCAT(p.name,' unresolved reference ',d.referenced_entity_name) FROM sys.sql_expression_dependencies d JOIN sys.procedures p ON p.object_id=d.referencing_id WHERE d.referenced_id IS NULL AND d.referenced_entity_name IS NOT NULL AND d.is_ambiguous=0;
SELECT TEST_NAME,DETAIL FROM @failures;
IF EXISTS(SELECT 1 FROM @failures) THROW 52000,'Schema validation failed.',1;
SELECT 'PASS' TEST_RESULT,'Schema metadata validation' TEST_NAME;
GO

