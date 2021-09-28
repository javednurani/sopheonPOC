Import-Module "$($env:System_DefaultWorkingDirectory)\DevOps\PowerShell\CloudNative.Common.psm1";
$EnvironmentsUtilityProject = "$($env:System_DefaultWorkingDirectory)\source\Sopheon.CloudNative.Environments\Sopheon.CloudNative.Environments.Utility\Sopheon.CloudNative.Environments.Utility.csproj";
$EnvironmentsUtilityDataSeeder = "$($env:System_DefaultWorkingDirectory)\source\Sopheon.CloudNative.Environments\EnvironmentsUtility\EnvironmentsUtility.exe";
Copy-Item -Path "$($env:System_DefaultWorkingDirectory)\source\Sopheon.CloudNative.Environments\deploy\*" -Destination $env:Build_ArtifactStagingDirectory;

Set-Location -Path "$($env:System_DefaultWorkingDirectory)\source\Sopheon.CloudNative.Environments";

New-Item -Path .\PublishOutput -ItemType directory;

$OutputCoveragePath = "$($env:System_DefaultWorkingDirectory)\source\Sopheon.CloudNative.Environments\TestResults\";

Write-Host "...Running dotnet ef migrations..."

dotnet ef migrations script -p "Sopheon.CloudNative.Environments.Data\Sopheon.CloudNative.Environments.Data.csproj" -o "$($env:Build_ArtifactStagingDirectory)\scripts.sql" -i;
Check-LastExitCode;

Write-Host "...Running dotnet publish on Functions.csproj";

dotnet publish "Sopheon.CloudNative.Environments.Functions\Sopheon.CloudNative.Environments.Functions.csproj" -o ".\PublishOutput\";
Check-LastExitCode;

#Setup for Integration tests here --
$IntegrationTestProjects = Get-Item -Path "$($env:System_DefaultWorkingDirectory)\source\Sopheon.CloudNative.Environments\**\*.IntegrationTests.csproj";

dotnet publish $EnvironmentsUtilityProject -r win-x64 -p:PublishSingleFile=true /p:PublishTrimmed=true /p:Version=1.0.1 /p:IncludeNativeLibrariesForSelfExtract=true /p:DebugType=none --self-contained true -o ./EnviromentsUtility;

& $EnvironmentsUtilityDataSeeder 

Foreach($file in $IntegrationTestProjects) {
    Write-Host "...Running integration tests on $($file.Name)..."
    dotnet test $file.FullName -p:CollectCoverage=true -p:CoverletOutput=$OutputCoveragePath -p:CoverletOutputFormat="json%2cCobertura" -p:MergeWith="$($OutputCoveragePath)\coverage.json" --logger:"xunit;LogFilePath=$($OutputCoveragePath)\$($file.Name.Replace('.csproj', '')).xml" -p:Exclude="[*]Sopheon.CloudNative.Environments.Data.Migrations.*"
}

#Setup for Unit Tests here --
$TestProjects = Get-Item -Path "$($env:System_DefaultWorkingDirectory)\source\Sopheon.CloudNative.Environments\**\*.UnitTests.csproj";

Write-Host "...Number of UnitTest projects found: $($TestProjects.Length)...";

Foreach($file in $TestProjects) {
    Write-Host "...Running unit tests on $($file.Name)..."
    dotnet test $file.FullName -p:CollectCoverage=true -p:CoverletOutput=$OutputCoveragePath -p:CoverletOutputFormat="json%2cCobertura" -p:MergeWith="$($OutputCoveragePath)\coverage.json" --logger:"xunit;LogFilePath=$($OutputCoveragePath)\$($file.Name.Replace('.csproj', '')).xml" -p:Exclude="[*]Sopheon.CloudNative.Environments.Data.Migrations.*"
}

# Zip/Archive Scripts 
Write-Host "Zipping Artfacts for Environment Management...";
& $ZipUtil "a" "-tzip" "$($env:Build_ArtifactStagingDirectory)\EnvironmentManagement" ".\PublishOutput\*" "-xr!build" "-xr!deploy";

Write-Host "Zipping Complete!";