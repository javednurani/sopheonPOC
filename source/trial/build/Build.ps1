$ZipUtil = "C:\Program Files\7-Zip\7z.exe";

Copy-Item -Path "$($env:System_DefaultWorkingDirectory)/usersignup/deploy/Deploy.ps1" -Destination "$($env:Build_ArtifactStagingDirectory)/Deploy.ps1";


# Set location to packages shell-api
Set-Location -Path "$($env:System_DefaultWorkingDirectory)/source/trial/packages/shell-api";
Write-Host "Location set for shared packages 'Shell-Api'";
npm install




# Zip/Archive Scripts 
Write-Host "Zipping Artfacts for Shell Trial...";
& $ZipUtil "a" "-tzip" "$($env:Build_ArtifactStagingDirectory)/ShellTrial_$($env:Build_BuildId)" "$($env:System_DefaultWorkingDirectory)/source/trial/dist/*" "-xr!build" "-xr!deploy";

Write-Host "Zipping Complete!";