using JamunaBank.Procurement.API.Configuration;
using JamunaBank.Procurement.API.Models;
using Microsoft.AspNetCore.Mvc;
using JamunaBank.Procurement.API.Data;
using JamunaBank.Procurement.API.Repositories.Implementations;
using JamunaBank.Procurement.API.Repositories.Interfaces;
using JamunaBank.Procurement.API.Services.Implementations;
using JamunaBank.Procurement.API.Services.Interfaces;
using JamunaBank.Procurement.API.Authentication;
using JamunaBank.Procurement.API.Authentication.Development;
using JamunaBank.Procurement.API.Authentication.Interfaces;
using JamunaBank.Procurement.API.Authentication.JamunaBank;
using JamunaBank.Procurement.API.Constants;
using Microsoft.AspNetCore.Authentication;
using AppAuthenticationOptions = JamunaBank.Procurement.API.Authentication.AuthenticationOptions;
using AppAuthenticationService = JamunaBank.Procurement.API.Authentication.AuthenticationService;
using IAppAuthenticationService = JamunaBank.Procurement.API.Authentication.Interfaces.IAuthenticationService;

namespace JamunaBank.Procurement.API.Extensions;

public static class ServiceCollectionExtensions
{
    public const string CorsPolicyName = "ConfiguredOrigins";

