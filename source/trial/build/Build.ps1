Import-Module "$($env:System_DefaultWorkingDirectory)\DevOps\PowerShell\CloudNative.Common.psm1";

Copy-Item -Path "$($env:System_DefaultWorkingDirectory)\source\trial\deploy\*" -Destination "$($env:Build_ArtifactStagingDirectory)";

# Set location to packages shell-api
Set-Location -Path "$($env:System_DefaultWorkingDirectory)\common\controls";
Write-Host "Location set for shared packages 'Sopheon Common Controls'";
npm install

Write-Host "Building package.json at Sopheon Common Controls location"
npm run build

# Set location to packages shell-api
Set-Location -Path "$($env:System_DefaultWorkingDirectory)\source\trial\packages\shell-api";
Write-Host "Location set for shared packages 'Shell-Api'";
npm install

Write-Host "Building package.json at Shell-Api location"
npm run build

# Set location to packages shared-ui
Set-Location -Path "$($env:System_DefaultWorkingDirectory)\source\trial\packages\shared-ui";
Write-Host "Location set for shared packages 'shared-ui'";
npm install

Write-Host "Building package.json at shared-ui location"
npm run build

# Set location to trial app
Set-Location -Path "$($env:System_DefaultWorkingDirectory)\source\trial\modules\shell";
Write-Host "Location set for shared packages 'shell'";
npm install

npm install jest-junit

Write-Host "Building package.json at trial location"
npm run build

Write-Host "Testing trial location"
npm run test -- --ci --reporters=jest-junit --reporters=default --coverage --coverageReporters=cobertura --watchAll=false

# Zip/Archive Scripts 
Write-Host "Zipping Artfacts for Shell Trial...";
& $ZipUtil "a" "-tzip" "$($env:Build_ArtifactStagingDirectory)\ShellTrial_$($env:Build_BuildId)" "$($env:System_DefaultWorkingDirectory)\source\trial\modules\shell\dist\*" "-xr!build" "-xr!deploy";

Write-Host "Zipping Complete!";