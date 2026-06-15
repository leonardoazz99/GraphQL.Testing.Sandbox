#!/usr/bin/env pwsh
# Exports both subgraph schemas and composes them into the gateway's Fusion archive.
# Run from the repository root:  ./compose.ps1
$ErrorActionPreference = "Stop"
Set-Location $PSScriptRoot

Write-Host "==> Exporting ProductionControl source schema"
dotnet run --project ProductionControl.GraphQL.Service -- schema export

Write-Host "==> Exporting MasterData source schema"
dotnet run --project MasterData.GraphQL.Service -- schema export

Write-Host "==> Restoring subgraph transport URLs"
@'
{
  "name": "ProductionControl",
  "transports": { "http": { "url": "http://localhost:5001/graphql" } }
}
'@ | Set-Content -NoNewline ProductionControl.GraphQL.Service/schema-settings.json

@'
{
  "name": "MasterData",
  "transports": { "http": { "url": "http://localhost:5002/graphql" } }
}
'@ | Set-Content -NoNewline MasterData.GraphQL.Service/schema-settings.json

Write-Host "==> Composing Fusion archive -> ApolloFederation.Service/gateway.far"
nitro fusion compose `
  -f ProductionControl.GraphQL.Service/schema.graphqls `
  -f MasterData.GraphQL.Service/schema.graphqls `
  -a ApolloFederation.Service/gateway.far

Write-Host "==> Done. Now run the three services (see README)."
