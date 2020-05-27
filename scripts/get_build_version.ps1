#!/usr/bin/env pwsh

$version_text = `
(Select-Xml -Path "./src/Directory.Build.props" -XPath "/Project/PropertyGroup/Version[contains(@Condition, `"`'`$(Version)`'==`'`'`")]//text()[1]").Node.Value;

Add-Type -Path "./scripts/NuGet.Versioning.dll"
New-Object -TypeName "NuGet.Versioning.NuGetVersion" -ArgumentList "$version_text"