    public static IServiceCollection AddApiFoundation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        });
        services.Configure<ApiBehaviorOptions>(options => options.InvalidModelStateResponseFactory = context =>
            new BadRequestObjectResult(ApiResponse<object>.Fail("Validation failed.",
                context.ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage),
                context.HttpContext.TraceIdentifier)));

        var timeout = configuration.GetValue<int?>("Database:CommandTimeoutSeconds") ?? 30;
        if (timeout <= 0) throw new InvalidOperationException("Database command timeout must be greater than zero.");
        services.AddSingleton(new DatabaseOptions
        {
            ConnectionString = configuration.GetConnectionString(DatabaseOptions.ConnectionStringName) ?? string.Empty,
            CommandTimeoutSeconds = timeout
        });
        services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<IStoredProcedureExecutor, StoredProcedureExecutor>();
        services.AddScoped<IDatabaseHealthRepository, DatabaseHealthRepository>();
        services.AddScoped<IDatabaseHealthService, DatabaseHealthService>();
        services.AddScoped<IOrgUnitRepository, OrgUnitRepository>();
        services.AddScoped<IOrgUnitService, OrgUnitService>();
        services.AddScoped<ICommonLookupRepository, CommonLookupRepository>();
        services.AddScoped<ICommonLookupService, CommonLookupService>();
        services.AddScoped<IAssetCategoryRepository, AssetCategoryRepository>();
        services.AddScoped<IAssetCategoryService, AssetCategoryService>();
        services.AddScoped<IAssetRepository, AssetRepository>();
        services.AddScoped<IAssetService, AssetService>();
        services.AddScoped<IAssetSpecificationRepository, AssetSpecificationRepository>();
        services.AddScoped<IAssetSpecificationService, AssetSpecificationService>();
        services.AddSingleton<IDevelopmentEmployeeResolver, DevelopmentEmployeeResolver>();
        services.AddScoped<IConcernDivisionSetupRepository, ConcernDivisionSetupRepository>();
        services.AddScoped<IConcernDivisionSetupService, ConcernDivisionSetupService>();
        services.AddScoped<IProcurementOfficerAssetMappingRepository, ProcurementOfficerAssetMappingRepository>();
        services.AddScoped<IProcurementOfficerAssetMappingService, ProcurementOfficerAssetMappingService>();
        services.AddScoped<IWorkflowRuleRepository, WorkflowRuleRepository>();
        services.AddScoped<IWorkflowRuleService, WorkflowRuleService>();
        services.AddScoped<IRequisitionMakerRepository, RequisitionMakerRepository>();
        services.AddScoped<IRequisitionMakerService, RequisitionMakerService>();
        services.AddScoped<IManagerWorkflowRepository, ManagerWorkflowRepository>();
        services.AddScoped<IManagerWorkflowService, ManagerWorkflowService>();
        services.AddScoped<IProcurementOfficerWorkflowRepository, ProcurementOfficerWorkflowRepository>();
        services.AddScoped<IProcurementOfficerWorkflowService, ProcurementOfficerWorkflowService>();
        services.AddScoped<IProcurementAuthorityRepository, ProcurementAuthorityRepository>();
        services.AddScoped<IProcurementAuthorityService, ProcurementAuthorityService>();
        services.AddScoped<IConcernDivisionWorkflowRepository, ConcernDivisionWorkflowRepository>();
        services.AddScoped<IConcernDivisionWorkflowService, ConcernDivisionWorkflowService>();
        services.AddScoped<IConcernAuthorityRepository, ConcernAuthorityRepository>();
        services.AddScoped<IConcernAuthorityService, ConcernAuthorityService>();
        services.AddScoped<IProcurementTrackingRepository, ProcurementTrackingRepository>();
        services.AddScoped<IProcurementTrackingService, ProcurementTrackingService>();
        services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
        services.AddScoped<IWorkOrderService, WorkOrderService>();
        services.AddScoped<ISupportingRepository, SupportingRepository>();
        services.AddScoped<ISupportingService, SupportingService>();
        services.AddScoped<IAttachmentFileService, AttachmentFileService>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        AddApiAuthentication(services, configuration);
        var cors = configuration.GetSection(CorsOptions.SectionName).Get<CorsOptions>() ?? new CorsOptions();
        services.AddCors(options => options.AddPolicy(CorsPolicyName, policy =>
        {
            if (cors.AllowedOrigins.Length > 0) policy.WithOrigins(cors.AllowedOrigins).AllowAnyHeader().AllowAnyMethod();
        }));
        return services;
    }

    private static void AddApiAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        var authentication = configuration.GetSection(AppAuthenticationOptions.SectionName).Get<AppAuthenticationOptions>()
            ?? throw new InvalidOperationException("Authentication configuration is missing.");
        var jwt = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
            ?? throw new InvalidOperationException("JWT configuration is missing.");
        if (jwt.SigningKey.Length < 32) throw new InvalidOperationException("JWT signing key must contain at least 32 characters.");
        if (jwt.ExpiryMinutes <= 0) throw new InvalidOperationException("JWT expiry must be greater than zero.");

        services.AddSingleton(authentication);
        services.AddSingleton(jwt);
        services.AddSingleton(TimeProvider.System);
        services.AddSingleton<JwtTokenService>();
        services.AddScoped<IAppAuthenticationService, AppAuthenticationService>();

        if (authentication.Mode.Equals(AuthenticationModes.Development, StringComparison.OrdinalIgnoreCase))
            services.AddScoped<IAuthenticationProvider, DevelopmentAuthenticationProvider>();
        else if (authentication.Mode.Equals(AuthenticationModes.JamunaBank, StringComparison.OrdinalIgnoreCase))
            services.AddScoped<IAuthenticationProvider, JamunaBankAuthenticationProvider>();
        else
            throw new InvalidOperationException($"Unsupported authentication mode '{authentication.Mode}'.");

        services.AddAuthentication(ProcurementJwtAuthenticationHandler.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, ProcurementJwtAuthenticationHandler>(
                ProcurementJwtAuthenticationHandler.SchemeName, _ => { });
        services.AddAuthorizationBuilder()
            .AddPolicy(AuthorizationPolicyNames.Maker, policy => policy.RequireRole(RoleCodes.Maker))
            .AddPolicy(AuthorizationPolicyNames.Manager, policy => policy.RequireRole(RoleCodes.Manager))
            .AddPolicy(AuthorizationPolicyNames.ProcurementOfficer, policy => policy.RequireRole(RoleCodes.ProcurementOfficer))
            .AddPolicy(AuthorizationPolicyNames.ProcurementAuthority, policy => policy.RequireRole(RoleCodes.ProcurementAuthority))
            .AddPolicy(AuthorizationPolicyNames.ConcernOfficial, policy => policy.RequireRole(RoleCodes.ConcernOfficial))
            .AddPolicy(AuthorizationPolicyNames.ConcernAuthority, policy => policy.RequireRole(RoleCodes.ConcernAuthority))
            .AddPolicy(AuthorizationPolicyNames.ProcurementTracker, policy => policy.RequireRole(
                RoleCodes.ProcurementTracker,
                RoleCodes.ProcurementOfficer,
                RoleCodes.ProcurementAuthority,
                RoleCodes.Admin))
            .AddPolicy(AuthorizationPolicyNames.Admin, policy => policy.RequireRole(RoleCodes.Admin));
    }
}
