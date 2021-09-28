Import-Module "$($env:System_DefaultWorkingDirectory)\DevOps\PowerShell\CloudNative.Common.psm1";
$EnvironmentsUtilityProject = "$($env:System_DefaultWorkingDirectory)\source\Sopheon.CloudNative.Environments\Sopheon.CloudNative.Environments.Utility\Sopheon.CloudNative.Environments.Utility.csproj";
$EnvironmentsUtilityDataSeeder = "$($env:System_DefaultWorkingDirectory)\source\Sopheon.CloudNative.Environments\EnvironmentsUtility\EnvironmentsUtility.exe";
#TODO: Does this need to be configurable....
$DatabaseName = "EnvironmentManagement";


Copy-Item -Path "$($env:System_DefaultWorkingDirectory)\source\Sopheon.CloudNative.Environments\deploy\*" -Destination $env:Build_ArtifactStagingDirectory;

Set-Location -Path "$($env:System_DefaultWorkingDirectory)\source\Sopheon.CloudNative.Environments";

New-Item -Path .\PublishOutput -ItemType directory;

$OutputCoveragePath = "$($env:System_DefaultWorkingDirectory)\source\Sopheon.CloudNative.Environments\TestResults\";

Write-Host "...Running dotnet ef migrations...";

dotnet ef migrations script -p "Sopheon.CloudNative.Environments.Data\Sopheon.CloudNative.Environments.Data.csproj" -o "$($env:Build_ArtifactStagingDirectory)\scripts.sql" -i;
Check-LastExitCode;

Write-Host "...Running dotnet publish on Functions.csproj";

dotnet publish "Sopheon.CloudNative.Environments.Functions\Sopheon.CloudNative.Environments.Functions.csproj" -o ".\PublishOutput\";
Check-LastExitCode;

#Setup for Integration tests here --
$IntegrationTestProjects = Get-Item -Path "$($env:System_DefaultWorkingDirectory)\source\Sopheon.CloudNative.Environments\**\*.IntegrationTests.csproj";

dotnet publish $EnvironmentsUtilityProject -r win-x64 -p:PublishSingleFile=true /p:PublishTrimmed=true /p:Version=1.0.1 /p:IncludeNativeLibrariesForSelfExtract=true /p:DebugType=none --self-contained true -o ./EnvironmentsUtility;

#Start up the func.exe using func start. This will spin up the functions to run at a local instance (Part of Azure Function Core Tools)
#This has to be ran separately as it is a long running process and would thread block us here...
$Process = Start-Process powershell -WorkingDirectory $env:System_DefaultWorkingDirectory {Set-Location ".\source\Sopheon.CloudNative.Environments\Sopheon.CloudNative.Environments.Functions"; func start;} -PassThru;

#Wait 10 seconds to let the Func app start up
Start-Sleep -Seconds 10;

#This is the func.exe process. We need to capture this object and we close this out. (Tip: This is the long running process mentioned above)
#$SubProcess = Get-Process -Name func;

#Create database - 
Write-Host "...Creating local database: $DatabaseName for integration tests...";
Invoke-Sqlcmd -ServerInstance . -UserName sa -Password $env:LocalDatabaseEnigma -Query "CREATE DATABASE $DatabaseName";

#Migrate database - 
Write-Host "...Running Migration Script on local database: $DatabaseName...";
Invoke-Sqlcmd -ServerInstance . -Username sa -Password $env:LocalDatabaseEnigma -InputFile "$($env:Build_ArtifactStagingDirectory)\scripts.sql";

#Seed Database - 
Write-Host "...Seeding local database: $DatabaseName...";
& $EnvironmentsUtilityDataSeeder -Database Local;

Foreach($file in $IntegrationTestProjects) {
    Write-Host "...Running integration tests on $($file.Name)...";
    dotnet test $file.FullName -p:CollectCoverage=true -p:CoverletOutput=$OutputCoveragePath -p:CoverletOutputFormat="json%2cCobertura" -p:MergeWith="$($OutputCoveragePath)\coverage.json" --logger:"xunit;LogFilePath=$($OutputCoveragePath)\$($file.Name.Replace('.csproj', '')).xml" -p:Exclude="[*]Sopheon.CloudNative.Environments.Data.Migrations.*"
}

#Clean up Database - 
Write-Host "...Cleaning up database: $DatabaseName...";
Invoke-Sqlcmd -ServerInstance . -UserName sa -Password $env:LocalDatabaseEnigma -Query "DROP DATABASE $DatabaseName";

#Setup for Unit Tests here -
$TestProjects = Get-Item -Path "$($env:System_DefaultWorkingDirectory)\source\Sopheon.CloudNative.Environments\**\*.UnitTests.csproj";
Write-Host "...Number of UnitTest projects found: $($TestProjects.Length)...";

Foreach($file in $TestProjects) {
    Write-Host "...Running unit tests on $($file.Name)...";
    dotnet test $file.FullName -p:CollectCoverage=true -p:CoverletOutput=$OutputCoveragePath -p:CoverletOutputFormat="json%2cCobertura" -p:MergeWith="$($OutputCoveragePath)\coverage.json" --logger:"xunit;LogFilePath=$($OutputCoveragePath)\$($file.Name.Replace('.csproj', '')).xml" -p:Exclude="[*]Sopheon.CloudNative.Environments.Data.Migrations.*"
}

#Tear down the integration tests setup -
if(-not $Process.HasExited) {
    #$SubProcess.Kill();
    $Process.Kill();
}

# Zip/Archive Scripts 
Write-Host "Zipping Artfacts for Environment Management...";
& $ZipUtil "a" "-tzip" "$($env:Build_ArtifactStagingDirectory)\EnvironmentManagement" ".\PublishOutput\*" "-xr!build" "-xr!deploy";

Write-Host "Zipping Complete!";