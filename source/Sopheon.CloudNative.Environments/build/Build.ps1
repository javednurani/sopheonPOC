$ZipUtil = "C:\Program Files\7-Zip\7z.exe";

function Check-LastExitCode {
    if ($LASTEXITCODE -ne 0) {
        throw "Last Exit Code was not 0 but instead: $($LASTEXITCODE)";
    }
}

Copy-Item -Path "$($env:System_DefaultWorkingDirectory)\source\Sopheon.CloudNative.Environments\deploy\*" -Destination $env:Build_ArtifactStagingDirectory;

Set-Location -Path "$($env:System_DefaultWorkingDirectory)\source\Sopheon.CloudNative.Environments";

mkdir PublishOutput

dotnet ef migrations script -p "Sopheon.CloudNative.Environments.Domain\Sopheon.CloudNative.Environments.Domain.csproj" -o "$($env:Build_ArtifactStagingDirectory)\scripts.sql" -i;
Check-LastExitCode;

dotnet publish "Sopheon.CloudNative.Environments.Functions\Sopheon.CloudNative.Environments.Functions.csproj" -o ".\PublishOutput\";
Check-LastExitCode;

# Zip/Archive Scripts 
Write-Host "Zipping Artfacts for Environment Management...";
& $ZipUtil "a" "-tzip" "$($env:Build_ArtifactStagingDirectory)\EnvironmentManagement" ".\PublishOutput\*" "-xr!build" "-xr!deploy";

Write-Host "Zipping Complete!";