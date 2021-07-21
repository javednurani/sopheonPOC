$ZipUtil = "C:\Program Files\7-Zip\7z.exe";

Copy-Item -Path "$($env:System_DefaultWorkingDirectory)/usersignup/deploy/Deploy.ps1" -Destination "$($env:Build_ArtifactStagingDirectory)/Deploy.ps1";

# Set location to packages shell-api
Set-Location -Path "$($env:System_DefaultWorkingDirectory)/source/trial/packages/shell-api";
Write-Host "Location set for shared packages 'Shell-Api'";
npm install
npm install license-checker

Write-Host "Building package.json at Shell-Api location"
npm run build

Write-Host "License check in folder: Shell-Api location"
license-checker --out "$($env:System_DefaultWorkingDirectory)/shell_api_licenses.csv" --csv --failOn 'GPL'


# Set location to packages shared-ui
Set-Location -Path "$($env:System_DefaultWorkingDirectory)/source/trial/packages/shared-ui";
Write-Host "Location set for shared packages 'shared-ui'";
npm install
npm install license-checker

Write-Host "Building package.json at shared-ui location"
npm run build

Write-Host "License check in folder: Shell-Api location"
license-checker --out "$($env:System_DefaultWorkingDirectory)/shared_ui_licenses.csv" --csv --failOn 'GPL'


# Set location to trial app
Set-Location -Path "$($env:System_DefaultWorkingDirectory)/source/trial/modules/shell";
Write-Host "Location set for shared packages 'shell'";
npm install
npm install license-checker

Write-Host "Building package.json at trial location"
npm run build

Write-Host "License check in folder: trial location"
license-checker --out "$($env:System_DefaultWorkingDirectory)/shell_licenses.csv" --csv --failOn 'GPL'



# Zip/Archive Scripts 
Write-Host "Zipping Artfacts for Shell Trial...";
& $ZipUtil "a" "-tzip" "$($env:Build_ArtifactStagingDirectory)/ShellTrial_$($env:Build_BuildId)" "$($env:System_DefaultWorkingDirectory)/source/trial/dist/*" "-xr!build" "-xr!deploy";

Write-Host "Zipping Complete!";