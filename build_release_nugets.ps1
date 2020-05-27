#!/usr/bin/env pwsh
Get-ChildItem src/*.csproj -Recurse | ForEach-Object { dotnet build -c Release $_.FullName /p:GeneratePackageOnBuild=true }
