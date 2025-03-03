$ErrorActionPreference = 'Stop'

Set-Location -LiteralPath $PSScriptRoot

$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
$env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
$env:DOTNET_NOLOGO = '1'

# Ensure Cake is up to date (Ignore errors properly)
try {
    dotnet tool update Cake.Tool --global -ErrorAction Continue
    dotnet tool update Cake.Tool -ErrorAction Continue
    dotnet add package Cake.Core --version latest -ErrorAction Continue
    dotnet restore -ErrorAction Continue
} catch {
    Write-Host "Ignoring errors from dotnet commands"
}

if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

dotnet cake @args
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
