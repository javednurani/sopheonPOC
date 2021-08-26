$ZipUtil = "C:\Program Files\7-Zip\7z.exe";

Copy-Item -Path "$($env:System_DefaultWorkingDirectory)/source/Sopheon.CloudNative.EnvironmentAdmin/deploy/*" -Destination "$($env:Build_ArtifactStagingDirectory)";

Set-Location -Path "$($env:System_DefaultWorkingDirectory)/source/Sopheon.CloudNative.EnvironmentAdmin";

#mkdir PublishOutput

dotnet ef migrations script -p "Sopheon.CloudNative.EnvironmentAdmin.Data\Sopheon.CloudNative.EnvironmentAdmin.Data.csproj" -o "$($env:Build_ArtifactStagingDirectory)\scripts.sql" -i

# Zip/Archive Scripts 
# Write-Host "Zipping Artfacts for Environment Management...";
# & $ZipUtil "a" "-tzip" "$($env:Build_ArtifactStagingDirectory)/EnvironmentManagement_$($env:Build_BuildId)" "$($env:System_DefaultWorkingDirectory)/source/Sopheon.CloudNative.EnvironmentAdmin/PublishOutput/*" "-xr!build" "-xr!deploy";

# Write-Host "Zipping Complete!";