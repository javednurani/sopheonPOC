Push-Location "$($PSScriptRoot)\source\setupScripts"

powershell -noexit .\StartEnvService.ps1

powershell -noexit .\StartProductService.ps1

Push-Location "..\webapps"

Write-Output "Build NPM and run."
powershell npm run buildpkgs
Write-Output "..................";
powershell npm run buildmods
Write-Output "............."
powershell npm install
Write-Output "........"
powershell -NoExit npm run start
Write-Output "...."
Write-Output "Done!"
Write-Output "Services and react node server should be running in new windows..."