$ZipUtil = "C:\Program Files\7-Zip\7z.exe";
$B2CAssets = "$($env:System_DefaultWorkingDirectory)/B2CAssets";
$Scripts = "$($env:System_DefaultWorkingDirectory)/Scripts";

try {
# Zip/Archive Scripts 
Write-Host "Zipping Artfacts for UserSignUp...";
& $ZipUtil "x" "$($PSScriptRoot)/UserSignUpSignIn_*.zip" "-o$($B2CAssets)";
& $ZipUtil "x" "$($env:System_DefaultWorkingDirectory)/_DevOpsScripts/DevOps/PowerShell_Scripts_*.zip" "-o$($Scripts)";

Write-Host "...Replacing Configuration Variables on website/index.html...";
# Replace variables on index.html 
powershell.exe -file "$($Scripts)/Replace_Config_Variables.ps1" -PathToFile "$($B2CAssets)/website/index.html";

Write-Host "...Replacing Environment Tokens on azureResources/termsOfUse/consentPage.html...";
# Replace tokens on constentPage.html 
powershell.exe -file "$($Scripts)/Replace_Environment_Tokens.ps1" -PathToFile "$($B2CAssets)/azureResources/termsOfUse/consentPage.html";

Write-Host "...Replacing Environment Tokens on azureResources/Login/LoginCustom.html...";
$ErrorActionPreference = 'Stop'
# Replace tokens on LoginCustom.html 
$output = powershell.exe -file "$($Scripts)/Replace_Environment_Tokens.ps1" -PathToFile "$($B2CAssets)/Login/LoginCustom.html" 2>&1;

Write-Host "...Replacing Environment Tokens on azureResources/TrustFrameworkBase.xml...";
# Replace tokens on TrustFrameworkBase.xml 
powershell.exe -file "$($Scripts)/Replace_Environment_Tokens.ps1" -PathToFile "$($B2CAssets)/azureResources/TrustFrameworkBase.xml";


# Upload the TrustFramework .xml files from the GraphApi...


} catch {
    Write-Host "ERROR: ";
    Write-Host $output;
    exit 1;
}