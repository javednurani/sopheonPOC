$ZipUtil = "C:\Program Files\7-Zip\7z.exe";

Copy-Item -Path "$($env:System_DefaultWorkingDirectory)/source/trial/deploy/Deploy.ps1" -Destination "$($env:Build_ArtifactStagingDirectory)/Deploy.ps1";

# Set location to packages shell-api
Set-Location -Path "$($env:System_DefaultWorkingDirectory)/source/trial/packages/shell-api";
Write-Host "Location set for shared packages 'Shell-Api'";
npm install

Write-Host "Building package.json at Shell-Api location"
npm run build


# Set location to packages shared-ui
Set-Location -Path "$($env:System_DefaultWorkingDirectory)/source/trial/packages/shared-ui";
Write-Host "Location set for shared packages 'shared-ui'";
npm install

Write-Host "Building package.json at shared-ui location"
npm run build


# Set location to trial app
Set-Location -Path "$($env:System_DefaultWorkingDirectory)/source/trial/modules/shell";
Write-Host "Location set for shared packages 'shell'";
npm install

Write-Host "Building package.json at trial location"
npm run build


# Zip/Archive Scripts 
Write-Host "Zipping Artfacts for Shell Trial...";
& $ZipUtil "a" "-tzip" "$($env:Build_ArtifactStagingDirectory)/ShellTrial_$($env:Build_BuildId)" "$($env:System_DefaultWorkingDirectory)/source/trial/modules/shell/dist/*" "-xr!build" "-xr!deploy";

Write-Host "Zipping Complete!";

Write-Output "Build Script Complete"