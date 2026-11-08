using JamunaBank.Procurement.API.Tests.Services;

var tests = new (string Name, Action Run)[]
{
    (nameof(CurrentUserServiceTests.ValidClaimsExposeAuthenticatedUser), CurrentUserServiceTests.ValidClaimsExposeAuthenticatedUser),
    (nameof(CurrentUserServiceTests.MissingRequiredClaimsThrow), CurrentUserServiceTests.MissingRequiredClaimsThrow),
    (nameof(CurrentUserServiceTests.RoleChecksAreCaseInsensitiveAndRejectMissingRole), CurrentUserServiceTests.RoleChecksAreCaseInsensitiveAndRejectMissingRole),
    (nameof(CurrentUserServiceTests.UnauthenticatedUserThrows), CurrentUserServiceTests.UnauthenticatedUserThrows),
    (nameof(CurrentUserServiceTests.OrganizationContextComesFromValidatedClaims), CurrentUserServiceTests.OrganizationContextComesFromValidatedClaims),
    (nameof(CurrentUserServiceTests.InvalidOrganizationIdsThrow), CurrentUserServiceTests.InvalidOrganizationIdsThrow)
    , (nameof(OrgUnitServiceTests.PagingAndTypeAreApplied), OrgUnitServiceTests.PagingAndTypeAreApplied)
    , (nameof(OrgUnitServiceTests.IdLookupUsesOrganizationProcedureResults), OrgUnitServiceTests.IdLookupUsesOrganizationProcedureResults)
    , (nameof(OrgUnitServiceTests.InvalidFiltersAndPagingAreRejected), OrgUnitServiceTests.InvalidFiltersAndPagingAreRejected)
    , (nameof(CommonLookupServiceTests.CategoryIsNormalized), CommonLookupServiceTests.CategoryIsNormalized)
    , (nameof(CommonLookupServiceTests.InvalidCategoryIsRejected), CommonLookupServiceTests.InvalidCategoryIsRejected)
    , (nameof(AssetServiceTests.SearchFiltersAreApplied), AssetServiceTests.SearchFiltersAreApplied)
    , (nameof(AssetServiceTests.ActorComesFromCurrentUser), AssetServiceTests.ActorComesFromCurrentUser)
    , (nameof(AssetServiceTests.RequiredFieldsAreValidated), AssetServiceTests.RequiredFieldsAreValidated)
    , (nameof(AssetServiceTests.ActivationPreservesStoredValues), AssetServiceTests.ActivationPreservesStoredValues)
    , (nameof(AssetSpecificationServiceTests.SupportedTypesAndOrderingValidate), AssetSpecificationServiceTests.SupportedTypesAndOrderingValidate)
    , (nameof(AssetSpecificationServiceTests.WritesUseCurrentEmployee), AssetSpecificationServiceTests.WritesUseCurrentEmployee)
    , (nameof(AssetSpecificationServiceTests.EmptyOptionsAndValuesValidate), AssetSpecificationServiceTests.EmptyOptionsAndValuesValidate)
    , (nameof(ConcernDivisionSetupServiceTests.OfficialsAndAuthoritiesRemainSeparate), ConcernDivisionSetupServiceTests.OfficialsAndAuthoritiesRemainSeparate)
    , (nameof(ConcernDivisionSetupServiceTests.DevelopmentEmployeesAndDatesValidate), ConcernDivisionSetupServiceTests.DevelopmentEmployeesAndDatesValidate)
    , (nameof(ConcernDivisionSetupServiceTests.MappingWritesUseCurrentEmployee), ConcernDivisionSetupServiceTests.MappingWritesUseCurrentEmployee)
    , (nameof(ProcurementOfficerAssetMappingServiceTests.ReturnsMultipleOfficerMappings), ProcurementOfficerAssetMappingServiceTests.ReturnsMultipleOfficerMappings)
    , (nameof(ProcurementOfficerAssetMappingServiceTests.AddValidatesEmployeeAndDates), ProcurementOfficerAssetMappingServiceTests.AddValidatesEmployeeAndDates)
    , (nameof(ProcurementOfficerAssetMappingServiceTests.WritesUseActorAndAssetScope), ProcurementOfficerAssetMappingServiceTests.WritesUseActorAndAssetScope)
    , (nameof(WorkflowRuleServiceTests.RuleValidationRejectsInvalidConfiguration), WorkflowRuleServiceTests.RuleValidationRejectsInvalidConfiguration)
    , (nameof(WorkflowRuleServiceTests.WritesUseCurrentUserAndActivationPreservesRule), WorkflowRuleServiceTests.WritesUseCurrentUserAndActivationPreservesRule)
    , (nameof(WorkflowRuleServiceTests.EvaluationReturnsDatabaseCalculatedRoute), WorkflowRuleServiceTests.EvaluationReturnsDatabaseCalculatedRoute)
    , (nameof(RequisitionMakerServiceTests.SupportsMultipleItemsAndTrustedRequesterContext), RequisitionMakerServiceTests.SupportsMultipleItemsAndTrustedRequesterContext)
    , (nameof(RequisitionMakerServiceTests.ReplacementAndDynamicOptionValidation), RequisitionMakerServiceTests.ReplacementAndDynamicOptionValidation)
    , (nameof(RequisitionMakerServiceTests.ReadsAndSubmitUseAuthenticatedEmployee), RequisitionMakerServiceTests.ReadsAndSubmitUseAuthenticatedEmployee)
    , (nameof(ManagerWorkflowServiceTests.PendingAndDetailUseCurrentManager), ManagerWorkflowServiceTests.PendingAndDetailUseCurrentManager)
    , (nameof(ManagerWorkflowServiceTests.ReturnAndRejectRequireRemarks), ManagerWorkflowServiceTests.ReturnAndRejectRequireRemarks)
    , (nameof(ManagerWorkflowServiceTests.TransitionsUseTrustedActorAndDistinctActions), ManagerWorkflowServiceTests.TransitionsUseTrustedActorAndDistinctActions)
    , (nameof(ProcurementOfficerWorkflowServiceTests.QueueAndActionsUseAuthenticatedOfficer), ProcurementOfficerWorkflowServiceTests.QueueAndActionsUseAuthenticatedOfficer)
    , (nameof(ProcurementOfficerWorkflowServiceTests.ReturnRejectAndCommentsRequireContent), ProcurementOfficerWorkflowServiceTests.ReturnRejectAndCommentsRequireContent)
    , (nameof(ProcurementOfficerWorkflowServiceTests.CommentAndTransitionsUseDistinctRepositoryOperations), ProcurementOfficerWorkflowServiceTests.CommentAndTransitionsUseDistinctRepositoryOperations)
    , (nameof(ProcurementOfficerWorkflowServiceTests.ApprovalReturnsDatabaseCalculatedDestination), ProcurementOfficerWorkflowServiceTests.ApprovalReturnsDatabaseCalculatedDestination)
    , (nameof(ProcurementAuthorityServiceTests.QueueAndActionsUseAuthenticatedAuthority), ProcurementAuthorityServiceTests.QueueAndActionsUseAuthenticatedAuthority)
    , (nameof(ProcurementAuthorityServiceTests.RejectRequiresRemarks), ProcurementAuthorityServiceTests.RejectRequiresRemarks)
    , (nameof(ProcurementAuthorityServiceTests.BulkUsesOneBatchAndProcessesFailuresIndependently), ProcurementAuthorityServiceTests.BulkUsesOneBatchAndProcessesFailuresIndependently)
    , (nameof(ProcurementAuthorityServiceTests.BulkDeduplicatesIdsAndReturnsDatabaseDestination), ProcurementAuthorityServiceTests.BulkDeduplicatesIdsAndReturnsDatabaseDestination)
    , (nameof(ConcernDivisionWorkflowServiceTests.QueueAndActionsUseAuthenticatedOfficial), ConcernDivisionWorkflowServiceTests.QueueAndActionsUseAuthenticatedOfficial)
    , (nameof(ConcernDivisionWorkflowServiceTests.ReturnAndNotRecommendRequireRemarks), ConcernDivisionWorkflowServiceTests.ReturnAndNotRecommendRequireRemarks)
    , (nameof(ConcernDivisionWorkflowServiceTests.OfficialChoicesRemainDistinct), ConcernDivisionWorkflowServiceTests.OfficialChoicesRemainDistinct)
    , (nameof(ConcernDivisionWorkflowServiceTests.VerifyCompleteReturnsMatrixDestination), ConcernDivisionWorkflowServiceTests.VerifyCompleteReturnsMatrixDestination)
    , (nameof(ConcernAuthorityServiceTests.QueueAndActionsUseAuthenticatedAuthority), ConcernAuthorityServiceTests.QueueAndActionsUseAuthenticatedAuthority)
    , (nameof(ConcernAuthorityServiceTests.ReturnAndRejectRequireRemarks), ConcernAuthorityServiceTests.ReturnAndRejectRequireRemarks)
    , (nameof(ConcernAuthorityServiceTests.BulkUsesSharedBatchAndKeepsPartialResults), ConcernAuthorityServiceTests.BulkUsesSharedBatchAndKeepsPartialResults)
    , (nameof(ConcernAuthorityServiceTests.ApprovalDestinationComesFromProcedureResult), ConcernAuthorityServiceTests.ApprovalDestinationComesFromProcedureResult)
    , (nameof(ProcurementTrackingServiceTests.SearchUsesOnlyRequestedSimpleFilters), ProcurementTrackingServiceTests.SearchUsesOnlyRequestedSimpleFilters)
    , (nameof(ProcurementTrackingServiceTests.SingleUpdateUsesAuthenticatedTrackerAndOneId), ProcurementTrackingServiceTests.SingleUpdateUsesAuthenticatedTrackerAndOneId)
    , (nameof(ProcurementTrackingServiceTests.BulkUsesSharedBatchAndWritesEveryRequisition), ProcurementTrackingServiceTests.BulkUsesSharedBatchAndWritesEveryRequisition)
    , (nameof(ProcurementTrackingServiceTests.InvalidStatusDateAndIdsAreRejected), ProcurementTrackingServiceTests.InvalidStatusDateAndIdsAreRejected)
    , (nameof(WorkOrderServiceTests.MultipleRequisitionsAndActorReachRepository), WorkOrderServiceTests.MultipleRequisitionsAndActorReachRepository)
    , (nameof(WorkOrderServiceTests.HeaderAndDateValidationIsApplied), WorkOrderServiceTests.HeaderAndDateValidationIsApplied)
    , (nameof(WorkOrderServiceTests.ReadsValidateIds), WorkOrderServiceTests.ReadsValidateIds)
    , (nameof(SupportingServiceTests.AttachmentAndCommentActorsComeFromCurrentUser), SupportingServiceTests.AttachmentAndCommentActorsComeFromCurrentUser)
    , (nameof(SupportingServiceTests.AttachmentStoresMetadataReferenceOnly), SupportingServiceTests.AttachmentStoresMetadataReferenceOnly)
    , (nameof(SupportingServiceTests.InvalidMetadataAndCommentsAreRejected), SupportingServiceTests.InvalidMetadataAndCommentsAreRejected)
    , (nameof(SupportingServiceTests.HistoryAndAuditFiltersReachRepository), SupportingServiceTests.HistoryAndAuditFiltersReachRepository)
};

var failures = 0;
foreach (var test in tests)
{
    try
    {
        test.Run();
        Console.WriteLine($"PASS {test.Name}");
    }
    catch (Exception exception)
    {
        failures++;
        Console.Error.WriteLine($"FAIL {test.Name}: {exception.Message}");
    }
}

Console.WriteLine($"Total: {tests.Length}, Passed: {tests.Length - failures}, Failed: {failures}");
return failures == 0 ? 0 : 1;
