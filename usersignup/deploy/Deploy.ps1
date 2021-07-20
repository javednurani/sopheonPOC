$ZipUtil = "C:\Program Files\7-Zip\7z.exe";
$B2CAssets = "$($env:System_DefaultWorkingDirectory)/B2CAssets";
$Scripts = "$($env:System_DefaultWorkingDirectory)/Scripts";

# Zip/Archive Scripts 
Write-Host "Zipping Artfacts for UserSignUp...";
& $ZipUtil "x" "$($PSScriptRoot)/UserSignUpSignIn_*.zip" "-o$($B2CAssets)";
& $ZipUtil "x" "$($env:System_DefaultWorkingDirectory)/_DevOpsScripts/DevOps/PowerShell_Scripts_*.zip" "-o$($Scripts)";

# Replace variables on index.html 
& "$($Scripts)/Replace_Config_Variables.ps1" "-PathToFile $($B2CAssets)/website/index.html";