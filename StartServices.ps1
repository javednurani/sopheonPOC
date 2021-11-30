Push-Location "$($PSScriptRoot)\source\setupScripts"


Start-Process powershell -ArgumentList { $host.UI.RawUI.WindowTitle = 'Environment Service'; & '.\StartEnvService.ps1'; };

Start-Process powershell -ArgumentList { $host.ui.RawUI.WindowTitle = 'Product Service'; & '.\StartProductService.ps1' };

Push-Location "..\webapps"

Write-Output "Build NPM and run."
pm run buildpkgs
Write-Output "..................";
#npm run buildmods
Write-Output "............."
npm install
Write-Output "........"
Start-Process powershell -ArgumentList { $host.ui.RawUI.WindowTitle = 'Website Server'; npm run start; Read-Host; };
Write-Output "...."
Write-Output "Done!"
Write-Output "Services and react node server should be running in new windows..."