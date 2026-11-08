# Jamuna Bank Central Procurement API — Steps 1–6

This solution contains the ASP.NET Core Web API foundation and the Step 2 common stored-procedure execution infrastructure. Business modules and authentication implementations remain deferred.

## Run

```powershell
dotnet build .\JamunaBank.Procurement.slnx
dotnet run --project .\src\JamunaBank.Procurement.API
```

Endpoints: `/api/system/health`, `/api/system/database-health`, `/openapi/v1.json`, and `/swagger`.

Step 5 adds authenticated reference-data reads:

- `GET /api/org-units?type=Branch&page=1&pageSize=50`
- `GET /api/org-units/{id}`
- `GET /api/common-lookups/{category}`

The database project currently provides `dbo.usp_Organization_GetAll` and
`dbo.usp_Common_GetLookup`. It does not provide an organization-by-ID procedure,
any `ROLE_MASTER` read procedure, or organization/role/common-lookup modification
procedures. The ID endpoint therefore filters the active results returned by
`dbo.usp_Organization_GetAll`. Role and configuration mutation APIs are deliberately
not exposed; no direct SQL fallback is used.

Step 6 adds authenticated Asset Master reads and ADMIN-only Asset Master writes:

- `GET /api/asset-categories?isActive=true`
- `GET /api/asset-categories/{id}`
- `GET /api/assets?assetCategoryId=1&assetName=laptop&assetCode=LT&isActive=true`
- `GET /api/assets/{id}`
- `POST /api/assets` (ADMIN)
- `PUT /api/assets/{id}` (ADMIN)
- `PUT /api/assets/{id}/activation` (ADMIN)

The database project has only `dbo.usp_AssetCategory_GetAll` for asset categories.
It has no category create, update, or activation procedure, so those write endpoints
are deliberately not exposed. Category-by-ID uses the stored-procedure list result.
`dbo.usp_Asset_GetAll` has category and active-only parameters but no name/code
parameters; name and code filters are applied to its DTO results. `dbo.usp_Asset_Update`
supports name, description, unit of measurement, active state, and actor only, so the
API does not imply that code, category, or advanced attributes can be updated.
No direct SQL fallback is used.

The data flow is strictly Controller → Service → Repository → stored procedure → SQL Server. `StoredProcedureExecutor` forces `CommandType.StoredProcedure`; the API must never issue inline SQL or direct table DML. The production connection string should be supplied through environment variables, user secrets, or a secret store. `appsettings.Development.json` uses Windows Authentication and contains no password. Its local-only `Encrypt=False` setting works around the current machine's TLS/SSPI issue; production must use encryption with a trusted certificate.

`/api/system/database-health` exercises `dbo.usp_Organization_GetAll`, an existing harmless read-only stored procedure. It does not query any table directly.

Swagger UI assets load from `unpkg.com` because the host's NuGet TLS credential provider could not restore Swashbuckle. The OpenAPI document itself is served locally without an external package.
