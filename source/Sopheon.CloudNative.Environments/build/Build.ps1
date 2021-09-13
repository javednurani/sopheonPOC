Import-Module "$($env:System_DefaultWorkingDirectory)\DevOps\PowerShell\CloudNative.Common.psm1";

Copy-Item -Path "$($env:System_DefaultWorkingDirectory)\source\Sopheon.CloudNative.Environments\deploy\*" -Destination $env:Build_ArtifactStagingDirectory;

Set-Location -Path "$($env:System_DefaultWorkingDirectory)\source\Sopheon.CloudNative.Environments";

New-Item -Path .\PublishOutput -ItemType directory;

$OutputCoveragePath = "$($env:System_DefaultWorkingDirectory)\source\Sopheon.CloudNative.Environments\TestResults\";


$TestProjects = Get-Item -Path "$($env:System_DefaultWorkingDirectory)\source\Sopheon.CloudNative.Environments\**\*.UnitTests.csproj";

Write-Host "...Number of UnitTest projects found: $($TestProjects.Length)...";

Foreach($file in $TestProjects) {
    Write-Host "...Running tests on $($file.Name)..."
    dotnet test $file.FullName -p:CollectCoverage=true -p:CoverletOutput=$OutputCoveragePath -p:CoverletOutputFormat="json%2cCobertura" -p:MergeWith="$($OutputCoveragePath)\coverage.json" --logger:"xunit;LogFilePath=$($OutputCoveragePath)\$($file.Name.Replace('.csproj', '')).xml"
}

Write-Host "...Running dotnet ef migrations..."

dotnet ef migrations script -p "Sopheon.CloudNative.Environments.Data\Sopheon.CloudNative.Environments.Data.csproj" -o "$($env:Build_ArtifactStagingDirectory)\scripts.sql" -i;
Check-LastExitCode;

Write-Host "...Running dotnet publish on Functions.csproj";

dotnet publish "Sopheon.CloudNative.Environments.Functions\Sopheon.CloudNative.Environments.Functions.csproj" -o ".\PublishOutput\";
Check-LastExitCode;

# Zip/Archive Scripts 
Write-Host "Zipping Artfacts for Environment Management...";
& $ZipUtil "a" "-tzip" "$($env:Build_ArtifactStagingDirectory)\EnvironmentManagement" ".\PublishOutput\*" "-xr!build" "-xr!deploy";

Write-Host "Zipping Complete!";