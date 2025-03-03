$ErrorActionPreference = 'Stop'

Set-Location -LiteralPath $PSScriptRoot

$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
$env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
$env:DOTNET_NOLOGO = '1'

# Ensure Cake is up to date
dotnet tool update Cake.Tool --global
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

dotnet tool update Cake.Tool
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

dotnet restore
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

# Find a .csproj file before adding package
$projectFile = Get-ChildItem -Path . -Recurse -Filter "*.csproj" | Select-Object -First 1
if ($projectFile) {
    dotnet add $projectFile.FullName package Cake.Core --version latest
} else {
    Write-Host "❌ ERROR: No .csproj file found!"
    exit 1
}

dotnet tool restore
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

dotnet cake @args
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
