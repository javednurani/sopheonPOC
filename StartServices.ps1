Push-Location "$($PSScriptRoot)\source\setupScripts"

powershell -NoExit '.\StartEnvService.ps1';

pwoershell -NoExit '.\StartProductService.ps1';

Push-Location "..\webapps"

Write-Output "Build NPM and run."
npm run buildpkgs
Write-Output "..................";
npm run buildmods
Write-Output "............."
npm install
Write-Output "........"
powershell -NoExit npm run start
Write-Output "...."
Write-Output "Done!"
Write-Output "Services and react node server should be running in new windows..."