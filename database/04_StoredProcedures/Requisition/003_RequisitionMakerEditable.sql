USE [JamunaBankProcurement];
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Requisition_SaveMakerEditable
 @RequisitionId BIGINT=NULL,@RequestedByEmployeeId VARCHAR(30),@RequestedOrgUnitId BIGINT,
 @Justification NVARCHAR(2000),@Items dbo.RequisitionItemTableType READONLY,
 @SpecValues dbo.RequisitionSpecValueTableType READONLY,@SpecOptions dbo.RequisitionSpecOptionTableType READONLY,
 @Actor VARCHAR(50)
AS
BEGIN
 SET NOCOUNT ON;
 EXEC dbo.usp_Requisition_SaveDraft
  @RequisitionId,@RequestedByEmployeeId,@RequestedOrgUnitId,@Justification,
  @Items,@SpecValues,@SpecOptions,@Actor,1;
END;
GO
