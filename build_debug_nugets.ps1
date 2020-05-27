#!/usr/bin/env pwsh
Get-ChildItem src/*.csproj -Recurse | ForEach-Object { dotnet build -c Debug $_.FullName /p:GeneratePackageOnBuild=true }
