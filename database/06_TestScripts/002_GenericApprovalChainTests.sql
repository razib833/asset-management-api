USE [JamunaBankProcurement];
GO
SET NOCOUNT ON;

IF OBJECT_ID('dbo.APPROVAL_UNIT','U') IS NULL THROW 51001, 'APPROVAL_UNIT was not created.', 1;
IF OBJECT_ID('dbo.APPROVAL_UNIT_APPROVER','U') IS NULL THROW 51002, 'APPROVAL_UNIT_APPROVER was not created.', 1;
IF OBJECT_ID('dbo.WORKFLOW_RULE_STEP','U') IS NULL THROW 51003, 'WORKFLOW_RULE_STEP was not created.', 1;
IF OBJECT_ID('dbo.WORKFLOW_INSTANCE_STEP','U') IS NULL THROW 51004, 'WORKFLOW_INSTANCE_STEP was not created.', 1;
IF OBJECT_ID('dbo.WORKFLOW_INSTANCE_STEP_APPROVER','U') IS NULL THROW 51005, 'WORKFLOW_INSTANCE_STEP_APPROVER was not created.', 1;
IF OBJECT_ID('dbo.WORKFLOW_STEP_ACTION','U') IS NULL THROW 51006, 'WORKFLOW_STEP_ACTION was not created.', 1;

IF OBJECT_ID('dbo.usp_ApprovalUnit_Upsert','P') IS NULL THROW 51007, 'Approval-unit procedure was not created.', 1;
IF OBJECT_ID('dbo.usp_ApprovalUnitApprover_Upsert','P') IS NULL THROW 51008, 'Approver procedure was not created.', 1;
IF OBJECT_ID('dbo.usp_WorkflowRuleStep_Upsert','P') IS NULL THROW 51009, 'Rule-step procedure was not created.', 1;
IF OBJECT_ID('dbo.usp_WorkflowApprovalChain_Initialize','P') IS NULL THROW 51010, 'Initialization procedure was not created.', 1;
IF OBJECT_ID('dbo.usp_WorkflowApprovalChain_Action','P') IS NULL THROW 51011, 'Action procedure was not created.', 1;

PRINT 'Generic approval-chain database objects are present.';
GO
