using System.Data;
using JamunaBank.Procurement.API.Data;
using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;
using JamunaBank.Procurement.API.Repositories.Base;
using JamunaBank.Procurement.API.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace JamunaBank.Procurement.API.Repositories.Implementations;

public sealed class GenericApprovalChainRepository(IStoredProcedureExecutor procedures)
    : BaseRepository(procedures), IGenericApprovalChainRepository
{
    public Task<IReadOnlyList<ApprovalUnitDto>> GetUnitsAsync(bool activeOnly,CancellationToken ct) =>
        StoredProcedures.QueryAsync<ApprovalUnitDto>("dbo.usp_ApprovalUnit_GetAll",r => new(
            r.GetInt64(r.GetOrdinal("APPROVAL_UNIT_ID")),r.GetNullable<long>("PARENT_APPROVAL_UNIT_ID"),r.GetNullable<long>("ORG_UNIT_ID"),
            r.GetString(r.GetOrdinal("UNIT_CODE")),r.GetString(r.GetOrdinal("UNIT_NAME")),r.GetString(r.GetOrdinal("UNIT_TYPE")),r.GetBoolean(r.GetOrdinal("IS_ACTIVE")),
            r.GetNullableString("PARENT_UNIT_NAME"),r.GetNullableString("ORG_UNIT_CODE"),r.GetNullableString("ORG_UNIT_NAME"),r.GetInt32(r.GetOrdinal("ACTIVE_APPROVER_COUNT"))),
            [I("@ActiveOnly",SqlDbType.Bit,activeOnly)],ct);

    public Task<StoredProcedureResult?> SaveUnitAsync(long? id,SaveApprovalUnitRequest x,string actor,CancellationToken ct) =>
        StoredProcedures.QuerySingleOrDefaultAsync("dbo.usp_ApprovalUnit_Upsert",StoredProcedureResult.FromReader,
            [I("@ApprovalUnitId",SqlDbType.BigInt,id),I("@ParentApprovalUnitId",SqlDbType.BigInt,x.ParentApprovalUnitId),I("@OrgUnitId",SqlDbType.BigInt,x.OrgUnitId),
             I("@UnitCode",SqlDbType.NVarChar,x.Code.Trim(),50),I("@UnitName",SqlDbType.NVarChar,x.Name.Trim(),150),I("@UnitType",SqlDbType.VarChar,x.Type.Trim().ToUpperInvariant(),20),
             I("@IsActive",SqlDbType.Bit,x.IsActive),I("@ActorEmployeeId",SqlDbType.VarChar,actor,30)],ct);

    public Task<IReadOnlyList<ApprovalUnitApproverDto>> GetApproversAsync(long unitId,bool activeOnly,CancellationToken ct) =>
        StoredProcedures.QueryAsync<ApprovalUnitApproverDto>("dbo.usp_ApprovalUnitApprover_GetByUnit",r => new(
            r.GetInt64(r.GetOrdinal("APPROVAL_UNIT_APPROVER_ID")),r.GetInt64(r.GetOrdinal("APPROVAL_UNIT_ID")),r.GetString(r.GetOrdinal("EMPLOYEE_ID")),
            r.GetString(r.GetOrdinal("DISPLAY_NAME")),r.GetNullableString("EMAIL"),r.GetNullableString("DESIGNATION"),r.GetString(r.GetOrdinal("APPROVER_ROLE_CODE")),
            r.GetInt32(r.GetOrdinal("AUTHORITY_LEVEL")),ToDate(r,"EFFECTIVE_FROM"),ToDate(r,"EFFECTIVE_TO"),r.GetBoolean(r.GetOrdinal("IS_ACTIVE"))),
            [I("@ApprovalUnitId",SqlDbType.BigInt,unitId),I("@ActiveOnly",SqlDbType.Bit,activeOnly)],ct);

    public Task<StoredProcedureResult?> SaveApproverAsync(SaveApprovalUnitApproverRequest x,string actor,CancellationToken ct) =>
        StoredProcedures.QuerySingleOrDefaultAsync("dbo.usp_ApprovalUnitApprover_Upsert",StoredProcedureResult.FromReader,
            [I("@ApprovalUnitApproverId",SqlDbType.BigInt,x.Id),I("@ApprovalUnitId",SqlDbType.BigInt,x.ApprovalUnitId),I("@EmployeeId",SqlDbType.VarChar,x.EmployeeId.Trim(),30),
             I("@ApproverRoleCode",SqlDbType.VarChar,x.RoleCode.Trim().ToUpperInvariant(),50),I("@AuthorityLevel",SqlDbType.Int,x.AuthorityLevel),
             I("@EffectiveFrom",SqlDbType.Date,x.EffectiveFrom),I("@EffectiveTo",SqlDbType.Date,x.EffectiveTo),I("@IsActive",SqlDbType.Bit,x.IsActive),I("@ActorEmployeeId",SqlDbType.VarChar,actor,30)],ct);

    public Task<IReadOnlyList<WorkflowRuleStepDto>> GetRuleStepsAsync(long ruleId,CancellationToken ct) =>
        StoredProcedures.QueryAsync<WorkflowRuleStepDto>("dbo.usp_WorkflowRuleStep_GetByRule",r => new(
            r.GetInt64(r.GetOrdinal("WORKFLOW_RULE_STEP_ID")),r.GetInt64(r.GetOrdinal("WORKFLOW_RULE_ID")),r.GetInt32(r.GetOrdinal("STEP_ORDER")),
            r.GetString(r.GetOrdinal("STEP_CODE")),r.GetString(r.GetOrdinal("STEP_NAME")),r.GetString(r.GetOrdinal("APPROVER_SOURCE")),r.GetNullable<long>("APPROVAL_UNIT_ID"),
            r.GetNullableString("UNIT_CODE"),r.GetNullableString("UNIT_NAME"),r.GetNullableString("EMPLOYEE_ID"),r.GetNullableString("EMPLOYEE_NAME"),r.GetNullableString("APPROVER_ROLE_CODE"),
            r.GetString(r.GetOrdinal("APPROVAL_MODE")),r.GetInt32(r.GetOrdinal("REQUIRED_APPROVAL_COUNT")),r.GetBoolean(r.GetOrdinal("IS_OPTIONAL")),r.GetNullableString("COMPLETION_DESTINATION"),r.GetBoolean(r.GetOrdinal("IS_ACTIVE"))),
            [I("@WorkflowRuleId",SqlDbType.BigInt,ruleId)],ct);

    public Task<StoredProcedureResult?> SaveRuleStepAsync(long? id,SaveWorkflowRuleStepRequest x,string actor,CancellationToken ct) =>
        StoredProcedures.QuerySingleOrDefaultAsync("dbo.usp_WorkflowRuleStep_Upsert",StoredProcedureResult.FromReader,
            [I("@WorkflowRuleStepId",SqlDbType.BigInt,id),I("@WorkflowRuleId",SqlDbType.BigInt,x.WorkflowRuleId),I("@StepOrder",SqlDbType.Int,x.StepOrder),
             I("@StepCode",SqlDbType.VarChar,x.StepCode.Trim().ToUpperInvariant(),50),I("@StepName",SqlDbType.NVarChar,x.StepName.Trim(),150),I("@ApproverSource",SqlDbType.VarChar,x.ApproverSource.Trim().ToUpperInvariant(),40),
             I("@ApprovalUnitId",SqlDbType.BigInt,x.ApprovalUnitId),I("@EmployeeId",SqlDbType.VarChar,x.EmployeeId?.Trim(),30),I("@ApproverRoleCode",SqlDbType.VarChar,x.ApproverRoleCode?.Trim().ToUpperInvariant(),50),
             I("@ApprovalMode",SqlDbType.VarChar,x.ApprovalMode.Trim().ToUpperInvariant(),15),I("@RequiredApprovalCount",SqlDbType.Int,x.RequiredApprovalCount),I("@IsOptional",SqlDbType.Bit,x.IsOptional),
             I("@CompletionDestination",SqlDbType.VarChar,x.CompletionDestination?.Trim().ToUpperInvariant(),50),I("@IsActive",SqlDbType.Bit,x.IsActive),I("@ActorEmployeeId",SqlDbType.VarChar,actor,30)],ct);

    public Task<StoredProcedureResult?> InitializeAsync(long workflowInstanceId,string actor,CancellationToken ct) =>
        StoredProcedures.QuerySingleOrDefaultAsync("dbo.usp_WorkflowApprovalChain_Initialize",StoredProcedureResult.FromReader,
            [I("@WorkflowInstanceId",SqlDbType.BigInt,workflowInstanceId),I("@ActorEmployeeId",SqlDbType.VarChar,actor,30)],ct);

    public Task<IReadOnlyList<PendingGenericApprovalDto>> GetPendingAsync(string employeeId,CancellationToken ct) =>
        StoredProcedures.QueryAsync<PendingGenericApprovalDto>("dbo.usp_WorkflowApprovalChain_GetPending",r => new(
            r.GetInt64(r.GetOrdinal("REQUISITION_ID")),r.GetString(r.GetOrdinal("REQUISITION_NO")),r.GetInt64(r.GetOrdinal("WORKFLOW_INSTANCE_STEP_ID")),
            r.GetInt32(r.GetOrdinal("STEP_ORDER")),r.GetString(r.GetOrdinal("STEP_CODE")),r.GetString(r.GetOrdinal("STEP_NAME")),r.GetString(r.GetOrdinal("APPROVAL_MODE")),
            r.GetInt32(r.GetOrdinal("REQUIRED_APPROVAL_COUNT")),r.GetNullable<DateTime>("ACTIVATED_ON")),[I("@EmployeeId",SqlDbType.VarChar,employeeId,30)],ct);

    public async Task<RequisitionDetailDto?> GetReviewDetailAsync(long workflowInstanceStepId,string employeeId,CancellationToken ct) =>
        RequisitionDetailDataSetMapper.Map(await StoredProcedures.QueryDataSetAsync(
            "dbo.usp_WorkflowApprovalChain_GetReviewDetail",
            [I("@WorkflowInstanceStepId",SqlDbType.BigInt,workflowInstanceStepId),I("@EmployeeId",SqlDbType.VarChar,employeeId,30)],ct));

    public Task<IReadOnlyList<ApprovalChainEntryDto>> GetByRequisitionAsync(long requisitionId,CancellationToken ct) =>
        StoredProcedures.QueryAsync<ApprovalChainEntryDto>("dbo.usp_WorkflowApprovalChain_GetByRequisition",r => new(
            r.GetInt64(r.GetOrdinal("WORKFLOW_INSTANCE_STEP_ID")),r.GetInt64(r.GetOrdinal("WORKFLOW_INSTANCE_ID")),r.GetInt32(r.GetOrdinal("STEP_ORDER")),
            r.GetString(r.GetOrdinal("STEP_CODE")),r.GetString(r.GetOrdinal("STEP_NAME")),r.GetString(r.GetOrdinal("APPROVER_SOURCE")),r.GetString(r.GetOrdinal("APPROVAL_MODE")),
            r.GetInt32(r.GetOrdinal("REQUIRED_APPROVAL_COUNT")),r.GetBoolean(r.GetOrdinal("IS_OPTIONAL")),r.GetNullableString("COMPLETION_DESTINATION"),r.GetString(r.GetOrdinal("STEP_STATUS")),
            r.GetNullable<DateTime>("ACTIVATED_ON"),r.GetNullable<DateTime>("COMPLETED_ON"),r.GetNullableString("EMPLOYEE_ID"),r.GetNullableString("DISPLAY_NAME"),
            r.GetNullable<int>("AUTHORITY_LEVEL"),r.GetNullableString("DECISION_STATUS"),r.GetNullable<DateTime>("DECISION_ON"),r.GetNullableString("REMARKS")),
            [I("@RequisitionId",SqlDbType.BigInt,requisitionId)],ct);

    public Task<StoredProcedureResult?> ActAsync(long stepId,string employeeId,GenericApprovalActionRequest x,CancellationToken ct) =>
        StoredProcedures.QuerySingleOrDefaultAsync("dbo.usp_WorkflowApprovalChain_Action",StoredProcedureResult.FromReader,
            [I("@WorkflowInstanceStepId",SqlDbType.BigInt,stepId),I("@EmployeeId",SqlDbType.VarChar,employeeId,30),I("@ActionType",SqlDbType.VarChar,x.ActionType.Trim().ToUpperInvariant(),15),
             I("@Remarks",SqlDbType.NVarChar,x.Remarks?.Trim(),1000),I("@BatchReference",SqlDbType.UniqueIdentifier,x.BatchReference)],ct);

    private static SqlParameter I(string name,SqlDbType type,object? value,int? size=null) => SqlParameterFactory.Input(name,type,value,size);
    private static DateOnly? ToDate(SqlDataReader reader,string name) => reader.GetNullable<DateTime>(name) is { } value ? DateOnly.FromDateTime(value) : null;
}
