#!/usr/bin/env bash
set -euox pipefail

cd "$(dirname "${BASH_SOURCE[0]}")"

export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1

# Ensure Cake is up to date
dotnet tool update Cake.Tool --global || true
dotnet tool update Cake.Tool || true
dotnet add package Cake.Core --version latest || true
dotnet restore || true

# Restore tools (in case Cake needs dependencies)
dotnet tool restore

# Run Cake
dotnet cake "$@"
