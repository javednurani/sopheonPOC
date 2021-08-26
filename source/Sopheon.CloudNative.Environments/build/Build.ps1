$ZipUtil = "C:\Program Files\7-Zip\7z.exe";

Copy-Item -Path "$($env:System_DefaultWorkingDirectory)/source/Sopheon.CloudNative.Environments/deploy/*" -Destination $env:Build_ArtifactStagingDirectory;

Set-Location -Path "$($env:System_DefaultWorkingDirectory)/source/Sopheon.CloudNative.Environments";

#mkdir PublishOutput

dotnet ef migrations script -p "Sopheon.CloudNative.Environments.Domain\Sopheon.CloudNative.Environments.Domain.csproj" -o "$($env:Build_ArtifactStagingDirectory)\scripts.sql" -i

# Zip/Archive Scripts 
# Write-Host "Zipping Artfacts for Environment Management...";
# & $ZipUtil "a" "-tzip" "$($env:Build_ArtifactStagingDirectory)/EnvironmentManagement_$($env:Build_BuildId)" "$($env:System_DefaultWorkingDirectory)/source/Sopheon.CloudNative.EnvironmentAdmin/PublishOutput/*" "-xr!build" "-xr!deploy";

# Write-Host "Zipping Complete!";