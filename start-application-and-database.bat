@echo off
setlocal

set "PROJECT_DIR=%~dp0"
set "SOLUTION=%PROJECT_DIR%JamunaBank.Procurement.slnx"
set "API_PROJECT=%PROJECT_DIR%src\JamunaBank.Procurement.API\JamunaBank.Procurement.API.csproj"
set "SQL_SERVICE=MSSQLSERVER"
set "API_URL=http://localhost:5101"

echo [1/5] Checking prerequisites...
where dotnet >nul 2>&1 || (
  echo ERROR: The .NET SDK is not installed or is not on PATH.
  exit /b 1
)

sc query "%SQL_SERVICE%" >nul 2>&1 || (
  echo ERROR: SQL Server service "%SQL_SERVICE%" was not found.
  echo Update SQL_SERVICE in this file if you use a named SQL Server instance.
  exit /b 1
)

echo [2/5] Starting SQL Server service...
sc query "%SQL_SERVICE%" | find /I "RUNNING" >nul
if errorlevel 1 (
  net start "%SQL_SERVICE%" >nul 2>&1
  if errorlevel 1 (
    echo ERROR: SQL Server could not be started.
    echo Right-click this file and choose "Run as administrator".
    exit /b 1
  )
) else (
  echo SQL Server is already running.
)

echo [3/5] Building the solution...
dotnet build "%SOLUTION%" --no-restore --configuration Release --disable-build-servers -m:1
if errorlevel 1 (
  echo ERROR: The solution build failed.
  exit /b 1
)

echo [4/5] Starting the Procurement API...
start "JamunaBank Procurement API" cmd /k "cd /d "%PROJECT_DIR%" && set ASPNETCORE_ENVIRONMENT=Development && dotnet run --project "%API_PROJECT%" --no-build --configuration Release --launch-profile http"

echo [5/5] Waiting for the API and database health check...
powershell.exe -NoProfile -ExecutionPolicy Bypass -Command ^
  "$deadline=(Get-Date).AddSeconds(30); do { try { $r=Invoke-RestMethod '%API_URL%/api/system/database-health' -TimeoutSec 2; if($r.success) { Write-Host ('READY: API and database are healthy. Active organization units: ' + $r.data.activeOrganizationUnitCount); exit 0 } } catch {}; Start-Sleep -Milliseconds 750 } while((Get-Date) -lt $deadline); Write-Error 'The API/database health endpoint did not become ready within 30 seconds. Check the API window for details.'; exit 1"

if errorlevel 1 exit /b 1

echo.
echo Application: %API_URL%
echo Swagger UI: %API_URL%/swagger
echo Close the "JamunaBank Procurement API" window or press Ctrl+C there to stop the API.
echo SQL Server remains running as a Windows service.
endlocal
