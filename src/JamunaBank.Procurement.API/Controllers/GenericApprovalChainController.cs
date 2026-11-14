using JamunaBank.Procurement.API.Constants;
using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Models;
using JamunaBank.Procurement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JamunaBank.Procurement.API.Controllers;

[ApiController,Authorize,Route("api")]
public sealed class GenericApprovalChainController(IGenericApprovalChainService service) : ControllerBase
{
    [HttpGet("approval-units"),Authorize(Policy=AuthorizationPolicyNames.Admin)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ApprovalUnitDto>>>> GetUnits([FromQuery]bool activeOnly=true,CancellationToken ct=default) =>
        Ok(ApiResponse<IReadOnlyList<ApprovalUnitDto>>.Ok(await service.GetUnitsAsync(activeOnly,ct),"Approval units loaded.",HttpContext.TraceIdentifier));

    [HttpPost("approval-units"),Authorize(Policy=AuthorizationPolicyNames.Admin)]
    public async Task<ActionResult<ApiResponse<StoredProcedureResult>>> CreateUnit(SaveApprovalUnitRequest request,CancellationToken ct) => Map(await service.SaveUnitAsync(null,request,ct),"Approval unit saved.");

    [HttpPut("approval-units/{id:long}"),Authorize(Policy=AuthorizationPolicyNames.Admin)]
    public async Task<ActionResult<ApiResponse<StoredProcedureResult>>> UpdateUnit(long id,SaveApprovalUnitRequest request,CancellationToken ct) => Map(await service.SaveUnitAsync(id,request,ct),"Approval unit saved.");

    [HttpGet("approval-units/{id:long}/approvers"),Authorize(Policy=AuthorizationPolicyNames.Admin)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ApprovalUnitApproverDto>>>> GetApprovers(long id,[FromQuery]bool activeOnly=true,CancellationToken ct=default) =>
        Ok(ApiResponse<IReadOnlyList<ApprovalUnitApproverDto>>.Ok(await service.GetApproversAsync(id,activeOnly,ct),"Approval-unit approvers loaded.",HttpContext.TraceIdentifier));

    [HttpPost("approval-unit-approvers"),Authorize(Policy=AuthorizationPolicyNames.Admin)]
    public async Task<ActionResult<ApiResponse<StoredProcedureResult>>> SaveApprover(SaveApprovalUnitApproverRequest request,CancellationToken ct) => Map(await service.SaveApproverAsync(request,ct),"Approval-unit approver saved.");

    [HttpGet("workflow-rules/{ruleId:long}/approval-steps"),Authorize(Policy=AuthorizationPolicyNames.Admin)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<WorkflowRuleStepDto>>>> GetRuleSteps(long ruleId,CancellationToken ct) =>
        Ok(ApiResponse<IReadOnlyList<WorkflowRuleStepDto>>.Ok(await service.GetRuleStepsAsync(ruleId,ct),"Workflow approval steps loaded.",HttpContext.TraceIdentifier));

    [HttpPost("workflow-rule-steps"),Authorize(Policy=AuthorizationPolicyNames.Admin)]
    public async Task<ActionResult<ApiResponse<StoredProcedureResult>>> CreateRuleStep(SaveWorkflowRuleStepRequest request,CancellationToken ct) => Map(await service.SaveRuleStepAsync(null,request,ct),"Workflow approval step saved.");

    [HttpPut("workflow-rule-steps/{id:long}"),Authorize(Policy=AuthorizationPolicyNames.Admin)]
    public async Task<ActionResult<ApiResponse<StoredProcedureResult>>> UpdateRuleStep(long id,SaveWorkflowRuleStepRequest request,CancellationToken ct) => Map(await service.SaveRuleStepAsync(id,request,ct),"Workflow approval step saved.");

    [HttpPost("workflow-instances/{id:long}/approval-chain/initialize"),Authorize(Policy=AuthorizationPolicyNames.Admin)]
    public async Task<ActionResult<ApiResponse<StoredProcedureResult>>> Initialize(long id,CancellationToken ct) => Map(await service.InitializeAsync(id,ct),"Generic approval chain initialized.");

    [HttpGet("generic-approvals/my-pending")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PendingGenericApprovalDto>>>> GetMyPending(CancellationToken ct) =>
        Ok(ApiResponse<IReadOnlyList<PendingGenericApprovalDto>>.Ok(await service.GetMyPendingAsync(ct),"Pending generic approvals loaded.",HttpContext.TraceIdentifier));

    [HttpGet("generic-approval-steps/{id:long}/review")]
    public async Task<ActionResult<ApiResponse<RequisitionDetailDto>>> GetReview(long id,CancellationToken ct)
    {
        var detail=await service.GetReviewDetailAsync(id,ct);
        return detail is null
            ? NotFound(ApiResponse<RequisitionDetailDto>.Fail("This approval task is no longer pending with you.",[],HttpContext.TraceIdentifier))
            : Ok(ApiResponse<RequisitionDetailDto>.Ok(detail,"Approval review detail loaded.",HttpContext.TraceIdentifier));
    }

    [HttpGet("requisitions/{id:long}/approval-chain")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ApprovalChainEntryDto>>>> GetChain(long id,CancellationToken ct) =>
        Ok(ApiResponse<IReadOnlyList<ApprovalChainEntryDto>>.Ok(await service.GetByRequisitionAsync(id,ct),"Approval chain loaded.",HttpContext.TraceIdentifier));

    [HttpPost("generic-approval-steps/{id:long}/actions")]
    public async Task<ActionResult<ApiResponse<StoredProcedureResult>>> Act(long id,GenericApprovalActionRequest request,CancellationToken ct) => Map(await service.ActAsync(id,request,ct),"Approval action completed.");

    private ActionResult<ApiResponse<StoredProcedureResult>> Map(StoredProcedureResult? result,string message)
    {
        if(result is null) return StatusCode(500,ApiResponse<StoredProcedureResult>.Fail("No stored-procedure result was returned.",["Operation failed."],HttpContext.TraceIdentifier));
        return result.ResultCode switch
        {
            ResultCodes.Success => Ok(ApiResponse<StoredProcedureResult>.Ok(result,message,HttpContext.TraceIdentifier)),
            ResultCodes.NotFound => NotFound(ApiResponse<StoredProcedureResult>.Fail("The requested workflow record was not found.",[result.ResultMessage],HttpContext.TraceIdentifier)),
            ResultCodes.Conflict or ResultCodes.InvalidWorkflowState => Conflict(ApiResponse<StoredProcedureResult>.Fail("The workflow operation is not valid in its current state.",[result.ResultMessage],HttpContext.TraceIdentifier)),
            ResultCodes.ValidationError or ResultCodes.InvalidActor => BadRequest(ApiResponse<StoredProcedureResult>.Fail("The approval-chain request is invalid.",[result.ResultMessage],HttpContext.TraceIdentifier)),
            _ => StatusCode(500,ApiResponse<StoredProcedureResult>.Fail("Approval-chain operation failed.",[result.ResultMessage],HttpContext.TraceIdentifier))
        };
    }
}
