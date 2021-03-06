Push-Location "$($PSScriptRoot)\source\setupScripts"


Start-Process powershell -ArgumentList ("-NoExit", { $host.UI.RawUI.WindowTitle = 'Environment Service'; & '.\StartEnvService.ps1'; });

Start-Process powershell -ArgumentList ("-NoExit", { $host.UI.RawUI.WindowTitle = 'Product Service'; & '.\StartProductService.ps1'; });

Push-Location "..\webapps"

Write-Output "Build NPM and run."
npm run buildpkgs
Write-Output "..................";
npm run buildmods
Write-Output "............."
npm install
Write-Output "........"
Start-Process powershell -ArgumentList ("-NoExit", { $host.UI.RawUI.WindowTitle = 'Website Server'; npm run start; });
Write-Output "...."
Write-Output "Done!"
Write-Output "Services and react node server should be running in new windows..."