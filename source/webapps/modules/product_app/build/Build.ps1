Import-Module "$($env:System_DefaultWorkingDirectory)\DevOps\PowerShell\CloudNative.Common.psm1";

Copy-Item -Path "$($env:System_DefaultWorkingDirectory)\source\webapps\modules\product_app\deploy\*" -Destination "$($env:Build_ArtifactStagingDirectory)";

# Set location to packages shell-api
Set-Location -Path "$($env:System_DefaultWorkingDirectory)\source\webapps\packages\shell-api";
Write-Host "Location set for shared packages 'Shell-Api'";
npm ci

Write-Host "Building package.json at Shell-Api location"
npm run build

# Set location to packages shared-ui
Set-Location -Path "$($env:System_DefaultWorkingDirectory)\source\webapps\packages\shared-ui";
Write-Host "Location set for shared packages 'shared-ui'";
npm ci

Write-Host "Building package.json at shared-ui location"
npm run build

# Set location to trial app
Set-Location -Path "$($env:System_DefaultWorkingDirectory)\source\webapps\modules\product_app";
Write-Host "Location set for shared packages 'app'";
npm ci

npm install jest-junit

Write-Host "Building package.json at app location"
npm run build

Write-Host "Testing trial location"
npm run test -- --ci --reporters=jest-junit --reporters=default --coverage --coverageReporters=cobertura --watchAll=false

# Zip/Archive Scripts 
Write-Host "Zipping Artfacts for App...";
& $ZipUtil "a" "-tzip" "$($env:Build_ArtifactStagingDirectory)\App_$($env:Build_BuildId)" "$($env:System_DefaultWorkingDirectory)\source\webapps\modules\product_app\dist\*" "-xr!build" "-xr!deploy";


Write-Host "Zipping Complete!";